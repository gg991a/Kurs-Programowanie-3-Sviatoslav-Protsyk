using System;
using System.Data.SqlClient;

namespace SklepSportowy
{
    public class Baza
    {
        private readonly string connectionString = @"Server=localhost\SQLEXPRESS;Database=SklepSportowy;Trusted_Connection=True;TrustServerCertificate=True;";
        public void PolaczIPobierz()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("--> Połączenie udane!");

                    string query = @"
                        SELECT p.Nazwa, p.Cena, k.Nazwa as Kategoria
                        FROM Produkty p
                        JOIN Kategorie k ON p.KategoriaId = k.Id";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("\n--- TOWARY ---");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Nazwa"]} - {reader["Cena"]} zł ({reader["Kategoria"]})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("BŁĄD: " + ex.Message);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Uruchamianie programu...");

            Baza baza = new Baza();
            baza.PolaczIPobierz();

            Console.ReadKey();
        }
    }
}