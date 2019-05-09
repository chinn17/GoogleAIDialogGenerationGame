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


    public Sprite menuItem1;
    public Sprite menuItem2;
    public Sprite menuItem3;
    public Sprite menuItem4;
    public Sprite menuItem5;
    public Sprite menuItem6;
    public Sprite menuItem7;
    public Sprite menuItem8;
    public Sprite menuItem9;
    public Sprite menuItem10;
    public Sprite emptyPizzaImage;

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

    public Image cartItemImage1;
    public Image cartItemImage2;
    public Image cartItemImage3;
    public Image cartItemImage4;
    public Image cartItemImage5;
    public Image cartItemImage6;
    public Image cartItemImage7;
    public Image cartItemImage8;
    public Image cartItemImage9;
    public Image cartItemImage10;

    public TextMeshProUGUI cartItemPrice1;
    public TextMeshProUGUI cartItemPrice2;
    public TextMeshProUGUI cartItemPrice3;
    public TextMeshProUGUI cartItemPrice4;
    public TextMeshProUGUI cartItemPrice5;
    public TextMeshProUGUI cartItemPrice6;
    public TextMeshProUGUI cartItemPrice7;
    public TextMeshProUGUI cartItemPrice8;
    public TextMeshProUGUI cartItemPrice9;
    public TextMeshProUGUI cartItemPrice10;

    public TextMeshProUGUI cartItemName1;
    public TextMeshProUGUI cartItemName2;
    public TextMeshProUGUI cartItemName3;
    public TextMeshProUGUI cartItemName4;
    public TextMeshProUGUI cartItemName5;

    public TextMeshProUGUI totalPriceLabel;

    public static int cartTotalNumber = 0;


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
        addItemButton1.onClick.AddListener(delegate { addToCustomerCart("pizza1"); });
        addItemButton2.onClick.AddListener(delegate { addToCustomerCart("pizza2"); });
        addItemButton3.onClick.AddListener(delegate { addToCustomerCart("pizza3"); });
        addItemButton4.onClick.AddListener(delegate { addToCustomerCart("pizza4"); });
        addItemButton5.onClick.AddListener(delegate { addToCustomerCart("pizza5"); });
        addItemButton6.onClick.AddListener(delegate { addToCustomerCart("pizza6"); });
        addItemButton7.onClick.AddListener(delegate { addToCustomerCart("pizza7"); });
        addItemButton8.onClick.AddListener(delegate { addToCustomerCart("pizza8"); });
        addItemButton9.onClick.AddListener(delegate { addToCustomerCart("pizza9"); });
        addItemButton10.onClick.AddListener(delegate { addToCustomerCart("pizza10"); });
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

        clearCart();
    }

    public void clearCart()
    {
        cartItemName1.text = "";
        cartItemName2.text = "";
        cartItemName3.text = "";
        cartItemName4.text = "";

        cartItemImage1.sprite = emptyPizzaImage;
        cartItemImage2.sprite = emptyPizzaImage;
        cartItemImage3.sprite = emptyPizzaImage;
        cartItemImage4.sprite = emptyPizzaImage;

        cartItemPrice1.text = "0";
        cartItemPrice2.text = "0";
        cartItemPrice3.text = "0";
        cartItemPrice4.text = "0";

        totalPriceLabel.text = "0";
        cartTotalNumber = 0;

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

    public void addToCustomerCart(string itemName)
    {
     
        con_man.send("/addToCart?itemName=" + itemName, Constants.response_addToCart, ResponseCart);
        addToMakerCart(itemName);
    }


    public void addToMakerCart(string itemName)
    {

        int pizzaNumber = 0;

        string[] pizzaNameList = { "pizza1", "pizza2", "pizza3", "pizza4", "pizza5", "pizza6", "pizza7", "pizza8", "pizza9", "pizza10" };
        int[] pizzaPriceList = { 12, 6, 19, 13, 8, 20, 22, 24, 60 };
        Sprite[] pizzaImages = { menuItem1, menuItem2, menuItem3, menuItem4, menuItem5, menuItem6, menuItem7, menuItem8, menuItem9, menuItem10 };


        for (int i = 0; i < pizzaNameList.Length; i++)
        {
            if (itemName == pizzaNameList[i])
            {
                pizzaNumber = i;
                break;
            }
        }


        if (cartItemImage1.sprite.name == "empty_pizza")
        {
            cartItemImage1.sprite = pizzaImages[pizzaNumber];
            cartItemPrice1.text = pizzaPriceList[pizzaNumber].ToString();
          //  cartItemName1.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];

        }
        else if (cartItemImage2.sprite.name == "empty_pizza")
        {
            cartItemImage2.sprite = pizzaImages[pizzaNumber];
            cartItemPrice2.text = pizzaPriceList[pizzaNumber].ToString();
          //  cartItemName2.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];

        }
        else if (cartItemImage3.sprite.name == "empty_pizza")
        {
            cartItemImage3.sprite = pizzaImages[pizzaNumber];
            cartItemPrice3.text = pizzaPriceList[pizzaNumber].ToString();
         //   cartItemName3.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];
        }
        else if (cartItemImage4.sprite.name == "empty_pizza")
        {
            cartItemImage4.sprite = pizzaImages[pizzaNumber];
            cartItemPrice4.text = pizzaPriceList[pizzaNumber].ToString();
         //   cartItemName4.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];
        }

        totalPriceLabel.text = cartTotalNumber.ToString();

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
