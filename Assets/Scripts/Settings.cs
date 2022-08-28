using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static Settings Instance;
    public bool IsGameOn;
    public int HighScore
    {
        get { return PlayerPrefs.GetInt(_highScoreKey, 0); }
        set { PlayerPrefs.SetInt(_highScoreKey, value); }
    }

    private string _highScoreKey = "HighScoreKey";


    public void StartGame() => IsGameOn = true;
    public void FinishGame() => IsGameOn = false;
    public Settings() => Instance = this;
    ~Settings() => Instance = null;




}
