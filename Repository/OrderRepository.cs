using Microsoft.Data.SqlClient;
using MotoShop.Data;
using MotoShop.Models;
using MotoShop.DataTransferObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MotoShop.Repository
{
    public class OrderRepository : Repository
    {
        public OrderRepository(DbContext context) : base(context) { }

        public List<OrderDto> GetOrders(string customer) 
        {
            List<OrderDto> data = new();

            string query = "SELECT * FROM Purchase JOIN Motorcycle ON Purchase.MotorcycleVIN = Motorcycle.VIN " +
                "JOIN Type ON Motorcycle.TypeId = Type.Id JOIN Model ON Motorcycle.ModelId = Model.Id " +
                "JOIN Brand ON Model.BrandId = Brand.Id " +
                "WHERE Customer = @Customer";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Customer", customer);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    data.Add(new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        VIN = Convert.ToString(reader["MotorcycleVIN"]),
                        Model = Convert.ToString(reader["ModelName"]),
                        Brand = Convert.ToString(reader["BrandName"]),
                        Type = Convert.ToString(reader["TypeName"]),
                        Manufactured = DateOnly.FromDateTime(Convert.ToDateTime(reader["Manufactored"])),
                        EngineDisplacement = Convert.ToDouble(reader["EngineDisplacement"])*1000,
                        Price = Convert.ToDouble(reader["Price"]),
                    });
                }

                connection.Close();
            }

            return data;
        }

        public bool Create(Order model) 
        {
            string query = "INSERT INTO Purchase (Customer, MotorcycleVIN) " +
                            "VALUES (@Customer, @MotorcycleVIN)";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Customer", model.Customer);
                    cmd.Parameters.AddWithValue("MotorcycleVIN", model.MotorcycleVIN);

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

        public bool Delete(int id) 
        {
            string query = "DELETE FROM Purchase WHERE Id = @Id";
            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Id", id);

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

        public bool ConfirmOrder(string? login)
        {
            if (login is null)
                return false;
            
            string query = "UPDATE Purchase SET Customer = null WHERE Customer = @Customer";
            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Customer", login);

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
    }
}
