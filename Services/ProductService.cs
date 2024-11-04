using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_stock.Data;
using dotnet_stock.DTO.Products;
using dotnet_stock.Entities;
using dotnet_stock.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet_stock.Services
{
    public class ProductService : IProductservice
    {
        public DatabaseContext DatabaseContext { get; }

        public IUploadFileService UploadFileService { get; set; }

        public ProductService(DatabaseContext databaseContext, IUploadFileService uploadFileService)
        {
            this.DatabaseContext = databaseContext;
            this.UploadFileService = uploadFileService;
        }
        public async Task<IEnumerable<Product>> FindAll()
        {
            // without join
            // var products = this.DatabaseContext.Products.ToList();

            // with join
            var products = await this.DatabaseContext.Products.Include(p => p.Category).ToListAsync();
            // return Ok(products);

            return products;
        }
        // ถ้าเป็น async ต้องมี task เสมอหรือไม่มีการใช้ async ก็จริงแต่เป็น async ก็ต้องใส่ task

        public async Task<Product?> FindById(int id)
        {
            var selectedProduct = await this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.ProductId == id).FirstOrDefaultAsync();
            return selectedProduct;
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            var result = await this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            return result;
        }

        public async Task Create(Product product)
        {
            this.DatabaseContext.Products.Add(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            this.DatabaseContext.Products.Remove(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            this.DatabaseContext.Products.Update(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles)
        {
            string errorMesage = String.Empty;
            string imageName = String.Empty;
            if (UploadFileService.IsUpload(formFiles))
            {
                errorMesage = UploadFileService.Validation(formFiles);
                if (String.IsNullOrEmpty(errorMesage))
                {
                    imageName = (await UploadFileService.UploadImages(formFiles))[0];
                }
            }
            return (errorMesage, imageName);
        }
    }

}