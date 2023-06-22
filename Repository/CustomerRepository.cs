using CloudinaryDotNet.Actions;
using Microsoft.Data.SqlClient;
using MotoShop.Data;
using MotoShop.Constants;
using MotoShop.Models;
using MotoShop.ViewModels.CustomerViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MotoShop.Repository
{
    public class CustomerRepository : Repository
    {
        public CustomerRepository(DbContext context) : base(context) { }

        public Customer? GetByLogin(string login)
        {
            Customer? customer = null;
            string query = "SELECT FirstName, LastName, Email, Phone  FROM Customer WHERE Login = @Login";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Login", login);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    customer = new Customer()
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                    };
                }
                connection.Close();
            }

            return customer;
        }

        public bool IsLoginExist(string login)
        {
            bool state = false;
            string query = "SELECT Login FROM Customer WHERE Login = @Login";
            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Login", login);
                SqlDataReader reader = cmd.ExecuteReader();

                state = reader.Read() && reader["Login"].ToString() == login;
                connection.Close();
            }

            return state;
        }

        public Customer? Authenticate(string login, string password)
        {
            Customer? customer = null;
            string query = "SELECT * FROM Customer WHERE Login = @Login AND Password = @Password";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Login", login);
                cmd.Parameters.AddWithValue("Password", password);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && reader["Login"].ToString() == login
                && reader["Password"].ToString() == password) 
                {
                    if(!Enum.TryParse<UserRole>(reader["Role"].ToString(), out var role))
                        return null;
                    customer = new Customer()
                    {
                        Login = reader["Login"].ToString(),
                        Role = role,
                    };
                }
                connection.Close();
            }
            return customer;
        }

        public bool Create(CustomerSignUpViewModel newCustomer)
        {
            string query = "INSERT INTO Customer (Login, Password, Email, FirstName, LastName, Phone, Role) " +
                            "VALUES (@Login, @Password, @Email, @FirstName, @LastName, @Phone, @Role)";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Login", newCustomer.Login);
                    cmd.Parameters.AddWithValue("Password", newCustomer.Password);
                    cmd.Parameters.AddWithValue("Email", newCustomer.Email);
                    cmd.Parameters.AddWithValue("FirstName", newCustomer.FirstName);
                    cmd.Parameters.AddWithValue("LastName", newCustomer.LastName);
                    cmd.Parameters.AddWithValue("Phone", newCustomer.Phone);
                    cmd.Parameters.AddWithValue("Role", UserRole.User.ToString());

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(CustomerEditViewModel customer)
        {
            string query = "UPDATE Customer SET Email = @Email, FirstName = @FirstName, LastName = @LastName, Phone = @Phone " +
                            "WHERE Login = @Login";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Email", customer.Email);
                    cmd.Parameters.AddWithValue("FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("Login", customer.Login);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Customer> GetCustomers() 
        {
            List<Customer> data = new();

            string query = "SELECT FirstName, LastName, Login, Email, Phone, Role  FROM Customer";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    if (!Enum.TryParse<UserRole>(reader["Role"].ToString(), out var role))
                        throw new Exception("Error of defining User's Role. Check data and try again.");
                    
                    Customer customer = new()
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Login = reader["Login"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Role = role,
                    };

                    data.Add(customer);
                }
                connection.Close();
            }

            return data;
        }

        public bool UpdateRole(UserRole role, string login) 
        {
            string query = "UPDATE Customer SET Role = @Role WHERE Login = @Login";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Role", role.ToString());
                    cmd.Parameters.AddWithValue("Login", login);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UserRole GetUserRoleByLogin(string login) 
        {
            UserRole role = new UserRole();
            string query = "SELECT Role  FROM Customer WHERE Login = @Login";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Login", login);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && !Enum.TryParse<UserRole>(reader["Role"].ToString(), out role))
                {
                    throw new Exception("Error of defining User's Role. Check data and try again.");
                }
                connection.Close();
            }

            return role;
        }
    }
}