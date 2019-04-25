using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class VerifyingConnection : MonoBehaviour

    //attached to join button in Join submenu
{
    //public var Client = SocketClient();


    //1.encapsulating field and using property- will update usages of that field to use the new properties that it creates

    //2.encapsulate field but still use field-doesnt change existing usages of the field elsewhere in the code
    //2nd option picked

   public void testingScriptAccess()
    {

        Console.WriteLine("client has invoked the verifyingConnection class");
    }


    // Start is called before the first frame update
    void Start()
    {
        var dbCon = DBConnection.Instance();

        //start game button connects first, waits for player2
        var dbP = new DatabaseProtocol();

        var clientConnection=0;
        var listenerConnection = 0;

        System.Random random = new System.Random();

        int firstPlayer = random.Next(1,3);
        int secondPlayer = random.Next(1, 3);

        

        if (dbCon.IsConnect() == true)//DatabaseConnection true
        {

            if (dbP)//if dbP.emailInputField is true, then 
            {

            }
            if (firstPlayer == 1 && secondPlayer != 1)//they go first
            {
                SceneManager.LoadScene(sceneName: "PizzaMakerUI");
            }
            else {
                SceneManager.LoadScene(sceneName: "CustomerUI");
                }
            dbCon.Close();
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
