using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score, missed;

    private void OnEnable()
    {
        BabyScript.OnSuccess += AddToScore;
        BabyScript.OnFailure += AddToMissed;

        GameManager.OnStarted += ResetScores;
        GameManager.OnComplete += UpdateUI;
    }

    private void OnDisable()
    {
        BabyScript.OnSuccess -= AddToScore;
        BabyScript.OnFailure -= AddToMissed;
    }

    private void AddToScore()
    {
        score++;
    }

    private void AddToMissed()
    {
        missed++;
    }

    private void UpdateUI()
    {
        //update score UI on game complete screen
    }
    private void ResetScores()
    {

    }

    //update UI in here
}
