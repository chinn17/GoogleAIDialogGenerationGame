using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Discount : MonoBehaviour
{

    public TextMeshProUGUI totalPriceLabel;
    public TextMeshProUGUI cartItemPriceLabel;
    public string cartItemNumber;
    public int discountPercentage;



    ConnectionManager con_man;
    GameObject main;

    void Awake()
    {
        main = GameObject.Find("PizzaMakerUI");

    }

    // Start is called before the first frame update
    void Start()
    {
        con_man = main.GetComponent<ConnectionManager>();
    }



    public void applyDiscount()
    {
        con_man.send("/discount?discountAmount="+ discountPercentage + "&itemNumber="+cartItemNumber, Constants.response_discount, ResponseDiscount);
        int difference = 0;


            int priceOfItem = Int32.Parse(cartItemPriceLabel.text);

            cartItemPriceLabel.text = (priceOfItem - ((priceOfItem * discountPercentage) / 100)).ToString();
            difference = ((priceOfItem * discountPercentage) / 100);





        GameHandler.cartTotalNumber = GameHandler.cartTotalNumber - difference;
        totalPriceLabel.text = GameHandler.cartTotalNumber.ToString();

    }

    public IEnumerator ResponseDiscount(Response response)
    {
        Debug.Log("Response Discount");

        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
