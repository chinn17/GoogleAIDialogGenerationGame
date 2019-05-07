using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    ConnectionManager con_man;
    GameObject main;

    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;


    void Awake()
    {
        main = GameObject.Find("MainMenu");

    }

    private void Start()
    {
        con_man = main.GetComponent<ConnectionManager>();
    }

    public void loginUser()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        //string timer = "123";
        //string toSend = "gameTimer/%timer=" + timer;
        //con_man.send(toSend, Constants.response_gametimer, ResponseTimer);
        con_man.send("/login?username=" + username + "&password=" + password, Constants.response_login, ResponseLogin);

    }


    public IEnumerator ResponseLogin(Response response)
    {
       if (response.response == "Success")
        {
            Debug.Log("Login Successful");
            PlayerPrefs.SetString("Username", usernameInputField.text);
            SceneManager.LoadScene("GameMenuScene", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Failed to Login");
        }
        yield return 0;
    }
}
