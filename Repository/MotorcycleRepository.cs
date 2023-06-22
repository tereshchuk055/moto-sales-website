using Microsoft.Data.SqlClient;
using MotoShop.Data;
using MotoShop.DataTransferObjects;
using MotoShop.Constants;
using MotoShop.Models;
using MotoShop.ViewModels.MotorcycleViewModels;
using System.Reflection.Metadata;

namespace MotoShop.Repository
{
    public class MotorcycleRepository : Repository
    {
        public MotorcycleRepository(DbContext context) : base(context) { }

        public List<MotorcycleDto> GetNextEight(int number)
        {
            List<MotorcycleDto> data = new();

            string query = "SELECT * FROM Motorcycle JOIN Model ON Motorcycle.ModelId = Model.Id " +
                "JOIN Brand ON Model.BrandId = Brand.Id JOIN Type ON Motorcycle.TypeId = Type.Id " +
                "WHERE Visible = 1 ORDER BY BrandName, ModelName OFFSET @Number ROWS FETCH NEXT 8 ROWS ONLY";

            using var connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("Number", number);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MotorcycleDto model = new()
                {
                    VIN = reader["VIN"].ToString(),
                    Model = reader["ModelName"].ToString(),
                    Brand = reader["BrandName"].ToString(),
                    Price = Convert.ToDouble(reader["Price"]),
                    EngineDisplacement = Convert.ToDouble(reader["EngineDisplacement"]),
                    Manufactured = reader["Manufactored"].ToString()?[..10],
                    Type = reader["TypeName"].ToString(),
                    Photo = reader["PhotoPath"].ToString()
                };

                data.Add(model);
            }

            connection.Close();

            return data;
        }

        public bool IsVinExist(string param)
        {
            string query = "SELECT VIN FROM Motorcycle WHERE VIN = @VIN";
            using SqlConnection connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("VIN", param);
            SqlDataReader reader = cmd.ExecuteReader();

            bool state = reader.Read() && reader["VIN"].ToString() == param;
            connection.Close();

            return state;
        }

        public bool IsMotorcycleVisible(string param)
        {
            string query = "SELECT Visible FROM Motorcycle WHERE VIN = @VIN";
            using SqlConnection connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("VIN", param);
            SqlDataReader reader = cmd.ExecuteReader();
            bool state = false;
            if (reader.Read())
                bool.TryParse(reader["Visible"].ToString(), out state);
            connection.Close();

            return state;
        }

        public List<MotorcycleDto> GetFiveNewest()
        {
            List<MotorcycleDto> data = new();

            string query = "SELECT TOP 5 BrandName, ModelName, MAX(Manufactored) AS Manufactored, " +
                            "MAX(PhotoPath) AS PhotoPath, MAX(TypeName) AS TypeName " +
                            "FROM Motorcycle " +
                            "JOIN Model ON Motorcycle.ModelId = Model.Id " +
                            "JOIN Brand ON Model.BrandId = Brand.Id " +
                            "JOIN Type ON Motorcycle.TypeId = Type.Id " +
                            "WHERE Visible = 1 " +
                            "GROUP BY ModelName, BrandName " +
                            "ORDER BY Manufactored DESC";

            using SqlConnection connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MotorcycleDto model = new()
                {
                    Model = reader["ModelName"].ToString(),
                    Brand = reader["BrandName"].ToString(),
                    Type = reader["TypeName"].ToString(),
                    Manufactured = reader["Manufactored"].ToString()?[..10],
                    Photo = reader["PhotoPath"].ToString(),
                };

                data.Add(model);
            }

            connection.Close();

