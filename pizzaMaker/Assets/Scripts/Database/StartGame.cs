using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    ConnectionManager con_man;
    GameObject main;

    public TMP_InputField usernameInputField;

    void Awake()
    {
        main = GameObject.Find("GameMenu");

    }

    private void Start()
    {
        con_man = main.GetComponent<ConnectionManager>();
    }

    public void startGame()
    {

        con_man.send("/startGame?username=" + PlayerPrefs.GetString("Username"), Constants.response_startGame, ResponseStartGame);

    }


    public IEnumerator ResponseStartGame(Response response)
    {
        Debug.Log("Start Game Response :" + response.response);

        if (response.response == "User connected")
        {
            SceneManager.LoadScene("PizzaMakerUI", LoadSceneMode.Single);

        }
        yield return 0;
    }

    public void joinGame()
    {
        string givenUsername = usernameInputField.text;
        string loggedInUsername = PlayerPrefs.GetString("Username");

        con_man.send("/joinGame?loggedInUsername=" + loggedInUsername + "&givenUsername=" + givenUsername, Constants.response_joinGame, ResponseJoinGame);

    }


    public IEnumerator ResponseJoinGame(Response response)
    {
        Debug.Log("Join Game Response :" + response.response);
        if (response.response == "PlayerConnected")
        {
            SceneManager.LoadScene("CustomerUI", LoadSceneMode.Single);

        }
        yield return 0;
    }

}
