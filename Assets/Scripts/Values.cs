using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values
{
    public static Values Instance;
    public bool IsGameOn;
    public int HighScore
    {
        get { return PlayerPrefs.GetInt(_highScoreKey, 0); }
        set
        {
            if (PlayerPrefs.GetInt(_highScoreKey) < value)
                PlayerPrefs.SetInt(_highScoreKey, value);
        }
    }

    private const string _highScoreKey = "HighScoreKey";


    public void StartGame() => IsGameOn = true;
    public void FinishGame() => IsGameOn = false;
    public Values() => Instance = this;
    ~Values() => Instance = null;




}
