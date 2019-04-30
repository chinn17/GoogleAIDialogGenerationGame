using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerGameplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalText;
    GameObject cartItem;
    string score;

    // Start is called before the first frame update
    void Awake()
    {
        cartItem = GameObject.Find("Cart Item 1");
        score = totalText.text;
    }

    private void Start()
    {
    }

    public void UpdateScore()
    {
        scoreText.text = "Score : " + score;
        cartItem.SetActive(false);
        totalText.text = "0";
        Debug.Log(score);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
