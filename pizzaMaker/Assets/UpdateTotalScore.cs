using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTotalScore : MonoBehaviour
{
    public TextMeshProUGUI happinessScoreLabel;
    public TextMeshProUGUI savingsScoreLabel;
    public TextMeshProUGUI totalScoreLabel;
    public TextMeshProUGUI profitScoreLabel;
    public TextMeshProUGUI totalTeamScoreLabel;


    public static int happinessScore;
    public static int savingsScore;
    public static int totalScore;
    public static int profitScore;
    public static int totalTeamScore;
    // Start is called before the first frame update
    void Start()
    {
        happinessScoreLabel.text = happinessScore.ToString();
        savingsScoreLabel.text = savingsScore.ToString();
        totalScoreLabel.text = (happinessScore + savingsScore).ToString();
        profitScoreLabel.text = profitScore.ToString();
        totalTeamScoreLabel.text = (profitScore + happinessScore + savingsScore).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