            return data;
        }

        public bool Create(Motorcycle newMotorcycle)
        {
            string query = "INSERT INTO Motorcycle (VIN, ModelId, TypeId, EngineDisplacement, " +
                            "Price, Manufactored, PhotoPath, Visible) " +
                            "VALUES (@VIN, @ModelId, @TypeId, @EngineDisplacement, @Price, " +
                            "@Manufactored, @PhotoPath, @Visible)";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("VIN", newMotorcycle.VIN);
                    cmd.Parameters.AddWithValue("ModelId", newMotorcycle.ModelId);
                    cmd.Parameters.AddWithValue("TypeId", newMotorcycle.TypeId);
                    cmd.Parameters.AddWithValue("EngineDisplacement", newMotorcycle.EngineDisplacement);
                    cmd.Parameters.AddWithValue("Price", newMotorcycle.Price);
                    cmd.Parameters.AddWithValue("Manufactored", newMotorcycle.Manufactured);
                    cmd.Parameters.AddWithValue("Visible", newMotorcycle.Visible);
                    cmd.Parameters.AddWithValue("PhotoPath", newMotorcycle.PhotoPath);

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

        public List<MotorcycleDto> Search(string? value)
        {
            List<MotorcycleDto> data = new();

            using var connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new("Search", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter parameter = new("@Value", System.Data.SqlDbType.VarChar);
            parameter.Value = value?? " ";
            cmd.Parameters.Add(parameter);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MotorcycleDto model = new()
                {
                    VIN = reader["VIN"].ToString(),
                    Model = reader["ModelName"].ToString(),
                    Brand = reader["BrandName"].ToString(),
                    Price = Convert.ToDouble(reader["Price"]),
                    EngineDisplacement = Convert.ToDouble(reader["EngineDisplacement"]),
                    Manufactured = reader["Manufactored"].ToString()?[..10],
                    Type = reader["TypeName"].ToString(),
                    Photo = reader["PhotoPath"].ToString()
                };

                data.Add(model);
            }

            connection.Close();

            return data;
        }

        public List<MotorcycleDto> Get()
        {
            List<MotorcycleDto> data = new();

            string query = "SELECT * FROM Motorcycle LEFT JOIN Model ON Motorcycle.ModelId = Model.Id " +
                "LEFT JOIN Brand ON Model.BrandId = Brand.Id LEFT JOIN Type ON Motorcycle.TypeId = Type.Id " +
                "ORDER BY BrandId, ModelId, Visible";

            using var connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MotorcycleDto model = new()
                {
                    VIN = reader["VIN"].ToString(),
                    Model = string.IsNullOrEmpty(reader["ModelName"].ToString()) ? 
                        "Undefined" : reader["ModelName"].ToString(),
                    Brand = string.IsNullOrEmpty(reader["BrandName"].ToString()) ? 
                        "Undefined" : reader["BrandName"].ToString(),
                    Price = Convert.ToDouble(reader["Price"]),
                    EngineDisplacement = Convert.ToDouble(reader["EngineDisplacement"]),
                    Manufactured = reader["Manufactored"].ToString()?[..10],
                    Type = string.IsNullOrEmpty(reader["TypeName"].ToString()) ? 
                        "Undefined" : reader["TypeName"].ToString(),
                    Photo = reader["PhotoPath"].ToString(),
                    Visible = Convert.ToBoolean(reader["Visible"])
                };

                data.Add(model);
            }

            connection.Close();

            return data;
        }

        public bool Delete(string vin)
        {
            string query = "DELETE FROM Motorcycle WHERE VIN = @Vin";
            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Vin", vin);

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

        public MotorcycleEditViewModel? GetEditVMById(string vin)
        {
            MotorcycleEditViewModel? data = null;

            string query = "SELECT * FROM Motorcycle LEFT JOIN Model ON Motorcycle.ModelId = Model.Id " +
                            "LEFT JOIN Brand ON Model.BrandId = Brand.Id WHERE VIN = @Vin";

            using SqlConnection connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("Vin", vin);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                data = new()
                {
                    VIN = reader["VIN"].ToString(),
                    ModelId = string.IsNullOrEmpty(reader["ModelId"].ToString()) ? null : Convert.ToInt32(reader["ModelId"]),
                    TypeId = string.IsNullOrEmpty(reader["TypeId"].ToString()) ? null : Convert.ToInt32(reader["TypeId"]),
                    BrandId = string.IsNullOrEmpty(reader["BrandId"].ToString()) ? null : Convert.ToInt32(reader["BrandId"]),
                    EngineDisplacement = reader["EngineDisplacement"].ToString(),
                    Price = float.Parse(reader["Price"].ToString()),
                    Manufactured = DateOnly.FromDateTime(Convert.ToDateTime(reader["Manufactored"])),
                    Visible = Convert.ToBoolean(reader["Visible"])
                };
            }

            connection.Close();

            return data;
        }

        public Motorcycle? GetById(string vin)
        {
            Motorcycle? data = null;

            string query = "SELECT * FROM Motorcycle JOIN Model ON Motorcycle.ModelId = Model.Id " +
                            "JOIN Brand ON Model.BrandId = Brand.Id WHERE VIN = @Vin";

            using SqlConnection connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("Vin", vin);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                data = new()
                {
                    VIN = reader["VIN"].ToString(),
                    ModelId = string.IsNullOrEmpty(reader["ModelId"].ToString()) ? null : Convert.ToInt32(reader["ModelId"]),
                    TypeId = string.IsNullOrEmpty(reader["TypeId"].ToString()) ? null : Convert.ToInt32(reader["TypeId"]),
                    EngineDisplacement = reader["EngineDisplacement"].ToString(),
                    Price = float.Parse(reader["Price"].ToString()),
                    Manufactured = DateOnly.FromDateTime(Convert.ToDateTime(reader["Manufactored"])),
                    Visible = Convert.ToBoolean(reader["Visible"]),
                    PhotoPath = reader["PhotoPath"].ToString()
                };
            }

            connection.Close();

            return data;
        }

        public bool Update(Motorcycle model)
        {
            string query = "UPDATE Motorcycle SET ModelId = @ModelId, TypeId = @TypeId, " +
                "EngineDisplacement = @EngineDisplacement, Price = @Price, " +
                "Manufactored = @Manufactored, PhotoPath = @PhotoPath, Visible = @Visible " +
                "WHERE VIN = @Vin";

            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new(query, connection);
                    cmd.Parameters.AddWithValue("Vin", model.VIN);
                    cmd.Parameters.AddWithValue("ModelId", model.ModelId);
                    cmd.Parameters.AddWithValue("TypeId", model.TypeId);
                    cmd.Parameters.AddWithValue("EngineDisplacement", model.EngineDisplacement);
                    cmd.Parameters.AddWithValue("Price", model.Price);
                    cmd.Parameters.AddWithValue("Manufactored", model.Manufactured);
                    cmd.Parameters.AddWithValue("PhotoPath", model.PhotoPath);
                    cmd.Parameters.AddWithValue("Visible", model.Visible);

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

        public List<MotorcycleDto> GetByBrandId(int id) 
        {
            List<MotorcycleDto> data = new();

            string query = "SELECT * FROM Motorcycle JOIN Model ON Motorcycle.ModelId = Model.Id " +
                "JOIN Brand ON Model.BrandId = Brand.Id JOIN Type ON Motorcycle.TypeId = Type.Id " +
                "WHERE Visible = 1 AND BrandId = @Id ORDER BY BrandId, ModelId";

            using var connection = _context.GetConnection();
            connection.Open();

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("Id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MotorcycleDto model = new()
                {
                    VIN = reader["VIN"].ToString(),
                    Model = reader["ModelName"].ToString(),
                    Brand = reader["BrandName"].ToString(),
                    Price = Convert.ToDouble(reader["Price"]),
                    EngineDisplacement = Convert.ToDouble(reader["EngineDisplacement"])*1000,
                    Manufactured = reader["Manufactored"].ToString()?[..10],
                    Type = reader["TypeName"].ToString(),
                    Photo = reader["PhotoPath"].ToString(),
                    Visible = Convert.ToBoolean(reader["Visible"])
                };

                data.Add(model);
            }

            connection.Close();

            return data;
        }

        public bool CreateBackup() 
        {
            string query = $"BACKUP DATABASE MotoShop TO DISK = '{Environment.CurrentDirectory}\\Temp\\MotoShop_FullDbBkup.bak' " +
                "WITH INIT, NAME = 'MotoShop Full Database backup', DESCRIPTION = 'MotoShop Full Database backup'";
            try
            {
                using (SqlConnection connection = _context.GetConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new(query, connection);

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
