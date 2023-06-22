using Microsoft.Data.SqlClient;
using MotoShop.Data;
using MotoShop.DataTransferObjects;
using MotoShop.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MotoShop.Repository
{
    public class ModelRepository : Repository
    {
        public ModelRepository(DbContext context) : base(context) { }

        public List<Model> Get()
        {
            List<Model> data = new();
            string query = "SELECT * FROM Model ORDER BY BrandId, ModelName";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Model model = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        BrandId = (reader["BrandId"] is DBNull) ? null : Convert.ToInt32(reader["BrandId"]),
                        ModelName = Convert.ToString(reader["ModelName"]),
                    };

                    data.Add(model);
                }

                connection.Close();
            }

            return data;
        }

        public BrandModelDto GetByModelId(int id)
        {
            BrandModelDto viewModel = new();

            string query = "SELECT* FROM Model JOIN Brand on Model.BrandId = Brand.Id WHERE Model.Id = @Id";
            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && Convert.ToInt32(reader["Id"]) == id)
                {
                    viewModel = new()
                    {
                        BrandId = (reader["BrandId"] is DBNull) ? null : Convert.ToInt32(reader["BrandId"]),
                        ModelId = Convert.ToInt32(reader["ModelId"]),
                        ModelName = Convert.ToString(reader["ModelName"]),
                        BrandName = Convert.ToString(reader["BrandName"])
                    };
                }
                connection.Close();
            }
            return viewModel;
        }

        public List<Model> GetByBrandId(int id)
        {
            List<Model> data = new();

            string query = "SELECT Model.Id, Model.ModelName, Model.BrandId " +
                "FROM Model JOIN Brand on Model.BrandId = Brand.Id " +
                "WHERE Brand.Id = @Id ORDER BY Model.ModelName";
            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Model model = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        BrandId = (reader["BrandId"] is DBNull) ? null : Convert.ToInt32(reader["BrandId"]),
                        ModelName = Convert.ToString(reader["ModelName"]),
                    };

                    data.Add(model);
                }
                connection.Close();
            }
            return data;
        }

        public bool Create(Model newModel)
        {
            string query = "INSERT INTO Model (BrandId, ModelName) " +
                            "VALUES (@BrandId, @ModelName)";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("BrandId", newModel.BrandId);
                    cmd.Parameters.AddWithValue("ModelName", newModel.ModelName);

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
            string query = "DELETE FROM Model WHERE Id = @Id";
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

        public Model? GetById(int id)
        {
            Model? model = null;

            string query = "SELECT * FROM Model WHERE Model.Id = @Id";
            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("Id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && Convert.ToInt32(reader["Id"]) == id)
                {
                    model = new()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        BrandId = (reader["BrandId"] is DBNull) ? null : Convert.ToInt32(reader["BrandId"]),
                        ModelName = Convert.ToString(reader["ModelName"]),
                    };
                }
                connection.Close();
            }
            return model;
        }

        public bool Update(Model model)
        {
            string query = "UPDATE Model SET BrandId = @BrandId, ModelName = @ModelName WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Id", model.Id);
                    cmd.Parameters.AddWithValue("BrandId", model.BrandId);
                    cmd.Parameters.AddWithValue("ModelName", model.ModelName);

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

        public Dictionary<string, double>? GetModelPriceDictionary(string brandName) 
        {
            Dictionary<string, double> data = new();

            string query = "SELECT ModelName, AVG(Price) as AvgPrice FROM Motorcycle " +
                "JOIN Model on Model.Id = Motorcycle.ModelId JOIN Brand on Brand.Id = Model.BrandId " +
                "WHERE BrandName = @BrandName GROUP BY ModelName, BrandName ORDER BY BrandName, ModelName";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("BrandName", brandName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    double.TryParse(reader["AvgPrice"].ToString(), out double price);

                    data.Add(reader["ModelName"].ToString(), Math.Round(price, 2));
                }
                connection.Close();
            }
            
            return data.Count > 0 ? data : null;
        }

        public List<ModelStatsDto> GetStats()
        {
            List<ModelStatsDto> data = new();

            string query = "SELECT ModelName, COUNT(ModelName) AS Number " +
                "FROM Purchase JOIN Motorcycle ON Motorcycle.VIN = Purchase.MotorcycleVIN " +
                "JOIN Model ON Motorcycle.ModelId = Model.Id " +
                "GROUP BY ModelName ORDER BY Number DESC";

            using (SqlConnection connection = _context.GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ModelStatsDto model = new()
                    {
                        ModelName = Convert.ToString(reader["ModelName"]),
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
