using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace wasalha
{
    public class DATABASE
    {
        private static string connectionString = "Data Source=DESKTOP-4BCE9JE\\SQLEXPRESS;Initial Catalog=shipping_system;Integrated Security=True";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
