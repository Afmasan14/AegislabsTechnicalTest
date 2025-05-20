// Services/ProductService.cs
using AegislabsTechnicalTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks; // Tambahkan ini
using AegislabsTechnicalTest.Repositories; // Tambahkan ini

namespace AegislabsTechnicalTest.Services
{
    public class ProductService
    {
        // Ubah ini dari private readonly ProductService _productService;
        // menjadi IProductRepository
        private readonly IProductRepository _productRepository;

        // Constructor untuk Dependency Injection
        // Ubah parameter dari ProductService menjadi IProductRepository
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Ubah method ini menjadi asinkron
        public async Task<List<Product>> GetProducts() // Tambahkan async Task dan hapus data dummy
        {
            // Panggil method dari repository untuk mendapatkan data dari database
            return await _productRepository.GetProductsAsync();
        }
    }
}