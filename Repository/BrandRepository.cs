using Microsoft.Data.SqlClient;
using MotoShop.Data;
using MotoShop.DataTransferObjects;
using MotoShop.Models;

namespace MotoShop.Repository
{
    public class BrandRepository : Repository
    {
        public BrandRepository(DbContext context) : base(context) { }

        public List<Brand> Get() 
        {
            List<Brand> data = new();
            string query = "SELECT * FROM Brand";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Brand model = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        BrandName = Convert.ToString(reader["BrandName"])
                    };

                    data.Add(model);
                }

                connection.Close();
            }

            return data;
        }

        public List<Brand> GetRandomTen() 
        {
            List<Brand> data = new();
            string query = "SELECT TOP 10 * FROM Brand ORDER BY NEWID()";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Brand model = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        BrandName = Convert.ToString(reader["BrandName"])
                    };

                    data.Add(model);
                }

                connection.Close();
            }

            return data;
        }

        public bool Create(Brand newBrand)
        {
            string query = "INSERT INTO Brand (BrandName) " +
                            "VALUES (@BrandName)";

            try 
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("BrandName", newBrand.BrandName);

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
            string query = "DELETE FROM Brand WHERE Id = @Id";
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

        public Brand? GetById(int id)
        {
            Brand? data = null;
            string query = "SELECT * FROM Brand WHERE Id = @Id";

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
                        BrandName = Convert.ToString(reader["BrandName"])
                    };
                }

                connection.Close();
            }

            return data;
        }

        public bool Update(Brand brand)
        {
            string query = "UPDATE Brand SET BrandName = @BrandName " +
                            "WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("BrandName", brand.BrandName);
                    cmd.Parameters.AddWithValue("Id", brand.Id);

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

        public Dictionary<string, double> GetBrandsAveragePrices() 
        {
            Dictionary<string, double> data = new();

            string query = "SELECT BrandName, AVG(Price) as AvgPrice FROM Motorcycle " +
                "JOIN Model on Model.Id = Motorcycle.ModelId JOIN Brand on Brand.Id = Model.BrandId " +
                "GROUP BY BrandName ORDER BY AvgPrice";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    double.TryParse(reader["AvgPrice"].ToString(), out double price);

                    data.Add(reader["BrandName"].ToString(), Math.Round(price, 2));
                }

                connection.Close();
            }

            return data;
        }

        public List<BrandStatsDto> GetStats() 
        {
            List<BrandStatsDto> data = new();

            string query = "SELECT BrandName, COUNT(BrandName) AS Number " +
                "FROM Purchase JOIN Motorcycle ON Motorcycle.VIN = Purchase.MotorcycleVIN " +
                "JOIN Model ON Motorcycle.ModelId = Model.Id JOIN Brand ON Model.BrandId = Brand.Id " +
                "GROUP BY BrandName ORDER BY Number DESC";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BrandStatsDto model = new()
                    {
                        BrandName = Convert.ToString(reader["BrandName"]),
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
