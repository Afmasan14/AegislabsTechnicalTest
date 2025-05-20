// Controllers/ReportController.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks; // Pastikan ini ada
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml; // Untuk EPPlus
using OfficeOpenXml.Style; // Untuk styling EPPlus
using AegislabsTechnicalTest.Models; // Untuk model Product
using AegislabsTechnicalTest.Services; // Untuk ProductService
using Rotativa.AspNetCore; // Untuk Rotativa
using Rotativa.AspNetCore.Options; // Opsional, tapi bisa membantu jika ada ambiguitas

namespace AegislabsTechnicalTest.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly ProductService _productService; // Service kita

        // Constructor: ProductService akan di-inject di sini oleh Dependency Injection
        public ReportController(ILogger<ReportController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Method untuk download Excel
        // Sekarang asinkron (async) karena ProductService.GetProducts() juga asinkron
        public async Task<IActionResult> DownloadExcel()
        {
            // Panggil service untuk mendapatkan data produk secara asinkron
            // Menggunakan 'await' untuk menunggu hasil dari Task<List<Product>>
            var products = await _productService.GetProducts();

            // Konten dan logika EPPlus
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");

                // Header
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Price";
                worksheet.Cells[1, 4].Value = "Stock";

                // Apply bold to headers
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Data dari database
                // Iterasi melalui daftar produk yang sudah didapatkan dari database
                for (int i = 0; i < products.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = products[i].Id;
                    worksheet.Cells[i + 2, 2].Value = products[i].Name;
                    worksheet.Cells[i + 2, 3].Value = products[i].Price;
                    worksheet.Cells[i + 2, 4].Value = products[i].Stock;
                }

                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Convert the Excel package to a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the file for download
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
            }
        }

        // Method untuk download PDF
        // Juga diubah menjadi asinkron (async)
        public async Task<IActionResult> DownloadPdf()
        {
            // Panggil service untuk mendapatkan data produk secara asinkron
            // Menggunakan 'await' untuk menunggu hasil dari Task<List<Product>>
            var products = await _productService.GetProducts();

            // Menggunakan Rotativa untuk membuat PDF dari view
            // Meneruskan List<Product> (hasil 'await') ke view, bukan Task<List<Product>>
            return new ViewAsPdf("PdfView", products)
            {
                FileName = "Products.pdf",
                PageSize = Size.A4, // Menggunakan Rotativa.AspNetCore.Options.Size
                PageOrientation = Orientation.Portrait, // Menggunakan Rotativa.AspNetCore.Options.Orientation
                PageMargins = { Left = 10, Right = 10, Top = 10, Bottom = 10 }
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}