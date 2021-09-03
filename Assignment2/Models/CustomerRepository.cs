using Assignment2.Exceptions;
using Assignment2.Models;
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
        public RepositoryException repositoryException;

        /// <summary>
        /// Initialize SQL Connection Builder with default values
        /// </summary>
        public CustomerRepository()
        {
            Builder.DataSource = @"5CG05206R1\SQLEXPRESS";
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
            Builder.DataSource = dataSource;
            Builder.InitialCatalog = initialCatalog;
            Builder.IntegratedSecurity = true;
        }

        public void AddCustomer(Customer addCustomer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Customer (FirstName, LastName, Country, PostalCode, Phone, Email)" + 
                        "VALUES (@FirstName, @LastName, @Country, @PostalCode, @Phone, @Email)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adding Customer object's values to query's placehodlers, ID will be auto generated
                        command.Parameters.AddWithValue("@FirstName", addCustomer.FirstName);
                        command.Parameters.AddWithValue("@LastName", addCustomer.LastName);
                        command.Parameters.AddWithValue("@Country", addCustomer.Country);
                        command.Parameters.AddWithValue("@PostalCode", addCustomer.PostalCode);
                        command.Parameters.AddWithValue("@Phone", addCustomer.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", addCustomer.Email);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
        }

        public Customer GetCustomerById(int id)
        {
            // Creating an empty Customer object, for values fetched from the database
            Customer customerFromDB = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email " +
                        $"FROM Customer WHERE CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametrize values to avoid SQL Injections
                        command.Parameters.AddWithValue("@CustomerId", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Creating and initializing a empty strings for values that can be null
                                string country = "";
                                string postalCode = "";
                                string phone = "";
                                string email = "";

                                // If the country is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Country"))) country = reader.GetString(3);
                                // If the postalcode is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("PostalCode"))) postalCode = reader.GetString(4);
                                // If the phone is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Phone"))) phone = reader.GetString(5 );
                                // If the email is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Email"))) email = reader.GetString(6);

                                // Adding the values to the ready made Customer object
                                customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    country,
                                    postalCode,
                                    phone,
                                    email
                                );
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
            return customerFromDB;
        }

        public List<Customer> GetCustomers()
        {
            // Creating empty list for fetched customers, that will be returned
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
                            while (reader.Read())
                            {
                                // Creating and initializing a empty strings for values that can be null
                                string country = "";
                                string postalCode = "";
                                string phone = "";
                                string email = "";

                                // If the country is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Country"))) country = reader.GetString(3);
                                // If the postalcode is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("PostalCode"))) postalCode = reader.GetString(4);
                                // If the phone is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Phone"))) phone = reader.GetString(5);
                                // If the email is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Email"))) email = reader.GetString(6);

                                // Adding the values to the ready made Customer object
                                Customer customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    country,
                                    postalCode,
                                    phone,
                                    email
                                );
                                // Adding the new Customer object to the list
                                customerToReturn.Add(customerFromDB);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
            return customerToReturn;
        }

        public Customer GetCustomerByName(string CustomerName)
        {
            // Creating an empty Customer object, for values fetched from the database
            Customer customerFromDB = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email FROM Customer WHERE Customer.FirstName LIKE '%' + @CustomerName + '%' OR Customer.LastName LIKE '%' + @CustomerName + '%'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametrize values to avoid SQL Injections
                        command.Parameters.AddWithValue("@CustomerName", CustomerName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Creating and initializing a empty strings for values that can be null
                                string country = "";
                                string postalCode = "";
                                string phone = "";
                                string email = "";

                                // If the country is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Country"))) country = reader.GetString(3);
                                // If the postalcode is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("PostalCode"))) postalCode = reader.GetString(4);
                                // If the phone is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Phone"))) phone = reader.GetString(5);
                                // If the email is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Email"))) email = reader.GetString(6);

                                // Adding the values to the ready made Customer object
                                customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    country,
                                    postalCode,
                                    phone,
                                    email
                                );
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
            return customerFromDB;
        }

        public List<Customer> GetCustomersPage(int limit, int offset)
        {
            // Creating empty list for fetched customers, that will be returned
            List<Customer> customerToReturn = new List<Customer>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email FROM Customer ORDER BY CustomerId OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", limit);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Creating and initializing a empty strings for values that can be null
                                string country = "";
                                string postalCode = "";
                                string phone = "";
                                string email = "";

                                // If the country is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Country"))) country = reader.GetString(3);
                                // If the postalcode is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("PostalCode"))) postalCode = reader.GetString(4);
                                // If the phone is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Phone"))) phone = reader.GetString(5);
                                // If the email is not null, adding the the value from database to the variable
                                if (!reader.IsDBNull(reader.GetOrdinal("Email"))) email = reader.GetString(6);

                                // Adding the values to the ready made Customer object
                                Customer customerFromDB = new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    country,
                                    postalCode,
                                    phone,
                                    email
                                );
                                // Adding the new Customer object to the list
                                customerToReturn.Add(customerFromDB);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
            return customerToReturn;
        }

        public void UpdateCustomer(Customer customer)
        {           
            string query = "UPDATE Customer SET FirstName=@firstName, LastName=@lastName, Country=@country, PostalCode=@postalCode, Phone=@phone, Email=@email  WHERE CustomerId = @customerId;";
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    // Adding Customer object's values to query's placehodlers
                    cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@country", customer.Country);
                    cmd.Parameters.AddWithValue("@postalCode", customer.PostalCode);
                    cmd.Parameters.AddWithValue("@phone", customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@email", customer.Email);
                    cmd.Parameters.AddWithValue("@customerId", customer.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
        }

        public List<CustomerCountry> GetNumberOfCustomersByCountry()
        {
            // Creating empty list for fetched information about amount of the customer per country
            List<CustomerCountry> customerNumbers = new List<CustomerCountry>();
            string query = "SELECT Country, COUNT(CustomerId) AS total FROM Customer GROUP BY Country ORDER BY total DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Creating a new CustomerCountry object that contais the name of the country and the amount of customers
                            CustomerCountry customerCountryNumber = new CustomerCountry(
                                reader.GetString(0),
                                reader.GetInt32(1)
                            );
                            // Addng the CustomerCountry object to the list
                            customerNumbers.Add(customerCountryNumber);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
            return customerNumbers;
        }

        public CustomerSpender GetHighestSpendingCustomers()
        {
            // Creating a new empty CustomerSpeder object
            CustomerSpender customerSpender = new CustomerSpender();

            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT Customer.CustomerId, SUM(Invoice.Total) AS total
                        FROM Customer, Invoice
                        WHERE Customer.CustomerId = Invoice.CustomerId
                        GROUP BY Customer.CustomerId
                        ORDER BY total DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    // Adding CustomerID and the total invoice amount to the CustomerSpender dictionary
                                    customerSpender.AddCustomerSpendings(reader.GetInt32(0), reader.GetDecimal(1));
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
            return customerSpender;
        }

        public CustomerGenre GetMostPopularGenreByCustomerId(int customerId)
        {
            // Creating a new empty CustomerGenre object
            CustomerGenre customerGenres = new CustomerGenre();
            try
            {
                using (SqlConnection connection = new SqlConnection(Builder.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT Genre.Name, COUNT(Genre.Name) AS amount " +
                                    "FROM Customer, Invoice, InvoiceLine, Track, Genre " +
                                    "WHERE Customer.CustomerId = Invoice.CustomerId " +
                                    "AND Invoice.CustomerId = InvoiceLine.InvoiceId " +
                                    "AND InvoiceLine.TrackId = Track.TrackId " +
                                    "AND Track.GenreId = Genre.GenreId " +
                                    $"AND Customer.CustomerId = @customerId " +
                                    "GROUP BY Genre.Name " +
                                    "ORDER BY amount DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adding Customer object's values to query's placehodlers, ID will be auto generated
                        command.Parameters.AddWithValue("@customerId", customerId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Creating a new list for all genres of the customer
                            List<int> customerGenreAmounts = new List<int>();
                            while (reader.Read())
                            {
                                // Adding the current row to the list
                                customerGenreAmounts.Add(reader.GetInt32(1));
                                if (customerGenreAmounts.Max() == reader.GetInt32(1))
                                {
                                    // If the current row's ammount is as big as the maximum amount of the genre, adding the row to the CustomerGenre dictionary
                                    customerGenres.AddCustomerGenreAmount(reader.GetString(0), reader.GetInt32(1));
                                }    
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepositoryException(ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex);
            }
            return customerGenres;
        }
    }
}