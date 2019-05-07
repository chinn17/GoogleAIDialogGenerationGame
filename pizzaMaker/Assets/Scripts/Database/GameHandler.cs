using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    ConnectionManager con_man;
    GameObject main;

    public string playerId;
    public GameObject chatPanel, textObject;
    public TMP_InputField messageField;

    public ProgressBar profitBar;

    public Button sendMessageButton;
    public Button addItemButton1;
    public Button addItemButton2;
    public Button addItemButton3;
    public Button addItemButton4;
    public Button addItemButton5;
    public Button addItemButton6;
    public Button addItemButton7;
    public Button addItemButton8;
    public Button addItemButton9;
    public Button addItemButton10;

    List<Message> chatMessages = new List<Message>();
    int maxMessages = 25;
   


    int frame_counter = 0;

    void Awake()
    {
        main = GameObject.Find("PizzaMakerUI");

    }

    private void Start()
    {
        con_man = main.GetComponent<ConnectionManager>();
        sendMessageButton.onClick.AddListener(sendMessage);
        addItemButton1.onClick.AddListener(delegate { addToCart("pizza1"); });
        addItemButton2.onClick.AddListener(delegate { addToCart("pizza2"); });
        addItemButton3.onClick.AddListener(delegate { addToCart("pizza3"); });
        addItemButton4.onClick.AddListener(delegate { addToCart("pizza4"); });
        addItemButton5.onClick.AddListener(delegate { addToCart("pizza5"); });
        addItemButton6.onClick.AddListener(delegate { addToCart("pizza6"); });
        addItemButton7.onClick.AddListener(delegate { addToCart("pizza7"); });
        addItemButton8.onClick.AddListener(delegate { addToCart("pizza8"); });
        addItemButton9.onClick.AddListener(delegate { addToCart("pizza9"); });
        addItemButton10.onClick.AddListener(delegate { addToCart("pizza10"); });
    }

    private void Update()
    {

        if (frame_counter % 24 == 0)
        {
            requestHeartbeat();
        }
        frame_counter++;


    }

    public void requestHeartbeat()
    {
        string playerId = "player1";
        con_man.send("/heartbeat?playerId=" + playerId, Constants.response_heartbeat, ResponseHeartbeat);
    }


    public IEnumerator ResponseHeartbeat(Response response)
    {
    
        string protocol = response.response.Substring(0, 3);
        response.response = response.response.Substring(3);

        if (protocol == "103")
        {
            addMessageToChatbox(response.response);
        }


        if (protocol == "106")
        {
            updateScore();
        }
        yield return 0;
    }

    public void updateScore()
    {
        //Calculate score
        profitBar.BarValue += 10;
    }

    public void sendMessage()
    {
        if (messageField.text != "")
        {
            if (chatMessages.Count >= maxMessages)
            {
                Destroy(chatMessages[0].textObject.gameObject);
                chatMessages.Remove(chatMessages[0]);
            }

            addMessageToChatbox(messageField.text);


            con_man.send("/chat?newMessage=" + messageField.text + "&playerId=player1", Constants.response_chat, ResponseChat);
            messageField.text = "";
        }
    }

    public void addToCart(string itemName)
    {
        con_man.send("/addToCart?itemName=" + itemName, Constants.response_addToCart, ResponseCart);
    }


    public void addMessageToChatbox(string messageText)
    {
        Message newMessage = new Message();
        newMessage.text = messageText;
        GameObject newTextObject = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newTextObject.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;

        chatMessages.Add(newMessage);
    }


    public void getGameTimer()
    {

        if (frame_counter % 24 == 0)
        {

            con_man.send("/gameTimer", Constants.response_gametimer, ResponseTimer);
        }

    }

    public IEnumerator ResponseTimer(Response response)
    {
        Debug.Log("Timer Response");

        yield return 0;
    }



    public IEnumerator ResponseChat(Response response)
    {
        //Debug.Log("Message Sent Successfully");

        yield return 0;
    }

    public IEnumerator ResponseCart(Response response)
    {
       // Debug.Log("Cart Response");

        yield return 0;
    }

}
