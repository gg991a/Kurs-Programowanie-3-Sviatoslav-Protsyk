using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace SportShopApp
{
    public class DatabaseHelper
    {
        private string connectionString = @"Server=localhost\SQLEXPRESS; Database=SportShopDB; Trusted_Connection=True; TrustServerCertificate=True;";

        public User Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Users WHERE Username = @u AND Password = @p";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = (int)reader["Id"],
                                Username = reader["Username"].ToString(),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Products";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : "",
                                Price = reader["Price"] != DBNull.Value ? (decimal)reader["Price"] : 0,
                                ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : "",
                                Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0
                            });
                        }
                    }
                }
            }
            return products;
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Products (Name, Category, Price, ImagePath, Quantity) VALUES (@Name, @Category, @Price, @ImagePath, @Quantity)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

                    if (string.IsNullOrEmpty(product.ImagePath))
                        cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void BuyProduct(User user, Product product, int amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string checkSql = "SELECT Quantity FROM Products WHERE Id = @Id";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn, transaction);
                    checkCmd.Parameters.AddWithValue("@Id", product.Id);
                    int currentQty = (int)checkCmd.ExecuteScalar();

                    if (currentQty < amount)
                        throw new Exception($"Za mało towaru! Dostępne tylko: {currentQty} szt.");

                    string updateSql = "UPDATE Products SET Quantity = Quantity - @Amount WHERE Id = @Id";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@Id", product.Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();

                    string orderSql = "INSERT INTO Orders (UserName, ProductName, Quantity, OrderDate) VALUES (@User, @Product, @Amount, @Date)";
                    SqlCommand orderCmd = new SqlCommand(orderSql, conn, transaction);
                    orderCmd.Parameters.AddWithValue("@User", user.Username);
                    orderCmd.Parameters.AddWithValue("@Product", product.Name);
                    orderCmd.Parameters.AddWithValue("@Amount", amount);
                    orderCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    orderCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Orders ORDER BY OrderDate DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                Id = (int)reader["Id"],
                                UserName = reader["UserName"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = (int)reader["Quantity"],
                                OrderDate = (DateTime)reader["OrderDate"]
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public void UpdateProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE Products SET Name=@Name, Category=@Category, Price=@Price, Quantity=@Quantity, ImagePath=@ImagePath WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

                    if (string.IsNullOrEmpty(product.ImagePath))
                        cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}