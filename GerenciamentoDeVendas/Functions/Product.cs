using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GerenciamentoDeVendas.Models;


namespace GerenciamentoDeVendas.Functions
{
    public class ProductDAO
    {
        private readonly string _connectionString;

        public ProductDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        //msm conceito do Customer
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            var query = "SELECT * FROM Products";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when reading products: {ex.Message}");
                throw;
            }
            return products;
        }
        public int InsertProduct(Product product)
        {
            //validar antes das operacoes
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("The name of product cannot be empty");
            }
            if (product.Price < 0)
                throw new ArgumentException("The price cannot be negative");

            if (product.Stock < 0)
                throw new ArgumentException("The stock cannot be negative");

            var insertSql = "INSERT INTO Products (Name, Description, Price, Stock) VALUES (@Name, @Description, @Price, @Stock); SELECT SCOPE_IDENTITY();";
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Stock", product.Stock);

                        return Convert.ToInt32(command.ExecuteScalar()); //retornar o id gerado
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error inserting product: {ex.Message}");
                throw;
            }
        }
        public Product GetProductById(int productId) 
        {
            if (productId < 0)
            {
                throw new ArgumentException("PoductID must be greater than zero");
            }
            var query = "SELECT * FROM Products WHERE ProductID = @ProductID";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Product search error: {ex.Message}");
            }
            return null; //retorna nada se o id n achar o id do produto
        }
        public bool UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            if (product.ProductID <= 0)
            {
                throw new ArgumentException("ProductID must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be null or empty");
            }

            if (product.Description != null && product.Description.Length > 500) // Exemplo de limite de 500 caracteres
            {
                throw new ArgumentException("Description cannot exceed 500 characters");
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            if (product.Stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative");
            }

            var updateQuery = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Stock = @Stock WHERE ProductID = @ProductID";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Stock", product.Stock);
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        
                        int rowsAffected = command.ExecuteNonQuery(); //linhas afetadas
                        return rowsAffected > 0; //retorna true se att
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error updating the product: {ex.Message}");
                throw;
            }
        }
        public bool DeleteProduct(int productId) 
        {
            if(productId < 0)
            {
                throw new ArgumentException("ProductID must be greater than zero");
            }

            var deleteQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
            
            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        int rowsAffected = command.ExecuteNonQuery(); //linhas afetadas
                        return rowsAffected > 0;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error when deleting product: {ex.Message}");
                throw;
            }
        }
    }
}
