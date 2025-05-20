// Repositories/IProductRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks; // Tambahkan ini untuk Task
using AegislabsTechnicalTest.Models; // Pastikan namespace model Anda sesuai

namespace AegislabsTechnicalTest.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(); // Method asinkron untuk mengambil produk
    }
}