// Repositories/ProductRepository.cs
using System.Collections.Generic;
using System.Data; // Untuk CommandType
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; // Untuk SqlConnection, SqlCommand, SqlDataReader
using AegislabsTechnicalTest.Models; // Pastikan namespace model Anda sesuai

namespace AegislabsTechnicalTest.Repositories
{
    public class ProductRepository : IProductRepository // Implementasikan interface
    {
        private readonly string _connectionString; // Untuk menyimpan connection string

        // Constructor untuk menerima connection string melalui Dependency Injection
        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>();

            // Menggunakan 'using' statement untuk memastikan SqlConnection tertutup dan dibuang
            using (var connection = new SqlConnection(_connectionString))
            {
                // Membuka koneksi secara asinkron
                await connection.OpenAsync();

                // Menggunakan 'using' statement untuk memastikan SqlCommand dibuang
                using (var command = new SqlCommand("GetProductsWithLowStock", connection)) // Nama Stored Procedure
                {
                    command.CommandType = CommandType.StoredProcedure; // Penting: Tentukan ini adalah Stored Procedure

                    // Mengeksekusi command dan membaca data secara asinkron
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Iterasi setiap baris hasil dari Stored Procedure
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32("Id"), // Ambil data Id (int)
                                Name = reader.GetString("Name"), // Ambil data Name (string)
                                Price = reader.GetDecimal("Price"), // Ambil data Price (decimal)
                                Stock = reader.GetInt32("Stock") // Ambil data Stock (int)
                            });
                        }
                    }
                }
            }
            return products;
        }
    }
}