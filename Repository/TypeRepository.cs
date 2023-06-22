using Microsoft.Data.SqlClient;
using MotoShop.Data;
using MotoShop.DataTransferObjects;
using MotoShop.Models;
using Type = MotoShop.Models.Type;

namespace MotoShop.Repository
{
    public class TypeRepository : Repository
    {
        public TypeRepository(DbContext context) : base(context) { }

        public List<Type> Get()
        {
            List<Type> data = new();
            string query = "SELECT * FROM Type";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Type model = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        TypeName = Convert.ToString(reader["TypeName"])
                    };

                    data.Add(model);
                }

                connection.Close();
            }

            return data;
        }

        public bool Create(Type newType)
        {
            string query = "INSERT INTO Type (TypeName) " +
                            "VALUES (@TypeName)";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("TypeName", newType.TypeName);

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
            string query = "DELETE FROM Type WHERE Id = @Id";
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

        public Type? GetById(int id)
        {
            Type? data = null;
            string query = "SELECT * FROM Type WHERE Id = @Id";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    data = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        TypeName = Convert.ToString(reader["TypeName"])
                    };
                }

                connection.Close();
            }

            return data;
        }

        public bool Update(Type type)
        {
            string query = "UPDATE Type SET TypeName = @TypeName " +
                            "WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("TypeName", type.TypeName);
                    cmd.Parameters.AddWithValue("Id", type.Id);

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

        public Dictionary<string, double> GetTypesAveragePrices()
        {
            Dictionary<string, double> data = new();

            string query = "SELECT TypeName, AVG(Price) as AvgPrice " +
                "FROM Motorcycle JOIN Type on Type.Id = Motorcycle.TypeId " +
                "GROUP BY TypeName ORDER BY AvgPrice";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    double.TryParse(reader["AvgPrice"].ToString(), out double price);

                    data.Add(reader["TypeName"].ToString(), Math.Round(price, 2));
                }

                connection.Close();
            }

            return data;
        }

        public List<TypeStatsDto> GetStats()
        {
            List<TypeStatsDto> data = new();

            string query = "SELECT TypeName, COUNT(TypeName) AS Number " +
                "FROM Purchase JOIN Motorcycle ON Motorcycle.VIN = Purchase.MotorcycleVIN " +
                "JOIN Type ON Motorcycle.TypeId = Type.Id " +
                "GROUP BY TypeName ORDER BY Number DESC";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TypeStatsDto model = new()
                    {
                        TypeName = Convert.ToString(reader["TypeName"]),
                        Number = Convert.ToInt32(reader["Number"])
                    };

                    data.Add(model);
                }

                connection.Close();
            }

            return data;
        }
    }
}
