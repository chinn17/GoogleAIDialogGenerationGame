using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySetup : MonoBehaviour
{
    GameObject cart;
    GameObject menu;
    GameObject errorMessage;
    GameObject hint;

    bool status = true;

    void Awake()
    {
        cart = GameObject.Find("Cart");
        menu = GameObject.Find("Menu");
        errorMessage = GameObject.Find("ErrorMessage");
        hint = GameObject.Find("Hint");
    }

    void Start()
    {
        menu.SetActive(status);
        cart.SetActive(!status);
        errorMessage.SetActive(false);
        hint.SetActive(false);
    }

    public void FlipStatus()
    {
        status = !status;
        Debug.Log(status);
        menu.SetActive(status);
        cart.SetActive(!status);
    }

    public void CloseErrorMessage()
    {
        errorMessage.SetActive(false);
    }

    public void ShowHint()
    {
        hint.SetActive(true);
    }

    public void CloseHint()
    {
        hint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
