using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using PortifolioTeste1.Persistence;

namespace PortifolioTeste1.Connection
{
    public class Conn
    {
        public static string getSql()
        {
            string cnnnn = "";
            string r = "";

            try
            {
                //cnnnn = WebApplication.CreateBuilder().Configuration.GetConnectionString("DevEventsCsBoatHom");
                cnnnn = WebApplication.CreateBuilder().Configuration.GetConnectionString("DevEventsCs");

                //r = ConfigurationManager.ConnectionStrings["DevEventsCs"].ConnectionString;
            }
            catch (Exception)
            {

            }

            return cnnnn;
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(getSql());
            return conn;
        }
    }
}
