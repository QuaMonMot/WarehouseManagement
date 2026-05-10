using Warehouse.BLL.Interfaces;
using Warehouse.BLL.Services;
using Warehouse.DAL;
using Warehouse.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Lấy chuỗi kết nối từ file cấu hình appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Myconnection");

// Đăng ký DatabaseHelper vào hệ thống Dependency Injection
builder.Services.AddScoped(sp => new DatabaseHelper(connectionString));

// 1. Đăng ký các Repository (Tầng DAL)
builder.Services.AddScoped<SupplierRepository>();
builder.Services.AddScoped<InventoryRepository>();
builder.Services.AddScoped<ReportRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<ProductRepository>();

// 2. Đăng ký các Service (Tầng BLL)
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<IProductService, ProductService>();

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
