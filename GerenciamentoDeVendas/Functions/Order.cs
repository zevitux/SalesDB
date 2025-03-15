using System;
using System.Data;
using GerenciamentoDeVendas.Models;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace GerenciamentoDeVendas.Functions
{
    public class OrderDAO
    {
        private readonly string _connectionString;

        public OrderDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            var query = "SELECT * FROM Orders";

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
                                orders.Add(new Order
                                {
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving orders: {ex.Message}");
                throw;
            }
            return orders; 
        }
        //verificacao no banco de dados
        private bool CustomerExists(int customerId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(1) FROM Customers WHERE CustomerID = @CustomerID"; //conta quantos clientes tem o id especificado (1)
                using(var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    return (int)command.ExecuteScalar() > 0; //return true se o count for > 0
                }
            }
        }
        public int InsertOrder(Order order)
        {
            if (!CustomerExists(order.CustomerID)) //validando o customerid
                throw new ArgumentException("Invalid CustomerID");

            if (order.TotalAmount < 0) //validando totalamount
                throw new ArgumentException("TotalAmount cannot be negative");

            if (order.OrderDate > DateTime.Now)
                throw new ArgumentException("Order date cannot be in future");


            var insertSql = "INSERT INTO Orders (OrderDate, TotalAmount, CustomerID) VALUES (@OrderDate, @TotalAmount, @CustomerID); SELECT SCOPE_IDENTITY();";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = (object)order.TotalAmount ?? DBNull.Value; //TotalAmount = null
                        command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        
                        int orderId = Convert.ToInt32(command.ExecuteScalar()); //obtem o ID gerado

                        if(order.Invoice != null)
                        {
                            var invoiceInsertSql = "INSERT INTO Invoices (OrderID, InvoiceDate, Amount) VALUES (@OrderID, @InvoiceDate, @Amount);";
                            using(var invoiceCommand = new SqlCommand(invoiceInsertSql, connection))
                            {
                                invoiceCommand.Parameters.AddWithValue("@OrderID", orderId);
                                invoiceCommand.Parameters.AddWithValue("@InvoiceDate", order.Invoice.InvoiceDate);
                                invoiceCommand.Parameters.AddWithValue("@Amount", order.Invoice.Amount);

                                invoiceCommand.ExecuteNonQuery(); //executa a inserction da fatura
                            }
                        }
                        return orderId; //retorna o Id do pedido
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error inserting order: {ex.Message}");
                throw;
            }
        }
        public Order GetOrderById(int orderId) 
        {
            if (orderId < 0)
            {
                throw new ArgumentException("OrderID must be greater than zero");
            }
            var query = "SELECT * FROM Orders WHERE OrderID = @OrderID";
            
            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Order
                                {
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
                                };
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Order search error: {ex.Message}");                
            }
            return null;
        }
        public bool UpdateOrder(Order order) 
        {           
            if (order.OrderID <= 0)
            {
                throw new ArgumentException("OrderID must be greater than zero");
            }

            if (order.CustomerID <= 0)
            {
                throw new ArgumentException("CustomerID must be greater than zero");
            }

            if (!CustomerExists(order.CustomerID))
            {
                throw new ArgumentException("Invalid CustomerID");
            }                        

            var updateQuery = "UPDATE Orders SET CustomerID = @CustomerID, OrderDate = @OrderDate, TotalAmount = @TotalAmount WHERE OrderID = @OrderID";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                        command.Parameters.AddWithValue("@OrderID", order.OrderID);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0; //return true
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error updating the order: {ex.Message}");
                throw;
            }
        }
        public bool DeleteOrder(int orderId) 
        {
            if(orderId < 0)
            {
                throw new ArgumentException("OrderId must be greater than zero");
            }

            var deleteQuery = "DELETE FROM Orders WHERE OrderID = @OrderID";
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error when deleting order: {ex.Message}");
                throw;
            }
        }
    }
}
