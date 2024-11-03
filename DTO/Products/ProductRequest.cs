namespace dotnet_stock.DTO.Products
{
      public class ProductRequest
      {
            public int? ProductId { get; set; }

            public string Name { get; set; }

            public int Stock { get; set; }

            public decimal Price { get; set; }

            public int CategoryId { get; set; }
            public List<IFormFile>? FormFiles { get; set; }
      }
}