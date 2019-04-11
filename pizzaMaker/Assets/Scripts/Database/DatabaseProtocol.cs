using MySql.Data.MySqlClient;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabaseProtocol : MonoBehaviour
{

    public TMP_InputField emailInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;


    public void RegisterUser()
    {
        string username = usernameInputField.text;
        string email = emailInputField.text;
        string password = (passwordInputField.text.Length > 0) ? passwordInputField.text : null;

        if (password != null)
        {
            if (AccountExists(username, email))
            {
                Debug.Log("Account Exists");
                return;
            }
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "googleaidb";
            if (dbCon.IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `users` (username, email, password) VALUES (?username, ?email, ?password)", dbCon.Connection);
                cmd.Parameters.AddWithValue("?username", username);
                cmd.Parameters.AddWithValue("?email", email);
                cmd.Parameters.AddWithValue("?password", SaltAndHash.Hash(password));
                var reader = cmd.ExecuteReader();
                dbCon.Close();
                SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
            }
        }
        else
        {
            Debug.Log("Password can not be blank");
        }

    }

    //Return True if Account Exists
    private bool AccountExists(string givenUsername, string givenEmail)
    {
        var dbCon = DBConnection.Instance();
        dbCon.DatabaseName = "googleaidb";
        if (dbCon.IsConnect())
        {
            MySqlCommand cmd = new MySqlCommand("SELECT EXISTS(SELECT 1 FROM `users` WHERE (`username` = ?username) OR (`email` = ?email))", dbCon.Connection);
            cmd.Parameters.AddWithValue("?username", givenUsername);
            cmd.Parameters.AddWithValue("?email", givenEmail);
            int result = Convert.ToInt16(cmd.ExecuteScalar());
            dbCon.Close();
            return result == 1 ? true : false;
        }
        return true;
    }

    //Return true if Successfully logged in
    public void LoginUser()
    {
        string username = usernameInputField.text;
        string password = (passwordInputField.text.Length > 0) ? passwordInputField.text : null;
        if (password != null)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "googleaidb";
            if (dbCon.IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT `password` from `users` WHERE `username` = ?username LIMIT 1", dbCon.Connection);
                cmd.Parameters.AddWithValue("?username", username);
                string hashedPassword = Convert.ToString(cmd.ExecuteScalar());
                if (SaltAndHash.Verify(password, hashedPassword))
                {
                    SetOnline(username);
                    SceneManager.LoadScene("GameMenuScene", LoadSceneMode.Single);
                }
                else
                {
                    Debug.Log("Incorrect Username or Password");
                }

            }
            dbCon.Close();
        }
        else
        {
            Debug.Log("Incorrect Username or Password");
        }



        //return result;
    }

    public bool SetOnline(string givenUsername)
    {
        var dbCon = DBConnection.Instance();
        dbCon.DatabaseName = "googleaidb";
        if (dbCon.IsConnect())
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = 1 WHERE `username` =?username", dbCon.Connection);
            cmd.Parameters.AddWithValue("?username", givenUsername);
            int result = Convert.ToInt16(cmd.ExecuteScalar());
            dbCon.Close();
            return result == 1 ? true : false;
        }
        return false;
    }

    public bool SetOffline(string givenUsername)
    {
        var dbCon = DBConnection.Instance();
        dbCon.DatabaseName = "googleaidb";
        if (dbCon.IsConnect())
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = 0 WHERE `username` =?username", dbCon.Connection);
            cmd.Parameters.AddWithValue("?username", givenUsername);
            int result = Convert.ToInt16(cmd.ExecuteScalar());
            dbCon.Close();
            return result == 1 ? true : false;
        }
        return false;
    }
    //Debug Stuff
    //public void PrintStuff(string printed)
    //{
    //    Debug.Log(emailInputField.text);
    //}

}
