using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GerenciamentoDeVendas.Models;

namespace GerenciamentoDeVendas.Functions
{
    class OrderItemDAO
    {
        private readonly string _connectionString;
        public OrderItemDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<OrderItem> GetAllOrderItems()
        {
            var orderItems = new List<OrderItem>();
            var query = "SELECT * FROM OrderItems";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderItems.Add(new OrderItem
                                {
                                    OrderItemID = reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving order items: {ex.Message}");
                throw;
            }
            return orderItems;
        }
        public int InsertOrderItem(OrderItem orderItem)
        {
            if (orderItem.OrderID < 0)
            {
                throw new ArgumentException("OrderId must be greater than zero");
            }

            if(orderItem.ProductID < 0)
            {
                throw new ArgumentException("ProductID must be greater than zero");
            }

            if(orderItem.Quantity < 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            if(orderItem.UnitPrice < 0)
            {
                throw new ArgumentException("UnitPrice cannot be negative");
            }

            var insertSql = "INSERT INTO OrderItems (OrderID, ProductID, Quantity, UnitPrice) VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice); SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                        command.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                        command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                        command.Parameters.AddWithValue("@UnitPrice", orderItem.UnitPrice);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting new order item: {ex.Message}");
                throw;
            }
        }
        public OrderItem GetOrderItemById(int orderItemId)
        {
            if(orderItemId < 0)
            {
                throw new ArgumentException("OrdemItemID must be greater than zero");
            }

            var query = "SELECT * FROM OrderItems WHERE OrderItemID = @OrderItemID;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderItemID", orderItemId);
                        using(var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new OrderItem
                                {
                                    OrderItemID = reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order item search error: {ex.Message}");
                throw;
            }
            return null; //null se o item n ser encontrado
        }
        public bool UpdateOrderItem(OrderItem orderItem)
        {             
            if (orderItem.OrderItemID <= 0)
            {
                throw new ArgumentException("OrderItemID must be greater than zero");
            }

            if (orderItem.OrderID <= 0)
            {
                throw new ArgumentException("OrderID must be greater than zero");
            }

            if (orderItem.ProductID <= 0)
            {
                throw new ArgumentException("ProductID must be greater than zero");
            }

            if (orderItem.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            if (orderItem.UnitPrice <= 0)
            {
                throw new ArgumentException("UnitPrice must be greater than zero");
            }
            
            var updateQuery = "UPDATE OrderItems SET OrderID = @OrderId, ProductID = @ProductID, Quantity = @Quantity, UnitPrice = @UnitPrice WHERE OrderItemID = @OrderItemID";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderItemID", orderItem.OrderItemID);
                        command.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                        command.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                        command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                        command.Parameters.AddWithValue("@UnitPrice", orderItem.UnitPrice);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error updating the order item: {ex.Message}");
                throw;
            }
        }
        public bool DeleteOrderItem(int orderItemId) 
        {
            if (orderItemId < 0) 
            {
                throw new ArgumentException("OrderItemID must be greater than zero");
            }

            var deleteQuery = "DELETE FROM OrderItems WHERE OrderItemID = @OrderItemID";
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderItemID", orderItemId);
                        int rowsAffectd = command.ExecuteNonQuery();
                        return rowsAffectd > 0;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error deleting the order item: {ex.Message}");
                throw;
            }
        }
    }
}
