﻿using Assignment2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    public class CustomerRepository : ICustomerRepository
    {
        public SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();
        public Customer Customer;

        /// <summary>
        /// Initialize SQL Connection Builder with default values
        /// </summary>
        public CustomerRepository()
        {
            Builder.DataSource = @"5CG05206QV\SQLEXPRESS"; // @"5CG05206QS\SQLEXPRESS";
            Builder.InitialCatalog = "Chinook";
            Builder.IntegratedSecurity = true;
        }
        /// <summary>
        /// Initialize SQL Connection Builder with given parameter values
        /// </summary>
        /// <param name="dataSource">Builder.DataSource path to database without @ sign</param>
        /// <param name="initialCatalog">Name of database</param>
        public CustomerRepository(string dataSource, string initialCatalog)
        {
            Builder.DataSource = "@" + dataSource;
            Builder.InitialCatalog = initialCatalog;
            Builder.IntegratedSecurity = true;
        }

        public Customer AddCustomer(Customer addCustomer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Customer (FirstName, LastName, Country, PostalCode, Phone, Email) VALUES (@FirstName, @LastName, @Country, @PostalCode, @Phone, @Email)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", addCustomer.FirstName);
                        command.Parameters.AddWithValue("@LastName", addCustomer.LastName);
                        command.Parameters.AddWithValue("@Country", addCustomer.Country);
                        command.Parameters.AddWithValue("@Phone", addCustomer.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", addCustomer.Email);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

            return addCustomer;
        }

        public Customer GetCustomerById(int id)
        {
            Customer customerFromDB = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email " +
                        $"FROM Customer WHERE CustomerId = {id}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetString(5),
                                    reader.GetString(6)
                                    );
                                }
                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.ToString());
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return customerFromDB;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customerToReturn = new List<Customer>();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email FROM Customer";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                //reader.IsDBNull check usage to prevent first null object??
                                while (reader.Read())
                                {
                                    Customer customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetString(5),
                                    reader.GetString(6)
                                    );

                                    customerToReturn.Add(customerFromDB);
                                }
                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.ToString());
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return customerToReturn;
        }

        public Customer GetCustomerByName(string CustomerName)
        {
            Customer customerFromDB = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email FROM Customer WHERE Customer.FirstName LIKE '%{CustomerName}%' OR Customer.LastName LIKE '%{CustomerName}%'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetString(5),
                                    reader.GetString(6)
                                    );
                                }
                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.ToString());
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return customerFromDB;

        }

        public List<Customer> GetCustomersPage(int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public Customer UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public CustomerCountry GetNumberOfCustomersByCountry(string country)
        {
            throw new NotImplementedException();
        }

        public CustomerSpender GetHighestSpendingCustomers()
        {
            throw new NotImplementedException();
        }

        public CustomerGenre GetMostPopularGenre(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
