using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    // Пример метода для получения всех продуктов
    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();
        
        using (var connection = GetConnection())
        {
            var command = new SqlCommand(
                @"SELECT p.*, b.BrandName, b.Description as BrandDescription, b.FoundedYear 
                  FROM Products p 
                  JOIN Brands b ON p.BrandId = b.BrandId", 
                connection);
            
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        StockQuantity = (int)reader["StockQuantity"],
                        BrandId = (int)reader["BrandId"],
                        Brand = new Brand
                        {
                            BrandId = (int)reader["BrandId"],
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["BrandDescription"].ToString(),
                            FoundedYear = reader["FoundedYear"] != DBNull.Value ? (int?)reader["FoundedYear"] : null
                        }
                    });
                }
            }
        }
        
        return products;
    }

    // Добавьте аналогичные методы для других сущностей...
}
