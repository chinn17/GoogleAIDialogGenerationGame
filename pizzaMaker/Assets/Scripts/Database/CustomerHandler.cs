using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerHandler : MonoBehaviour
{
    ConnectionManager con_man;
    GameObject main;

    GameObject errorMessage;

    public string playerId;
    public GameObject chatPanel, textObject;
    public TMP_InputField messageField;

    public Button sendMessageButton;
    public Button payButton;

    public ProgressBar happinessBar;
    public ProgressBar savingsBar;


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
    public TextMeshProUGUI cartItemName6;
    public TextMeshProUGUI cartItemName7;
    public TextMeshProUGUI cartItemName8;
    public TextMeshProUGUI cartItemName9;
    public TextMeshProUGUI cartItemName10;

    public TextMeshProUGUI totalPriceLabel;
    public TextMeshProUGUI budgetLabel;

    int cartTotalNumber = 0;

    int[] cartHappinessScores = { 0, 0, 0, 0 };
    int[] pizzaHappinessScores = { 2, 0, 10, 5, 3, 25, 20, 10, 10, 2 };


    List<Message> chatMessages = new List<Message>();
    int maxMessages = 25;



    int frame_counter = 0;

    void Awake()
    {
        main = GameObject.Find("CustomerUI");
        errorMessage = GameObject.Find("ErrorMessage");

    }

    private void Start()
    {
        con_man = main.GetComponent<ConnectionManager>();
        sendMessageButton.onClick.AddListener(sendMessage);
        payButton.onClick.AddListener(payPressed);
        happinessBar.BarValue = 0;
        savingsBar.BarValue = 100;
        cartItemName1.text = "";
        cartItemName2.text = "";
        cartItemName3.text = "";
        cartItemName4.text = "";
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
        string playerId = this.playerId;
        con_man.send("/heartbeat?playerId=" + playerId, Constants.response_heartbeat, ResponseHeartbeat);
    }


    public IEnumerator ResponseHeartbeat(Response response)
    {
        string protocol = response.response.Substring(0,3);
        response.response = response.response.Substring(3);


        if (protocol == "104")
        {
            //Add To Customer's Cart
            addItemToCart(response.response);

        }


        if (protocol == "103")
        {
            addResponseMessageToChatbox(response.response);
        }

        if (protocol == "107")
        {

            Debug.Log("APPLYING DISCOUNT HERE : "+ response.response);
            int discountPercentage = Int32.Parse(response.response.Substring(0, 2));
            string cartItemNumber = response.response.Substring(2);
            int difference = 0;

            if (cartItemNumber == "1")
            {
                int priceOfItem = Int32.Parse(cartItemPrice1.text);

                cartItemPrice1.text = (priceOfItem - ((priceOfItem * discountPercentage) / 100)).ToString();
                difference = ((priceOfItem * discountPercentage) / 100);


            }
           else if (cartItemNumber == "2")
            {
                int priceOfItem = Int32.Parse(cartItemPrice1.text);

                cartItemPrice2.text = (priceOfItem - ((priceOfItem * discountPercentage) / 100)).ToString();
                difference = ((priceOfItem * discountPercentage) / 100);


            }
            else if (cartItemNumber == "3")
            {
                int priceOfItem = Int32.Parse(cartItemPrice1.text);

                cartItemPrice3.text = (priceOfItem - ((priceOfItem * discountPercentage) / 100)).ToString();
                difference = ((priceOfItem * discountPercentage) / 100);

            }
            else if (cartItemNumber == "4")
            {
                int priceOfItem = Int32.Parse(cartItemPrice1.text);

                cartItemPrice4.text = (priceOfItem - ((priceOfItem * discountPercentage) / 100)).ToString();
                difference = ((priceOfItem * discountPercentage) / 100);

            }
            else if (cartItemNumber == "5")
            {
                int priceOfItem = Int32.Parse(cartItemPrice1.text);

                cartItemPrice5.text = (priceOfItem - ((priceOfItem * discountPercentage) / 100)).ToString();
                difference = ((priceOfItem * discountPercentage) / 100);

            }


            cartTotalNumber = cartTotalNumber - difference;
            totalPriceLabel.text = cartTotalNumber.ToString();

        }


        yield return 0;
    }


    public void addItemToCart(string itemName)
    {

        int pizzaNumber = 0;

        string[] pizzaNameList = { "pizza1", "pizza2", "pizza3", "pizza4", "pizza5", "pizza6", "pizza7", "pizza8", "pizza9", "pizza10" };
        int[] pizzaPriceList = { 13, 15, 10, 12, 11, 13, 14, 12, 14, 13 };
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
            cartItemName1.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];
            cartHappinessScores[0] = pizzaHappinessScores[pizzaNumber];

        }
        else if (cartItemImage2.sprite.name == "empty_pizza")
        {
            cartItemImage2.sprite = pizzaImages[pizzaNumber];
            cartItemPrice2.text = pizzaPriceList[pizzaNumber].ToString();
            cartItemName2.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];
            cartHappinessScores[1] = pizzaHappinessScores[pizzaNumber];

        }
        else if (cartItemImage3.sprite.name == "empty_pizza")
        {
            cartItemImage3.sprite = pizzaImages[pizzaNumber];
            cartItemPrice3.text = pizzaPriceList[pizzaNumber].ToString();
            cartItemName3.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];
            cartHappinessScores[2] = pizzaHappinessScores[pizzaNumber];
        }
        else if (cartItemImage4.sprite.name == "empty_pizza")
        {
            cartItemImage4.sprite = pizzaImages[pizzaNumber];
            cartItemPrice4.text = pizzaPriceList[pizzaNumber].ToString();
            cartItemName4.text = itemName;
            cartTotalNumber += pizzaPriceList[pizzaNumber];
            cartHappinessScores[3] = pizzaHappinessScores[pizzaNumber];
        }

        totalPriceLabel.text = cartTotalNumber.ToString();

    }

    private string[] WhiteList = { "PIZZA", "THANK", "YOU", "PURCHASE", "BUY", "PRICE", "BUDGET", "HELLO", "HI", "I'M", "LOOKING", "PLEASE", "DOLLAR", "COIN", "SECOND", "YES", "YEAH", "NO", "NOPE", "OKAY", "OK", "SUGGEST", "ADD", "TAKE", "OFF", "DISCOUNT", "DON'T" };
    private string[] Punctuation = { ".", "!", ",", "?", ":", ";", "&" };


    public void sendMessage()
    {
        if (messageField.text != "")
        {
            if (WhiteList.Any(messageField.text.ToUpper().Contains))
            {
                if (Punctuation.Any(messageField.text.EndsWith))
                {
                    if (chatMessages.Count >= maxMessages)
                    {
                        Destroy(chatMessages[0].textObject.gameObject);
                        chatMessages.Remove(chatMessages[0]);
                    }

                    addMessageToChatbox(messageField.text);


                    con_man.send("/chat?newMessage=" + messageField.text + "&playerId=" + this.playerId, Constants.response_chat, ResponseChat);
                    messageField.text = "";
                }
                else
                {
                    ShowErrorMessage();
                    Debug.Log("Don't forget to punctuate!");
                }

            }
            else
            {
                ShowErrorMessage();
                Debug.Log("Please use complete sentences.");
            }
        }
    }

    public void ShowErrorMessage()
    {
        errorMessage.SetActive(true);
    }


    public void addMessageToChatbox(string messageText)
    {
        Message newMessage = new Message();
        newMessage.text = messageText;
        GameObject newTextObject = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newTextObject.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = Color.black;

        chatMessages.Add(newMessage);
    }

    public void addResponseMessageToChatbox(string messageText)
    {
        Message newMessage = new Message();
        newMessage.text = messageText;
        GameObject newTextObject = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newTextObject.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = Color.white;

        chatMessages.Add(newMessage);
    }

    public void payPressed()
    {

        //Send request to Maker
        con_man.send("/pay", Constants.response_pay, ResponsePay);


        //Calculate scores
        foreach (int itemScore in cartHappinessScores) 
        {
            happinessBar.BarValue += itemScore;
        }


        int totalAmount = Int32.Parse(totalPriceLabel.text);
        savingsBar.BarValue -= totalAmount;




        //Reduce from budget
        int budget = Int32.Parse(budgetLabel.text);
        budget -= totalAmount;
        budgetLabel.text = budget.ToString();

        //Clear cart




        clearCart();

        UpdateTotalScore.happinessScore = (int)happinessBar.BarValue;
        UpdateTotalScore.savingsScore = (int)savingsBar.BarValue;
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

        for (int i = 0; i < 4; i++) {
            cartHappinessScores[i] = 0;
        }
    }

    public IEnumerator ResponsePay(Response response)
    {


        yield return 0;
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

}

[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
}
