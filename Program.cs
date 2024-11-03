using dotnet_stock.Data;
using dotnet_stock.Interfaces;
using dotnet_stock.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// dotnet add package Microsoft.EntityFrameworkCore.InMemory
// using Microsoft.EntityFrameworkCore;
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQLServer")));

// Add Product service
// AddTransient:
// ทุกครั้งที่มีการเรียกใช้ Service นี้ในโค้ด จะมีการสร้าง Instance ใหม่ของ ProductService เสมอ
// ใช้เหมาะกับ Service ที่ไม่มีการเก็บสถานะ (stateless) หรือเมื่อ Service ต้องการสร้าง Instance ใหม่ทุกครั้งที่ถูกเรียกใช้งาน
// builder.Services.AddTransient<IProductservice, ProductService>();

// AddScoped:
// ในแต่ละ Request (เช่น การเรียก HTTP Request หนึ่งครั้ง) จะมีการสร้าง Instance ของ ProductService ขึ้นมาแค่หนึ่งครั้ง และจะใช้ Instance เดิมนั้นตลอดในระหว่างการทำงานของ Request นั้น ๆ
// เหมาะกับ Service ที่ต้องการเก็บสถานะไว้ในช่วงของการ Request หรือจำเป็นต้องใช้งานแบบแชร์กันภายใน Request นั้น ๆ แต่ไม่แชร์กับ Request อื่น
builder.Services.AddScoped<IProductservice, ProductService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
