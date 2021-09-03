using Assignment2.Exceptions;
using Assignment2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating the Repository with specific server name of the database
            ICustomerRepository customerRepo = new CustomerRepository(@"5CG05206QV\SQLEXPRESS", "Chinook");

            int method = 9;

            // display all customers
            if (method == 1)
            {
                foreach (Customer customer in customerRepo.GetCustomers())
                {
                    Console.WriteLine(customer);
                }
            }
            // display specific customer by ID
            else if (method == 2)
            {
                Console.WriteLine(customerRepo.GetCustomerById(44));
            }
            //read specific customer by name. accepts partial matches
            else if (method == 3)
            {
                Console.WriteLine(customerRepo.GetCustomerByName("Hannah"));
            }
            // display a limited amount of customers starting from point of choice
            else if (method == 4)
            {
                foreach (Customer customer in customerRepo.GetCustomersPage(5, 20))
                {
                    Console.WriteLine(customer);
                }
            }
            // add new customer
            else if (method == 5)
            {
                Customer bruce = new Customer()
                {
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    Country = "USA",
                    PostalCode = "00100",
                    PhoneNumber = "050 123 4567",
                    Email = "batman@wayneenterprise.com"
                };
                customerRepo.AddCustomer(bruce);
            }
            // update data of a customer of choice
            else if (method == 6)
            {
                Customer bruce = new Customer()
                {
                    Id = 60,
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    Country = "USA",
                    PostalCode = "00110",
                    PhoneNumber = "050 123 4567",
                    Email = "batman@wayneenterprise.com"
            };
                customerRepo.UpdateCustomer(bruce);

            }
            // display top countries by customer count
            else if (method == 7)
            {
                foreach(CustomerCountry country in customerRepo.GetNumberOfCustomersByCountry())
                {
                    Console.WriteLine(country);
                }
            }
            // displays top spenders among all customers
            else if (method == 8)
            {
                Console.WriteLine(customerRepo.GetHighestSpendingCustomers());
            }
            // displays the top genre choice of a specific customer by ID
            else if (method == 9)
            {
                Console.WriteLine(customerRepo.GetMostPopularGenreByCustomerId(15));
            }

        }
    }
}
