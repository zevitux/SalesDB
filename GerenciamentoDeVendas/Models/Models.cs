using System;
using System.Collections.Generic;

namespace GerenciamentoDeVendas.Models
{
    public class Customer
    {
        public int CustomerID { get; set; } //pk
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class Product
    {
        public int ProductID { get; set; } //pk
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; } //pk
        public int CustomerID { get; set; } //fk Costumer
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; } //fk Order
        public int ProductID { get; set; } //fk Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

    public class Invoice //Fatura
    {
        public int InvoiceID { get; set; } //pk
        public int OrderID { get; set; } //fk unica Order
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public Order Order { get; set; }
    } 
}