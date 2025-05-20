// Program.cs
using OfficeOpenXml;
using AegislabsTechnicalTest.Services;
using Rotativa.AspNetCore;
using System;
using Microsoft.Extensions.Configuration;
using AegislabsTechnicalTest.Repositories; // Tambahkan ini

// --- Konfigurasi Lisensi EPPlus (untuk EPPlus versi 5.x.x) ---
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// --- Akhir Konfigurasi Lisensi EPPlus ---


var builder = WebApplication.CreateBuilder(args);

// --- Registrasi Connection String ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(connectionString);
// --- Akhir Registrasi Connection String ---


// Add services to the container.
builder.Services.AddControllersWithViews();

// --- Ubah dan Tambahkan Registrasi Service dan Repository di sini ---
// Registrasikan ProductRepository sebagai implementasi dari IProductRepository
// Menggunakan AddScoped atau AddTransient, AddSingleton untuk objek yang tidak berubah (connection string)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Ubah registrasi ProductService agar menerima IProductRepository
builder.Services.AddScoped<ProductService>(); // ProductService sekarang akan di-inject IProductRepository
// --- Akhir Ubah dan Tambahkan Registrasi Service dan Repository ---


// --- Konfigurasi Rotativa (PENTING!) ---
Rotativa.AspNetCore.RotativaConfiguration.Setup(builder.Environment.WebRootPath, "Rotativa");
// --- Akhir Konfigurasi Rotativa ---


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();