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


                cmd.Parameters.AddWithValue("?username", username);//username Protocol
                cmd.Parameters.AddWithValue("?email", email);//email Protocol
                cmd.Parameters.AddWithValue("?password", SaltAndHash.Hash(password));//password Protocol
                

                var reader = cmd.ExecuteReader();
                /*
                 * sends the SQL statements to the connection
                 *object and populates a SqlDataReader object
                 * based on the SQL statement
                */

                dbCon.Close();
            }
        }

        public static void UserLogin(string username, string email, string password)
        {
            var dbCon = DBConnection.Instance();

            var cmd = new MySqlCommand("CHECK FROM `users` (username, email, password) VALUES (?username, ?email, ?password)", dbCon.Connection);

            cmd.Parameters.AddWithValue("?username",username);
            cmd.Parameters.AddWithValue("?email", email);
            cmd.Parameters.AddWithValue("?password", password);

            var reader = cmd.ExecuteReader();

            dbCon.Close();

        }

        public static void MessagingInformation(string user1, string user2, string chat_id)
        {//chat_id is to find the messaging history and load it easier
            var dbCon = DBConnection.Instance();
            //connection instance
            //dbCon.DatabaseName = "googleaidb";
            //gets the database name
            //if (dbCon.IsConnect())//if the database is connected
            //{
                var cmd = new MySqlCommand("INSERT INTO `users` (user1, user2, chat_id) VALUES (?user1, ?user2, ?chat_id)", dbCon.Connection);
                //make an instance of MySqlCommand

                cmd.Parameters.AddWithValue("?user1", user1);//user1 Protocol
                cmd.Parameters.AddWithValue("?user2", user2);
                cmd.Parameters.AddWithValue("?chat_id", chat_id);//chat_id Protocol


                var reader = cmd.ExecuteReader();

                //get chat_id by matching the chat_idNumber to each user to make sure they have messaged each other
                //created when two users start to chat

                dbCon.Close();
            //}
        }

        public static void LobbyStatus(string lobby_id, string user1, string user2)
        {//the game will only need the information of the two players to "ready up" before returning a boolean that starts the game
            var dbCon = DBConnection.Instance();

            var cmd = new MySqlCommand("INSERT INTO `users` (username, chat_id) VALUES (?lobby_id, ?user1, ?user2)", dbCon.Connection);

            cmd.Parameters.AddWithValue("?lobby_id", lobby_id);
            cmd.Parameters.AddWithValue("?user1", user1);
            cmd.Parameters.AddWithValue("?user2", user2);

            var reader = cmd.ExecuteReader();

            dbCon.Close();

        }



    }//end of public class Protocol
}
