using Assets.Scripts;
using MySql.Data.MySqlClient;
using System;
using System.Net;
using System.Net.Sockets;
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
            string toSend = "register/%username=" + username + "%password=" + password + "%email=" + email + "%";

            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(Constant.Server), 4343);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverAddress);

            // Sending
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            clientSocket.Send(toSendLenBytes);
            clientSocket.Send(toSendBytes);

            // Receiving
            byte[] rcvLenBytes = new byte[4];
            clientSocket.Receive(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] rcvBytes = new byte[rcvLen];
            clientSocket.Receive(rcvBytes);
            string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

            Debug.Log("Client received: " + rcv);
            if (rcv == "Success")
            {
                SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
            }
            else
            {
                Debug.Log(rcv);
            }
            clientSocket.Close();
        }
        else
        {
            Debug.Log("Password can not be blank");
        }

    }


    //Return True if Account Exists
    //private bool AccountExists(string givenUsername, string givenEmail)
    //{
    //    var dbCon = DBConnection.Instance();
    //    dbCon.DatabaseName = "googleaidb";
    //    if (dbCon.IsConnect())
    //    {
    //        MySqlCommand cmd = new MySqlCommand("SELECT EXISTS(SELECT 1 FROM `users` WHERE (`username` = ?username) OR (`email` = ?email))", dbCon.Connection);
    //        cmd.Parameters.AddWithValue("?username", givenUsername);
    //        cmd.Parameters.AddWithValue("?email", givenEmail);
    //        int result = Convert.ToInt16(cmd.ExecuteScalar());
    //        dbCon.Close();
    //        return result == 1 ? true : false;
    //    }
    //    return true;
    //}

    //Return true if Successfully logged in
    public void LoginUser()
    {
        string username = usernameInputField.text;
        string password = (passwordInputField.text.Length > 0) ? passwordInputField.text : null;
        if (password != null)
        {
            string toSend = "login/%username=" + username + "%password=" + password + "%";

            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(Constant.Server), 4343);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverAddress);

            // Sending
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            clientSocket.Send(toSendLenBytes);
            clientSocket.Send(toSendBytes);

            // Receiving
            byte[] rcvLenBytes = new byte[4];
            clientSocket.Receive(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] rcvBytes = new byte[rcvLen];
            clientSocket.Receive(rcvBytes);
            string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

            Debug.Log("Client received in Login: " + rcv);
            if (rcv == "Success")
            {
                PlayerPrefs.SetString("Username", username);
                SceneManager.LoadScene("GameMenuScene", LoadSceneMode.Single);
            }
            else
            {
                Debug.Log(rcv);
            }
            clientSocket.Close();
        }
        else
        {
            Debug.Log("Incorrect Username or Password");
        }



        //return result;
    }

    //public void SetOnline()
    //{
    //    var dbCon = DBConnection.Instance();
    //    dbCon.DatabaseName = "googleaidb";
    //    if (dbCon.IsConnect())
    //    {
    //        MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = 1 WHERE `username` =?username", dbCon.Connection);
    //        cmd.Parameters.AddWithValue("?username", PlayerPrefs.GetString("Username"));
    //        int result = Convert.ToInt16(cmd.ExecuteScalar());
    //        dbCon.Close();
    //    }
    //}

    //public void SetOnline(string givenUsername)
    //{
    //    var dbCon = DBConnection.Instance();
    //    dbCon.DatabaseName = "googleaidb";
    //    if (dbCon.IsConnect())
    //    {
    //        MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = 1 WHERE `username` =?username", dbCon.Connection);
    //        cmd.Parameters.AddWithValue("?username", givenUsername);
    //        int result = Convert.ToInt16(cmd.ExecuteScalar());
    //        dbCon.Close();
    //    }
    //}
    //public void SetOffline(string givenUsername)
    //{
    //    var dbCon = DBConnection.Instance();
    //    dbCon.DatabaseName = "googleaidb";
    //    if (dbCon.IsConnect())
    //    {
    //        MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = 0 WHERE `username` =?username", dbCon.Connection);
    //        cmd.Parameters.AddWithValue("?username", givenUsername);
    //        int result = Convert.ToInt16(cmd.ExecuteScalar());
    //        dbCon.Close();
    //    }
    //}

    public void StartGame()
    {
        string toSend = "startgame/%username=" + PlayerPrefs.GetString("Username") + "%";

        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(Constant.Server), 4343);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(serverAddress);

        // Sending
        int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
        byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
        byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
        clientSocket.Send(toSendLenBytes);
        clientSocket.Send(toSendBytes);

        // Receiving
        byte[] rcvLenBytes = new byte[4];
        clientSocket.Receive(rcvLenBytes);
        int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
        byte[] rcvBytes = new byte[rcvLen];
        clientSocket.Receive(rcvBytes);
        string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

        Debug.Log("Client received in StartGame: " + rcv);
        if (rcv.Contains(" connected."))
        {
            SceneManager.LoadScene("PizzaMakerUI", LoadSceneMode.Single);

        }
        else
        {
            Debug.Log(rcv);
        }
        clientSocket.Close();
    }

    //public void StartGame(string givenUsername)
    //{
    //    string toSend = "startgame/%username=" + givenUsername + "%";

    //    IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(Constant.Server), 4343);

    //    Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //    clientSocket.Connect(serverAddress);

    //    // Sending
    //    int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
    //    byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
    //    byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
    //    clientSocket.Send(toSendLenBytes);
    //    clientSocket.Send(toSendBytes);

    //    // Receiving
    //    byte[] rcvLenBytes = new byte[4];
    //    clientSocket.Receive(rcvLenBytes);
    //    int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
    //    byte[] rcvBytes = new byte[rcvLen];
    //    clientSocket.Receive(rcvBytes);
    //    string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

    //    Debug.Log("Client received: " + rcv);
    //    if (rcv == "Success")
    //    {
    //        SceneManager.LoadScene("GameMenuScene", LoadSceneMode.Single);
    //    }
    //    else
    //    {
    //        Debug.Log(rcv);
    //    }
    //    clientSocket.Close();
    //}
    //public void SetInProgress(string givenUsername)
    //{
    //    var dbCon = DBConnection.Instance();
    //    dbCon.DatabaseName = "googleaidb";
    //    if (dbCon.IsConnect())
    //    {
    //        MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = 3 WHERE `username` =?username", dbCon.Connection);
    //        cmd.Parameters.AddWithValue("?username", usernameInputField.text);
    //        int result = Convert.ToInt16(cmd.ExecuteScalar());
    //        dbCon.Close();
    //    }
    //}

    public void JoinGame()
    {
        string givenUsername = usernameInputField.text;
        string loggedInUsername = PlayerPrefs.GetString("Username");


        string toSend = "joingame/%loggedInUsername=" + loggedInUsername + "%givenUsername=" + givenUsername + "%";

        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(Constant.Server), 4343);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(serverAddress);

        // Sending
        int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
        byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
        byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
        clientSocket.Send(toSendLenBytes);
        clientSocket.Send(toSendBytes);

        // Receiving
        byte[] rcvLenBytes = new byte[4];
        clientSocket.Receive(rcvLenBytes);
        int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
        byte[] rcvBytes = new byte[rcvLen];
        clientSocket.Receive(rcvBytes);
        string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

        Debug.Log("Client received in Join Game: " + rcv);
        if (rcv == "PlayerConnected")
        {
            SceneManager.LoadScene("CustomerUI", LoadSceneMode.Single);

        }
        else
        {
            Debug.Log(rcv);
        }
        clientSocket.Close();
    }

    //0 = Offline
    //1 = Online
    //2 = Game Created but waiting
    //3 = In progress
    private void ChangeStatus(string givenUsername, int givenStatus)
    {
        var dbCon = DBConnection.Instance();
        dbCon.DatabaseName = "googleaidb";
        if (dbCon.IsConnect())
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE `users` SET `online` = ?status WHERE `username` =?username", dbCon.Connection);
            cmd.Parameters.AddWithValue("?status", givenStatus);
            cmd.Parameters.AddWithValue("?username", givenUsername);
            int result = Convert.ToInt16(cmd.ExecuteScalar());
            dbCon.Close();
        }
    }
    //Debug Stuff
    //public void PrintStuff(string printed)
    //{
    //    Debug.Log(emailInputField.text);
    //}

}
