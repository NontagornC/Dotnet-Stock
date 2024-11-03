using dotnet_stock.Entities;

namespace dotnet_stock.Interfaces
{
      public interface IProductservice
      {
            Task<IEnumerable<Product>> FindAll();

            Task<Product> FindById(int id);

            Task Create(Product product);

            Task Update(Product product);

            Task Delete(Product product);

            Task<IEnumerable<Product>> Search(string name);

            Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles);
      }
}