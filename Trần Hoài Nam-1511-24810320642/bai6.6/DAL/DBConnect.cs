using System.Data.SqlClient;

namespace DAL
{
    public class DBConnect
    {
        
        private static readonly string connectionString =
            @"Data Source=.;Initial Catalog=LibraryDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}