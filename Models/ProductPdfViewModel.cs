// Models/ProductPdfViewModel.cs
using System.Collections.Generic;
using System; // Untuk DateTime

namespace AegislabsTechnicalTest.Models
{
    public class ProductPdfViewModel
    {
        public List<Product> Products { get; set; }
        public string ReportDate { get; set; } = DateTime.Now.ToString("dd MMMM yyyy HH:mm"); // Format tanggal dan waktu
        public string Title { get; set; } = "Daftar Produk";
    }
}