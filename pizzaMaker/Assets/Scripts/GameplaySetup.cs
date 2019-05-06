using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySetup : MonoBehaviour
{
    GameObject cart;
    GameObject menu;

    bool status = true;

    void Awake()
    {
        cart = GameObject.Find("Cart");
        menu = GameObject.Find("Menu");
    }

    void Start()
    {
        menu.SetActive(status);
        cart.SetActive(!status);
    }

    public void FlipStatus()
    {
        status = !status;
        Debug.Log(status);
        menu.SetActive(status);
        cart.SetActive(!status);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
