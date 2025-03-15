using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GerenciamentoDeVendas.Models;

namespace GerenciamentoDeVendas.Functions
{
    class InvoiceDAO
    {
        private readonly string _connectionString;
        public InvoiceDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Invoice> GetAllInvoices()
        {
            var invoices = new List<Invoice>();
            var query = "SELECT * FROM Invoices";

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
                                invoices.Add(new Invoice
                                {
                                    InvoiceID = reader.GetInt32(reader.GetOrdinal("InvoiceID")),
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving invoices: {ex.Message}");
                throw;
            }
            return invoices;
        }
        public int InsertInvoice(Invoice invoice)
        {
            if (invoice.OrderID <= 0)
            {
                throw new ArgumentException("OrderID must be greater than zero");
            }

            if (invoice.InvoiceDate == default)
            {
                throw new ArgumentException("InvoiceDate must be a valid date");
            }

            if (invoice.Amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }

            var insertSql = "INSERT INTO Invoices (OrderID, InvoiceDate, Amount) VALUES (@OrderID, @InvoiceDate, @Amount); SELECT SCOPE_IDENTITY();";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", invoice.OrderID);
                        command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                        command.Parameters.AddWithValue("@Amount", invoice.Amount);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error inserting new invoice: {ex.Message}");
                throw;
            }
        }
        public Invoice GetInvoiceById(int invoiceId)
        {
            if(invoiceId < 0)
            {
                throw new ArgumentException("InvoiceId must be greater than zero");
            }

            var query = "SELECT * FROM Invoices WHERE InvoiceID = @InvoiceID;";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InvoiceID", invoiceId);
                        using(var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Invoice
                                {
                                    InvoiceID = reader.GetInt32(reader.GetOrdinal("InvoiceID")),
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount"))
                                };                                
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Invoice search error: {ex.Message}");
                throw;
            }
            return null;
        }
        public bool UpdateInvoice(Invoice invoice) 
        {
            if (invoice.InvoiceID < 0)
            {
                throw new ArgumentException("InvoiceID must be greater than zero");
            }

            var updateQuery = "UPDATE Invoices SET OrderID = @OrderID, InvoiceDate = @InvoiceDate, Amount = @Amount WHERE InvoiceID = @InvoiceID";

            try
            {
                using( var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using( var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", invoice.OrderID);
                        command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                        command.Parameters.AddWithValue("@Amount", invoice.Amount);
                        command.Parameters.AddWithValue("@InvoiceID", invoice.InvoiceID);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating the invoice: {ex.Message}");
                throw;
            }
        }
        public bool DeleteInvoice(int invoiceId) 
        {
            if (invoiceId < 0)
            {
                throw new ArgumentException("InvoiceId must be greater than zero");
            }

            var deleteQuery = "DELETE FROM Invoices WHERE InvoiceID = @InvoiceID";

            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@InvoiceID", invoiceId);
                        int rowsAffectd = command.ExecuteNonQuery();
                        return rowsAffectd > 0;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error deleting the invoice: {ex.Message}");
                throw;
            }
        }
    }
}