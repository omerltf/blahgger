using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Blahgger.Models
{
    public class BlogHelper
    {
        public int GetCurrentUserId(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Blahgger"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                      SELECT Id, Email, Password
                      FROM Users
                      where Email=@Email
                    ";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
            }
            return -1;
        }
    }
}