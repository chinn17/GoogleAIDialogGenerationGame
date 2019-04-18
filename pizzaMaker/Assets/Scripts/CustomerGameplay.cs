using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerGameplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI totalText;
    string score;

    // Start is called before the first frame update
    void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        score = "0";
    }

    private void Start()
    {
    }

    public void GetTotal()
    {
        totalText = GetComponent<TextMeshProUGUI>();
        score = totalText.text;
        Debug.Log(score);
    }

    public void UpdateScore()
    {
        //scoreText.text = "Score : " + score;
        //Debug.Log(score);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
