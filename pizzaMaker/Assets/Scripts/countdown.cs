using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class countdown : MonoBehaviour
{
    public TextMeshProUGUI timer;
    float totalTime = 120f; //2 minutes
    // Start is called before the first frame update
    bool firstGame = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        totalTime -= Time.deltaTime;
        if (totalTime > 0)
        {
            UpdateLevelTimer(totalTime);
        }
        else if (firstGame)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "PizzaMakerUI")
            {
                SceneManager.LoadScene("CustomerUI", LoadSceneMode.Single);
                firstGame = false;

            }
            else if (currentScene == "CustomerUI")
            {
                SceneManager.LoadScene("PizzaMakerUI", LoadSceneMode.Single);
                firstGame = false;

            }
        }
        else
        {
            SceneManager.LoadScene("PlayerWonScene", LoadSceneMode.Single);

        }


    }

    public void UpdateLevelTimer(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.RoundToInt(totalSeconds % 60f);

        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
