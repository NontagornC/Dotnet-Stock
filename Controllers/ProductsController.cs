using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet_stock.Data;
using dotnet_stock.DTO.Products;
using dotnet_stock.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using dotnet_stock.Models;

namespace dotnet_stock.Controllers
{
      // ApiController จะเป็นตัว validate ในระดับ controller ถ้าไม่ต้องการ check ก็ comment ApiController ทิ้งสะ
      [Route("api/[controller]")]
      [ApiController]
      public class ProductsController : ControllerBase
      {
            public DatabaseContext DatabaseContext { get; set; }
            public ProductsController(DatabaseContext databaseContext)
            {
                  this.DatabaseContext = databaseContext;

            }

            [HttpGet("")]
            public ActionResult<IEnumerable<ProductResponse>> GetProducts()
            // public IActionResult GetProducts()
            {
                  // without join
                  // var products = this.DatabaseContext.Products.ToList();

                  // with join
                  var products = this.DatabaseContext.Products.Include(p => p.Category).Select(ProductResponse.FromProduct).ToList();
                  // return Ok(products);

                  return products;
            }

            [HttpGet("{id}")]
            public IActionResult GetProductById(int id)
            {
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
                  var selectedProduct = this.DatabaseContext.Products.Include(p => p.Category).Select(ProductResponse.FromProduct).Where(p => p.ProductId == id).FirstOrDefault();
                  return Ok(selectedProduct);
            }


            [HttpGet("search")]
            public ActionResult<IEnumerable<ProductResponse>> Search([FromQuery] string name)
            {
                  // FromQuery เป็นการบอก dotnet ว่าจะต้องดึงค่าจาก query string
                  // เช่น http://localhost:5000/api/Products/search?name=product_name
                  var result = this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.ToLower().Contains(name.ToLower())).Select(ProductResponse.FromProduct).ToList();
                  return result;
            }

            [HttpPost("")]
            public IActionResult AddProduct([FromForm] ProductRequest productRequest)
            {
                  var categoryExists = this.DatabaseContext.Categories.Any(c => c.CategoryId == productRequest.CategoryId);
                  if (!categoryExists)
                  {
                        return BadRequest("Invalid CategoryId. The specified category does not exist.");
                  }

                  var product = new Product()
                  {
                        Name = productRequest.Name,
                        Stock = productRequest.Stock,
                        Price = productRequest.Price,
                        CategoryId = productRequest.CategoryId
                  };
                  product.Image = "";

                  this.DatabaseContext.Products.Add(product);
                  this.DatabaseContext.SaveChanges();
                  return StatusCode((int)HttpStatusCode.Created, product);
            }

      }
}