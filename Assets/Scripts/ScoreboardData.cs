using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreboardData
{
    public int[] scoreboard;

    public ScoreboardData (GameManager gameManager)
    {
        scoreboard = new int[gameManager.scoreboardSize];

        for (int i = 0; i < gameManager.scoreboardSize; i++)
        {
            scoreboard[i] = GameManager.scoreboard[i];
        }

    }
}
