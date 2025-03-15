using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GerenciamentoDeVendas.Models;

namespace GerenciamentoDeVendas.Functions
{
    public class CustomerDAO
    {
        //validacao email
        private bool EmailExists(string email)
        {
            var query = "SELECT COUNT(1) FROM Customers WHERE Email = @Email";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private readonly string _connectionString;
        public CustomerDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        //inserir um novo cliente
        public List<Customer> GetAllCustomers() //list para armazenar os clientes
        {
            var customers = new List<Customer>();
            var query = "SELECT * FROM Customers";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            while (reader.Read()) //loop para ler todos os cutomers
                            {
                                customers.Add(new Customer
                                {
                                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customers: {ex.Message}");
                throw;
            }
            return customers;
        }
        public int InsertCustomer(Customer customer)
        {
            //validacao de dados
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(customer.Email) || !IsValidEmail(customer.Email))
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (EmailExists(customer.Email))
            {
                throw new ArgumentException("A customer with this email already exists");
            }

            if (!string.IsNullOrWhiteSpace(customer.Phone) && customer.Phone.Length > 15) // Exemplo de limite de 15 caracteres
            {
                throw new ArgumentException("Phone number cannot exceed 15 characters");
            }

            var insertSql = "INSERT INTO Customers (Name, Email, Phone) VALUES (@Name, @Email, @Phone); SELECT SCOPE_IDENTITY();";
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", customer.Name);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@Phone", customer.Phone);

                        return Convert.ToInt32(command.ExecuteScalar()); //Retorna Id
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error inserting customer: {ex.Message}");
                throw; // exception para o chamador poder lidar com ela
            }
        }
        //ver um cliente pelo ID
        public Customer GetCustomerById(int customerId) 
        {
            if(customerId < 0)
            {
                throw new ArgumentException("CustomerID must be greater than zero");
            }
            var query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) //If para garantir que so vai acionar se houver pelo menos um registro retornado pela consulta
                            {
                                return new Customer
                                {
                                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Customer search error: {ex.Message}");
            }
            return null; //Retorna nada se nao achar o cliente pelo id
        }
        //att as infos de algum cliente
        public bool UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
            }

            if (customer.CustomerID <= 0)
            {
                throw new ArgumentException("CustomerID must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                throw new ArgumentException("Customer name cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(customer.Email) || !IsValidEmail(customer.Email))
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (!string.IsNullOrWhiteSpace(customer.Phone) && customer.Phone.Length > 15) //limite de 15 caracteres
            {
                throw new ArgumentException("Phone number cannot exceed 15 characters.");
            }

            var updateQuery = "UPDATE Customers SET Name = @Name, Email = @Email, Phone = @Phone WHERE CustomerID = @CustomerID";
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", customer.Name);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@Phone", customer.Phone);
                        command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

                        int rowsAffected = command.ExecuteNonQuery();//linhas afetadas
                        return rowsAffected > 0; //retorna true se atualizar
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating the client: {ex.Message}");
                throw;
            }
        }
        //deletar um cliente
        public bool DeleteCustomer(int customerId) 
        {
            if (customerId < 0)
            {
                throw new ArgumentException("CustomerID must be greater than zero");
            }

            var deleteQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
            
            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        int rowsAffected = command.ExecuteNonQuery();//linhas afetadas
                        return rowsAffected > 0; //retorna true se deletar
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error when deleting customer: {ex.Message}");
                throw;
            }
        }
    }
}
