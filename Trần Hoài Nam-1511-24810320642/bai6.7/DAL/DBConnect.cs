using System.Data.SqlClient;

namespace DAL
{
    public class DBConnect
    {
        private static readonly string connectionString =
            @"Data Source=.;Initial Catalog=ClinicDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}