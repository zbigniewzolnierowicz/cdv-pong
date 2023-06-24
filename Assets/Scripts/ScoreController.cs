using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    public int leftScore;
    public int rightScore;

    private void Start()
    {
        leftScore = 0;
        rightScore = 0;
    }

    public void OnLeftScore()
    {
        leftScore += 1;
        leftScoreText.text = leftScore.ToString();
    }

    public void OnRightScore()
    {
        rightScore += 1;
        rightScoreText.text = rightScore.ToString();
    }
}
