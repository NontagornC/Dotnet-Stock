using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_stock.Data;
using dotnet_stock.DTO.Products;
using Microsoft.AspNetCore.Mvc;
//using dotnet_stock.Models;

namespace dotnet_stock.Controllers
{
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
            public IActionResult GetProducts()
            {
                  var products = this.DatabaseContext.Products.ToList();
                  return Ok(products);
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


                  // lin q
                  var selectedProduct = this.DatabaseContext.Products.Select(ProductResponse.FromProduct).Where(p => p.ProductId == id).FirstOrDefault();
                  return Ok(selectedProduct);
            }

      }
}