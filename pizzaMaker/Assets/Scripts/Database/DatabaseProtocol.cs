using MySql.Data.MySqlClient;
using System;
using TMPro;
using UnityEngine;

public class DatabaseProtocol : MonoBehaviour
{

    public TMP_InputField emailInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;


    //public void RegisterUser(string username, string email, string password)
    public void RegisterUser()
    {
        String username = usernameInputField.text;
        String email = emailInputField.text;
        String password = passwordInputField.text;
        var dbCon = DBConnection.Instance();
        dbCon.DatabaseName = "googleaidb";
        if (dbCon.IsConnect())
        {
           // cmd2 = gameObject.AddComponent<MySqlCommand>();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `users` (username, email, password) VALUES (?username, ?email, ?password)", dbCon.Connection);
            cmd.Parameters.AddWithValue("?username", username);
            cmd.Parameters.AddWithValue("?email", email);
            cmd.Parameters.AddWithValue("?password", SaltAndHash.Hash(password));
            var reader = cmd.ExecuteReader();
            dbCon.Close();
        }
    }

    public void printStuff(string printed)
    {
        Debug.Log(emailInputField.text);
    }

}
