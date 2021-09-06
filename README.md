# Database access tool

.NET Core Console Application used to interface with the "Chinook" SQLExpress database.

Second weekly assignment of Experis Academy. Done with Mikko Nikku and Sebastian Söderholm.

To use this, change customerRepo into your own data source. The console application can be used to choose different operations that call the correct methods:
- display all customers
- display specific customer by ID
- read specific customer by name. accepts partial matches.
- display a limited amount of customers starting from point of choice
- add new customer
- update data of a customer of choice.
- display top countries by customer count
- displays top spenders among all customers
- displays the top genre choice of a specific customer by ID

The are multiple methods you can call:
- display all customers GetCustomers() returns a list of Customer objects
- display specific customer by ID GetCustomerById(int id) returns Customer object
- read specific customer by name. accepts partial matches. GetCustomerByName(string CustomerName) returns Customer object
- display a limited amount of customers starting from point of choice GetCustomersPage(int limit, int offset) returns a list of Customer objects
- add new customer. AddCustomer(Customer addCustomer) returns Customer object
- update data of a customer of choice. UpdateCustomer(Customer customer)
- display top countries by customer count GetNumberOfCustomersByCountry() returns a list of CustomerCountry objects
- displays top spenders among all customers GetHighestSpendingCustomers() returns a CustomerSpender object
- displays the top genre choice of a specific customer by ID GetMostPopularGenreByCustomerId(int customerId) returns CustomerGenre object
