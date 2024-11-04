using System.Net;
using dotnet_stock.DTO.Products;
using dotnet_stock.Entities;
using dotnet_stock.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
//using dotnet_stock.Models;

namespace dotnet_stock.Controllers
{
      // ApiController จะเป็นตัว validate ในระดับ controller ถ้าไม่ต้องการ check ก็ comment ApiController ทิ้งสะ
      [Route("api/[controller]")]
      [ApiController]
      public class ProductsController : ControllerBase
      {
            public IProductservice Productservice { get; }
            public ProductsController(IProductservice productservice)
            {
                  this.Productservice = productservice;

            }

            [HttpGet("")]
            // ใช้ service
            public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsAsync()
            {
                  return (await this.Productservice.FindAll()).Select(ProductResponse.FromProduct).ToList();
            }

            // ไม่ใช้ service
            // public ActionResult<IEnumerable<ProductResponse>> GetProducts()
            // // public IActionResult GetProducts()
            // {
            //       // without join
            //       // var products = this.DatabaseContext.Products.ToList();

            //       // with join
            //       var products = this.DatabaseContext.Products.Include(p => p.Category).Select(ProductResponse.FromProduct).ToList();
            //       // return Ok(products);

            //       return products;
            // }

            [HttpGet("{id}")]
            // with service
            public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int id)
            {
                  var selectedProduct = await this.Productservice.FindById(id);
                  if (selectedProduct == null)
                  {
                        return NotFound();
                  }
                  return ProductResponse.FromProduct(selectedProduct);

            }
            // without service
            // public IActionResult GetProductById(int id)
            // {
            // With out DTO
            // var product = this.DatabaseContext.Products.Find(id);
            // return Ok(product);

            // with DTO  ----> data transfer object ป้องการโชว์ข้อมูลทั้งหมด
            // var selectedProduct = this.DatabaseContext.Products.Find(id);
            // if (selectedProduct != null)
            // {

            //       return Ok(ProductResponse.FromProduct(selectedProduct));

            // }
            // return NotFound();


            // lin q with join category
            // var selectedProduct = this.DatabaseContext.Products.Include(p => p.Category).Select(ProductResponse.FromProduct).Where(p => p.ProductId == id).FirstOrDefault();
            // return Ok(selectedProduct);
            // }


            [HttpGet("search")]
            public async Task<ActionResult<IEnumerable<ProductResponse>>> Search([FromQuery] string name)
            {
                  // .Select เหมือนการ map ในโค้ดข้างล่างคือได้ข้อมูลจาก service มาแล้วเอามา map โดย .Select ผ่าน Method FromProduct และ return กลับไปเป็น list ผ่าน ToList
                  var result = (await this.Productservice.Search(name)).Select(ProductResponse.FromProduct).ToList();
                  return result;

            }

            // !! without service
            // public ActionResult<IEnumerable<ProductResponse>> Search([FromQuery] string name)
            // {
            //       // FromQuery เป็นการบอก dotnet ว่าจะต้องดึงค่าจาก query string
            //       // เช่น http://localhost:5000/api/Products/search?name=product_name
            //       var result = this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.ToLower().Contains(name.ToLower())).Select(ProductResponse.FromProduct).ToList();
            //       return result;
            // }

            // [HttpPost("")]
            // without service
            // public IActionResult AddProduct([FromForm] ProductRequest productRequest)
            // {
            //       var categoryExists = this.DatabaseContext.Categories.Any(c => c.CategoryId == productRequest.CategoryId);
            //       if (!categoryExists)
            //       {
            //             return BadRequest("Invalid CategoryId. The specified category does not exist.");
            //       }

            //       // without mapster
            //       // var product = new Product()
            //       // {
            //       //       Name = productRequest.Name,
            //       //       Stock = productRequest.Stock,
            //       //       Price = productRequest.Price,
            //       //       CategoryId = productRequest.CategoryId
            //       // };
            //       // product.Image = "";

            //       // with mapster
            //       var product = productRequest.Adapt<Product>();

            //       this.DatabaseContext.Products.Add(product);
            //       this.DatabaseContext.SaveChanges();
            //       return StatusCode((int)HttpStatusCode.Created, product);
            // }
            [HttpPost("")]
            public async Task<ActionResult<Product>> AddProductAsync([FromForm] ProductRequest productRequest)
            {
                  string finalImageName = "";
                  if (productRequest.FormFiles != null)
                  {
                        (string errorMessage, string imageName) = await Productservice.UploadImage(productRequest.FormFiles);
                        if (!String.IsNullOrEmpty(errorMessage))
                        {
                              return BadRequest();
                        }
                        finalImageName = imageName;

                  }
                  var product = productRequest.Adapt<Product>();
                  product.Image = finalImageName;
                  await this.Productservice.Create(product);
                  return StatusCode((int)HttpStatusCode.Created, product);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProductAsync(int id)
            {
                  var product = await this.Productservice.FindById(id);
                  if (product == null)
                  {
                        return NotFound();
                  }

                  await this.Productservice.Delete(product);
                  return StatusCode((int)HttpStatusCode.Created, product);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateProductAsync(int id, [FromForm] ProductRequest productRequest)
            {
                  if (id != productRequest.ProductId)
                  {
                        return BadRequest();
                  }

                  var product = await this.Productservice.FindById(id);
                  if (product == null)
                  {
                        return NotFound();
                  }

                  // (string errorMessage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
                  // if (!String.IsNullOrEmpty(errorMessage))
                  // {
                  //     return BadRequest();
                  // }
                  // if (!String.IsNullOrEmpty(imageName))
                  // {
                  //     product.Image = imageName;
                  // }

                  productRequest.Adapt(product);
                  await Productservice.Update(product);
                  return Ok(ProductResponse.FromProduct(product));

            }

      }
}