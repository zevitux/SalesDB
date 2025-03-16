using GerenciamentoDeVendas.Functions;
using GerenciamentoDeVendas.Models;
using System;
using System.Collections.Generic;

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
                Console.WriteLine("3. Exit");
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
                        return; // sair do programa
                    default:
                        Console.WriteLine("Invalid option! Try again");
                        break;
                }

                Console.WriteLine("Press any key to back to menu");
                Console.ReadKey();
            }


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

            Console.Write("Name: ");
            newCustomer.Name = Console.ReadLine();

            Console.Write("Email: ");
            newCustomer.Email = Console.ReadLine();

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
            if(int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = customerDAO.GetCustomerById(customerId);
                if(customer != null)
                {
                    Console.Write("New Name (leave blank so as not to change): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        customer.Name = newName;
                    }
                    Console.Write("New Email (leave blank so as not to change): ");
                    string newEmail = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newEmail))
                    {
                        customer.Email = newEmail;
                    }

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
            if(int.TryParse(Console.ReadLine(), out int customerId))
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
                    Console.WriteLine($"ID: {product.ProductID} - Name: {product.Name} - Description: {product.Description} - Price: {product.Price:C} - Stock: {product.Stock}");
                }
            }
        }
        static void AddProduct(ProductDAO productDAO)
        {
            var newProduct = new Product();

            Console.Write("Name: ");
            newProduct.Name = Console.ReadLine();

            Console.Write("Description: ");
            newProduct.Description = Console.ReadLine();

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
                    Console.Write("New Name (leave blank to not change): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        product.Name = newName;
                    }

                    Console.Write("New Description (leave blank to not change): ");
                    string newDescription = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newDescription))
                    {
                        product.Description = newDescription;
                    }

                    Console.Write("New Price (leave blank to not change): ");
                    string newPriceInput = Console.ReadLine();
                    if (decimal.TryParse(newPriceInput, out decimal newPrice))
                    {
                        product.Price = newPrice;
                    }

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
    }
}
