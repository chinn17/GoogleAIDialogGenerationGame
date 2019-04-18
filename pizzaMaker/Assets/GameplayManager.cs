using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{


    public int toppingLimit = 0;
    public Image newPizzaImage;
    public TextMeshProUGUI newPizzaName;
    public int newPizzaPrice = 0;
    public int pizzaNumber = 0;

    public Button resetButton;

    public int cartTotalNumber = 0;


    public Button topping1;
    public Button topping2;
    public Button topping3;
    public Button topping4;
    public Button topping5;
    public Button topping6;
    public Button topping7;
    public Button topping8;
    public Button topping9;
    public Button topping10;


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

    public TextMeshProUGUI cartItemPrice1;
    public TextMeshProUGUI cartItemPrice2;
    public TextMeshProUGUI cartItemPrice3;
    public TextMeshProUGUI cartItemPrice4;

    public TextMeshProUGUI cartItemName1;
    public TextMeshProUGUI cartItemName2;
    public TextMeshProUGUI cartItemName3;
    public TextMeshProUGUI cartItemName4;


    public TextMeshProUGUI totalPrice;

    public Image cartItem1;
    public Image cartItem2;
    public Image cartItem3;
    public Image cartItem4;


    public Button addButton;

    private void Start()
    {
        addButton.onClick.AddListener(addToCart);
        resetButton.onClick.AddListener(resetPizzaStation);
        cartItemName1.text = "";
        cartItemName2.text = "";
        cartItemName3.text = "";
        cartItemName4.text = "";
        newPizzaName.text = "";

    }

    public void updatePizzaName(string newToppingName)
    {
        toppingLimit++;
        Debug.Log(toppingLimit);


        int[] pizzaList = { 12, 6, 19, 13, 8, 20, 22, 24, 60 };
        int[] pizzaPriceList = { 12, 6, 19, 13, 8, 20, 22, 24, 60 };
        string[] pizzaNameList = {"Pizza 1", "Pizza 2", "Pizza 3", "Pizza 4", "Pizza 5", "Pizza 6", "Pizza 7", "Pizza 8", "Pizza 9", "Pizza 10" };

        Sprite[] pizzaImages = { menuItem1, menuItem2, menuItem3, menuItem4, menuItem5, menuItem6, menuItem7, menuItem8, menuItem9, menuItem10 };


        string[] toppingList = { "", "seaweed", "shrimp", "fish1", "fish2", "fish3", "lobster", "eel", "fish_eggs", "octopus", "squid" };

        for (int i = 0; i < toppingList.Length; i++)
        {
            if (newToppingName == toppingList[i])
            {
                pizzaNumber += i;
            }

        }


        if (toppingLimit == 3)
        {

            for (int i = 0; i < pizzaList.Length; i++)
            {
                if (pizzaNumber == pizzaList[i])
                {
                    newPizzaImage.sprite = pizzaImages[i];
                    newPizzaPrice = pizzaPriceList[i];
                    newPizzaName.text = pizzaNameList[i];
                }
            }

        }


    }


    public void addToCart()
    {
    
        cartTotalNumber += newPizzaPrice;
        
        if (cartItem1.sprite.name == "empty_pizza")
        {
            cartItem1.sprite = newPizzaImage.sprite;
            cartItemPrice1.text = newPizzaPrice.ToString();
            cartItemName1.text = newPizzaName.text;
            resetPizzaStation();
        }
        else if (cartItem2.sprite.name == "empty_pizza")
        {
            cartItem2.sprite = newPizzaImage.sprite;
            cartItemPrice2.text = newPizzaPrice.ToString();
            cartItemName2.text = newPizzaName.text;
            resetPizzaStation();
        }
        else if (cartItem3.sprite.name == "empty_pizza")
        {
            cartItem3.sprite = newPizzaImage.sprite;
            cartItemPrice3.text = newPizzaPrice.ToString();
            cartItemName3.text = newPizzaName.text;
            resetPizzaStation();
        }
        else if (cartItem4.sprite.name == "empty_pizza")
        {
            cartItem4.sprite = newPizzaImage.sprite;
            cartItemPrice4.text = newPizzaPrice.ToString();
            cartItemName4.text = newPizzaName.text;
            resetPizzaStation();
        }


        toppingLimit = 0;
        pizzaNumber = 0;
        totalPrice.text = cartTotalNumber.ToString();

    }


    void resetPizzaStation()
    {
        Button[] toppingButtons = { topping1, topping2, topping3, topping4, topping5, topping6, topping7, topping8, topping9, topping10 };

        for (int i = 0; i< toppingButtons.Length; i++)
        {
            toppingButtons[i].image.color = Color.white;
        }

        newPizzaImage.sprite = emptyPizzaImage;
        newPizzaName.text = "";
        newPizzaPrice = 0;
        toppingLimit = 0;
        pizzaNumber = 0;

    }


}
