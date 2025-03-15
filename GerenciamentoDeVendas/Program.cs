using GerenciamentoDeVendas.Functions;
using GerenciamentoDeVendas.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GerenciamentoDeVendas
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Database=SalesDB;Integrated Security=True;";

            //instanciando
            var customerDAO = new CustomerDAO(connectionString);
            var productDAO = new ProductDAO(connectionString);
            var orderDAO = new OrderDAO(connectionString);
            var orderItemDAO = new OrderItemDAO(connectionString);
            var invoiceDAO = new InvoiceDAO(connectionString);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Manage Customers");
                Console.WriteLine("2. Manage Products");
                Console.WriteLine("3. Manage Orders");
                Console.WriteLine("4. Manage Order Items");
                Console.WriteLine("5. Manage Invoice");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageCustomers(customerDAO);
                        break;
                    case "2":
                        ManageProducts(productDAO);
                        break;
                    case "3":
                        ManageOrders(orderDAO, customerDAO);
                        break;
                    case "4":
                        ManageOrderItems(orderItemDAO, orderDAO);
                        break;
                    case "5":
                        ManageInvoices(invoiceDAO);
                        break;
                    case "6":
                        return;
                }

            }   

            Console.WriteLine("Press any key to back to menu");
            Console.ReadKey();
            
        }
        static void ManageCustomers(CustomerDAO customerDAO)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1.List customers\n2.Add customer\n3.Att customers\n4.Delete customer\n5.Back to the main menu");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListCustomers(customerDAO);
                        break;
                    case "2":
                        AddCustomer(customerDAO);
                        break;
                    case "3":
                        UpdateCustomer(customerDAO);
                        break;
                    case "4":
                        DeleteCustomer(customerDAO);
                        break;
                    case "5":
                        return; // voltar ao menu principal
                    default:
                        Console.WriteLine("Invalid option! Try again");
                        break;
                }

                Console.WriteLine("Press any key to back to the options...");
                Console.ReadKey();
            }
        }
        static void ManageProducts(ProductDAO productDAO)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1.List products\n2.Add product\n3.Att product\n4.Delete product\n5.Back to the main menu");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListProducts(productDAO);
                        break;
                    case "2":
                        AddProduct(productDAO);
                        break;
                    case "3":
                        UpdateProduct(productDAO);
                        break;
                    case "4":
                        DeleteProduct(productDAO);
                        break;
                    case "5":
                        return; // voltar ao menu principal
                    default:
                        Console.WriteLine("Invalid option! Try again");
                        break;
                }
                Console.WriteLine("Press any key to back to the options...");
                Console.ReadKey();
            }
        }
        static void ManageOrders(OrderDAO orderDAO, CustomerDAO customerDAO)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. List Orders");
                Console.WriteLine("2. Add Order");
                Console.WriteLine("3. Update Order");
                Console.WriteLine("4. Delete Order");
                Console.WriteLine("5. Back to the main menu");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListOrders(orderDAO);
                        break;
                    case "2":
                        AddOrder(orderDAO, customerDAO);
                        break;
                    case "3":
                        UpdateOrder(orderDAO, customerDAO);
                        break;
                    case "4":
                        DeleteOrder(orderDAO);
                        break;
                    case "5":
                        return; // voltar ao menu principal
                    default:
                        Console.WriteLine("Invalid option! Try again");
                        break;
                }

                Console.WriteLine("Press any key to back to the options...");
                Console.ReadKey();
            }
        }
        static void ManageOrderItems(OrderItemDAO orderItemDAO, OrderDAO orderDAO)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. List Order Items\n2. Add Order Item\n3. Update Order Item\n4. Delete Order Item\n5. Back to the main menu");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListOrderItems(orderItemDAO);
                        break;
                    case "2":
                        AddOrderItem(orderItemDAO, orderDAO);
                        break;
                    case "3":
                        UpdateOrderItem(orderItemDAO);
                        break;
                    case "4":
                        DeleteOrderItem(orderItemDAO);
                        break;
                    case "5":
                        return; // Voltar ao menu principal
                    default:
                        Console.WriteLine("Invalid option! Try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        static void ManageInvoices(InvoiceDAO invoiceDAO)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. List Invoices\n2. Add Invoice\n3. Update Invoice\n4. Delete Invoice\n5. Back to the main menu");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ListInvoices(invoiceDAO);
                        break;
                    case "2":
                        AddInvoice(invoiceDAO);
                        break;
                    case "3":
                        UpdateInvoice(invoiceDAO);
                        break;
                    case "4":
                        DeleteInvoice(invoiceDAO);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option! Try again.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void ListCustomers(CustomerDAO customerDAO)
        {
            List<Customer> customers = customerDAO.GetAllCustomers();
            Console.WriteLine("Customers list:");
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found");
            }
            else
            {
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerID} - Name: {customer.Name} - Email: {customer.Email} - Phone: {customer.Phone}");
                }
            }
        }
        static void AddCustomer(CustomerDAO customerDAO)
        {
            var newCustomer = new Customer();

            // adicionando nome do cliente
            Console.Write("Name: ");
            newCustomer.Name = Console.ReadLine();

            // adicionando email do cliente 
            Console.Write("Email: ");
            newCustomer.Email = Console.ReadLine();

            // adicionando telefone do cliente
            Console.Write("Phone: ");
            newCustomer.Phone = Console.ReadLine();

            try
            {
                int newCustomerId = customerDAO.InsertCustomer(newCustomer);
                Console.WriteLine($"New customers inserting with ID: {newCustomerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when adding customer: {ex.Message}");
            }
        }
        static void UpdateCustomer(CustomerDAO customerDAO)
        {
            Console.Write("Type ID of the customer do you want update: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = customerDAO.GetCustomerById(customerId);
                if (customer != null)
                {

                    // mudannca de nome
                    Console.Write("New Name (leave blank so as not to change): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        customer.Name = newName;
                    }

                    // mudanca de email
                    Console.Write("New Email (leave blank so as not to change): ");
                    string newEmail = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newEmail))
                    {
                        customer.Email = newEmail;
                    }

                    // mudanca de telefone
                    Console.Write("New Phone (leave blank so as not to change): ");
                    string newPhone = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newPhone))
                    {
                        customer.Phone = newPhone;
                    }

                    try
                    {
                        if (customerDAO.UpdateCustomer(customer))
                        {
                            Console.WriteLine("Customer updated with sucess");
                        }
                        else
                        {
                            Console.WriteLine("Customer update falied");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Customer not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }
        static void DeleteCustomer(CustomerDAO customerDAO)
        {
            Console.Write("Enter the ID of the client you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                try
                {
                    if (customerDAO.DeleteCustomer(customerId))
                    {
                        Console.WriteLine("Customer successfully deleted");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the customer! The client may not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when deleting customer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID!");
            }
        }

        static void ListProducts(ProductDAO productDAO)
        {
            List<Product> products = productDAO.GetAllProducts();
            Console.WriteLine("Products list:");
            if (products.Count == 0)
            {
                Console.WriteLine("No products found");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductID} - Name: {product.Name} - Description: {product.Description} - Price: {product.Price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"))} - Stock: {product.Stock}");
                }
            }
        }
        static void AddProduct(ProductDAO productDAO)
        {
            var newProduct = new Product();

            // adicionando nome do produto
            Console.Write("Name: ");
            newProduct.Name = Console.ReadLine();

            // adicionando descrição do produto 
            Console.Write("Description: ");
            newProduct.Description = Console.ReadLine();

            // adicionando preço do produto 
            Console.Write("Price: $");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                newProduct.Price = price;
            }
            else
            {
                Console.WriteLine("Invalid price! Please enter a valid decimal number");
                return;
            }

            // adicionando quantidade em estoque do produto
            Console.Write("Stock: ");
            if (int.TryParse(Console.ReadLine(), out int stock))
            {
                newProduct.Stock = stock;
            }
            else
            {
                Console.WriteLine("Invalid stock quantity! Please enter a valid integer number");
                return;
            }

            try
            {
                int newProductId = productDAO.InsertProduct(newProduct);
                Console.WriteLine($"New product inserted with ID: {newProductId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when adding product: {ex.Message}");
            }
        }
        static void UpdateProduct(ProductDAO productDAO)
        {
            Console.Write("Type ID of the product you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var product = productDAO.GetProductById(productId);
                if (product != null)
                {

                    // mudanca de nome  
                    Console.Write("New Name (leave blank to not change): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        product.Name = newName;
                    }

                    // mudanca de descricao 
                    Console.Write("New Description (leave blank to not change): ");
                    string newDescription = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newDescription))
                    {
                        product.Description = newDescription;
                    }

                    // mudanca de preco 
                    Console.Write("New Price (leave blank to not change): ");
                    string newPriceInput = Console.ReadLine();
                    if (decimal.TryParse(newPriceInput, out decimal newPrice))
                    {
                        product.Price = newPrice;
                    }

                    // mudanca de estoque
                    Console.Write("New Stock (leave blank to not change): ");
                    string newStockInput = Console.ReadLine();
                    if (int.TryParse(newStockInput, out int newStock))
                    {
                        product.Stock = newStock;
                    }

                    try
                    {
                        if (productDAO.UpdateProduct(product))
                        {
                            Console.WriteLine("Product updated successfully");
                        }
                        else
                        {
                            Console.WriteLine("Product update failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }
        static void DeleteProduct(ProductDAO productDAO)
        {
            Console.Write("Enter the ID of the product you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                try
                {
                    if (productDAO.DeleteProduct(productId))
                    {
                        Console.WriteLine("Product successfully deleted");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the product! The product may not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when deleting product: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID!");
            }
        }

        static void ListOrders(OrderDAO orderDAO)
        {
            List<Order> orders = orderDAO.GetAllOrders();
            Console.WriteLine("Orders list:");
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found");
            }
            else
            {
                foreach (var order in orders)
                {
                    Console.WriteLine($"OrderID: {order.OrderID} - CustomerID: {order.CustomerID} - OrderDate: {order.OrderDate} - TotalAmount: {order.TotalAmount:C}");
                }
            }
        }
        static void AddOrder(OrderDAO orderDAO, CustomerDAO customerDAO)
        {
            var newOrder = new Order();

            // obter o ID do cliente
            Console.Write("Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                newOrder.CustomerID = customerId;
            }
            else
            {
                Console.WriteLine("Invalid Customer ID!");
                return;
            }

            // obter a data do pedido
            Console.Write("Order Date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime orderDate))
            {
                newOrder.OrderDate = orderDate;
            }
            else
            {
                Console.WriteLine("Invalid date format!");
                return;
            }

            // obter o valor total do pedido
            Console.Write("Total Amount: $");
            if (decimal.TryParse(Console.ReadLine(), out decimal totalAmount))
            {
                newOrder.TotalAmount = totalAmount;
            }
            else
            {
                Console.WriteLine("Invalid amount!");
                return;
            }

            // Adicionar um item ao pedido
            var orderItem = new OrderItem();

            Console.Write("Product ID: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                orderItem.ProductID = productId;
            }
            else
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            Console.Write("Quantity: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
            {
                orderItem.Quantity = quantity;
            }
            else
            {
                Console.WriteLine("Invalid quantity!");
                return;
            }

            Console.Write("Unit Price: $");
            if (decimal.TryParse(Console.ReadLine(), out decimal unitPrice))
            {
                orderItem.UnitPrice = unitPrice;
            }
            else
            {
                Console.WriteLine("Invalid unit price!");
                return;
            }

            // adiciona o item ao pedido
            newOrder.OrderItems = new List<OrderItem> { orderItem };

            try
            {
                int newOrderId = orderDAO.InsertOrder(newOrder);
                Console.WriteLine($"New order inserted with ID: {newOrderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when adding order: {ex.Message}");
            }
        }
        static void UpdateOrder(OrderDAO orderDAO, CustomerDAO customerDAO)
        {
            Console.Write("Enter the ID of the order you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                var order = orderDAO.GetOrderById(orderId);
                if (order != null)
                {
                    // obter o novo ID do cliente
                    Console.Write("New Customer ID (leave blank to not change): ");
                    string newCustomerIdInput = Console.ReadLine();
                    if (int.TryParse(newCustomerIdInput, out int newCustomerId))
                    {
                        order.CustomerID = newCustomerId;
                    }

                    // obter a nova data do pedido
                    Console.Write("New Order Date (leave blank to not change): ");
                    string newOrderDateInput = Console.ReadLine();
                    if (DateTime.TryParse(newOrderDateInput, out DateTime newOrderDate))
                    {
                        order.OrderDate = newOrderDate;
                    }

                    // obter o novo valor total do pedido
                    Console.Write("New Total Amount (leave blank to not change): ");
                    string newTotalAmountInput = Console.ReadLine();
                    if (decimal.TryParse(newTotalAmountInput, out decimal newTotalAmount))
                    {
                        order.TotalAmount = newTotalAmount;
                    }

                    try
                    {
                        if (orderDAO.UpdateOrder(order))
                        {
                            Console.WriteLine("Order updated successfully");
                        }
                        else
                        {
                            Console.WriteLine("Order update failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Order not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }
        static void DeleteOrder(OrderDAO orderDAO)
        {
            Console.Write("Enter the ID of the order you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                try
                {
                    if (orderDAO.DeleteOrder(orderId))
                    {
                        Console.WriteLine("Order successfully deleted");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the order! The order may not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when deleting order: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID!");
            }

        }

        static void ListOrderItems(OrderItemDAO orderItemDAO)
        {
            List<OrderItem> orderItems = orderItemDAO.GetAllOrderItems();
            Console.WriteLine("Order Items list:");
            if (orderItems.Count == 0)
            {
                Console.WriteLine("No order items found");
            }
            else
            {
                foreach (var orderItem in orderItems)
                {
                    Console.WriteLine($"OrderItemID: {orderItem.OrderItemID} - OrderID: {orderItem.OrderID} - ProductID: {orderItem.ProductID} - Quantity: {orderItem.Quantity} - UnitPrice: {orderItem.UnitPrice.ToString("C", CultureInfo.CreateSpecificCulture("en-US"))}");
                }
            }
        }
        static void AddOrderItem(OrderItemDAO orderItemDAO, OrderDAO orderDAO)
        {
            var newOrderItem = new OrderItem();

            // adicionar OrderID
            Console.Write("Order ID: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                newOrderItem.OrderID = orderId;
            }
            else
            {
                Console.WriteLine("Invalid Order ID!");
                return;
            }

            // adicionar ProductID  
            Console.Write("Product ID: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                newOrderItem.ProductID = productId;
            }
            else
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            // adicionar Quantity   
            Console.Write("Quantity: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
            {
                newOrderItem.Quantity = quantity;
            }
            else
            {
                Console.WriteLine("Invalid quantity!");
                return;
            }

            // adicionar UnitPrice  
            Console.Write("Unit Price: $");
            if (decimal.TryParse(Console.ReadLine(), out decimal unitPrice))
            {
                newOrderItem.UnitPrice = unitPrice;
            }
            else
            {
                Console.WriteLine("Invalid unit price!");
                return;
            }

            try
            {
                int newOrderItemId = orderItemDAO.InsertOrderItem(newOrderItem);
                Console.WriteLine($"New order item inserted with ID: {newOrderItemId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when adding order item: {ex.Message}");
            }
        }
        static void UpdateOrderItem(OrderItemDAO orderItemDAO)
        {
            Console.Write("Enter the ID of the order item you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int orderItemId))
            {
                var orderItem = orderItemDAO.GetOrderItemById(orderItemId);
                if (orderItem != null)
                {
                    // atualizar OrderID
                    Console.Write("New Order ID (leave blank to not change): ");
                    string newOrderIdInput = Console.ReadLine();
                    if (int.TryParse(newOrderIdInput, out int newOrderId))
                    {
                        orderItem.OrderID = newOrderId;
                    }

                    // atualizar ProductID
                    Console.Write("New Product ID (leave blank to not change): ");
                    string newProductIdInput = Console.ReadLine();
                    if (int.TryParse(newProductIdInput, out int newProductId))
                    {
                        orderItem.ProductID = newProductId;
                    }

                    // atualizar Quantity
                    Console.Write("New Quantity (leave blank to not change): ");
                    string newQuantityInput = Console.ReadLine();
                    if (int.TryParse(newQuantityInput, out int newQuantity))
                    {
                        orderItem.Quantity = newQuantity;
                    }

                    // atualizar UnitPrice
                    Console.Write("New Unit Price (leave blank to not change): ");
                    string newUnitPriceInput = Console.ReadLine();
                    if (decimal.TryParse(newUnitPriceInput, out decimal newUnitPrice))
                    {
                        orderItem.UnitPrice = newUnitPrice;
                    }

                    try
                    {
                        if (orderItemDAO.UpdateOrderItem(orderItem))
                        {
                            Console.WriteLine("Order item updated successfully");
                        }
                        else
                        {
                            Console.WriteLine("Order item update failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Order item not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }
        static void DeleteOrderItem(OrderItemDAO orderItemDAO)
        {
            Console.Write("Enter the ID of the order item you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int orderItemId))
            {
                try
                {
                    if (orderItemDAO.DeleteOrderItem(orderItemId))
                    {
                        Console.WriteLine("Order item successfully deleted");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the order item! The order item may not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when deleting order item: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID!");
            }
        }

        static void ListInvoices(InvoiceDAO invoiceDAO)
        {
            List<Invoice> invoices = invoiceDAO.GetAllInvoices();
            Console.WriteLine("Invoices list:");
            if (invoices.Count == 0)
            {
                Console.WriteLine("No invoices found");
            }
            else
            {
                foreach (var invoice in invoices)
                {
                    Console.WriteLine($"InvoiceID: {invoice.InvoiceID} - OrderID: {invoice.OrderID} - InvoiceDate: {invoice.InvoiceDate} - Amount: {invoice.Amount.ToString("C", CultureInfo.CreateSpecificCulture("en-US"))}");
                }
            }
        }
        static void AddInvoice(InvoiceDAO invoiceDAO)
        {
            var newInvoice = new Invoice();

            // adicionar OrderID
            Console.Write("Order ID: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                newInvoice.OrderID = orderId;
            }
            else
            {
                Console.WriteLine("Invalid Order ID!");
                return;
            }

            // adicionar InvoiceDate    
            Console.Write("Invoice Date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime invoiceDate))
            {
                newInvoice.InvoiceDate = invoiceDate;
            }
            else
            {
                Console.WriteLine("Invalid date format!");
                return;
            }

            // adicionar Amount 
            Console.Write("Amount: $");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                newInvoice.Amount = amount;
            }
            else
            {
                Console.WriteLine("Invalid amount!");
                return;
            }

            try
            {
                int newInvoiceId = invoiceDAO.InsertInvoice(newInvoice);
                Console.WriteLine($"New invoice inserted with ID: {newInvoiceId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when adding invoice: {ex.Message}");
            }
        }
        static void UpdateInvoice(InvoiceDAO invoiceDAO)
        {
            Console.Write("Enter the ID of the invoice you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = invoiceDAO.GetInvoiceById(invoiceId);
                if (invoice != null)
                {
                    // atualizar OrderID
                    Console.Write("New Order ID (leave blank to not change): ");
                    string newOrderIdInput = Console.ReadLine();
                    if (int.TryParse(newOrderIdInput, out int newOrderId))
                    {
                        invoice.OrderID = newOrderId;
                    }
                    // atualizar InvoiceDate
                    Console.Write("New Invoice Date (leave blank to not change): ");
                    string newInvoiceDateInput = Console.ReadLine();
                    if (DateTime.TryParse(newInvoiceDateInput, out DateTime newInvoiceDate))
                    {
                        invoice.InvoiceDate = newInvoiceDate;
                    }
                    // atualizar Amount
                    Console.Write("New Amount (leave blank to not change): ");
                    string newAmountInput = Console.ReadLine();
                    if (decimal.TryParse(newAmountInput, out decimal newAmount))
                    {
                        invoice.Amount = newAmount;
                    }
                    try
                    {
                        if (invoiceDAO.UpdateInvoice(invoice))
                        {
                            Console.WriteLine("Invoice updated successfully");
                        }
                        else
                        {
                            Console.WriteLine("Invoice update failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invoice not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }
        static void DeleteInvoice(InvoiceDAO invoiceDAO)
        {
            Console.Write("Enter the ID of the invoice you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                try
                {
                    if (invoiceDAO.DeleteInvoice(invoiceId))
                    {
                        Console.WriteLine("Invoice successfully deleted");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the invoice! The invoice may not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when deleting invoice: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID!");
            }
        }
    }
}