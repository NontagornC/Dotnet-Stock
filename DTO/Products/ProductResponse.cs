namespace dotnet_stock.DTO.Products
{
      public class ProductResponse
      {
            public int ProductId { get; set; }
            public string Name { get; set; } = "";
            public string? Image { get; set; }
            public int Stock { get; set; }
            public decimal Price { get; set; }

            public string CategoryName { get; set; } = "";

            public static ProductResponse FromProduct(dotnet_stock.Entities.Product product)
            {
                  return new ProductResponse
                  {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Image = product.Image,
                        Stock = product.Stock,
                        Price = product.Price,
                        CategoryName = product.Category.Name
                  };

            }
      }


}