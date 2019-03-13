using Data;
using MySql.Data.MySqlClient;
using Password;

namespace DBProtocol
{
    public class DatabaseProtocol
    {
        public static void RegisterUser(string username, string email, string password)

        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "googleaidb";
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand("INSERT INTO `users` (username, email, password) VALUES (?username, ?email, ?password)", dbCon.Connection);
                cmd.Parameters.AddWithValue("?username", username);
                cmd.Parameters.AddWithValue("?email", email);
                cmd.Parameters.AddWithValue("?password", SaltAndHash.Hash(password));
                var reader = cmd.ExecuteReader();
                dbCon.Close();
            }
        }


    }
}
