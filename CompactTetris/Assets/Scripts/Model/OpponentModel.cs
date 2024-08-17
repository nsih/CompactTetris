using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class OpponentModel
{
    private bool isPlay;
    private string userId;
    private int time;
    private int score;

    private string gameSceneImg;

    //
    public static event Action OnOpponentDataChanged;


    public OpponentModel()
    {
        isPlay = true;
        userId = null;
        time = 0;
        score = 0;
        gameSceneImg = null;
    }

    //define
    public String UserId
    {
        get => userId;
        set
        {
            if (userId != value)
            {
                userId = value;
                OnOpponentDataChanged?.Invoke();
            }
        }
    }
    public int Score
    {
        get => score;
        set
        {
            if (score != value)
            {
                score = value;
                OnOpponentDataChanged?.Invoke();
            }
        }
    }
    public int Time
    {
        get => time;
        set
        {
            if(time != value)
            {
                time = value;
                OnOpponentDataChanged?.Invoke();
            }
        }
    }
    public bool IsPlay
    {
        get => isPlay;
        set
        {
            if(isPlay != value)
            {
                isPlay = value;
                OnOpponentDataChanged?.Invoke();
            }
        }
    }

    public string GameSceneImg
    {
        get => gameSceneImg;
        set
        {
            if (gameSceneImg != value)
            {
                gameSceneImg = value;
                OnOpponentDataChanged?.Invoke(); 
            }
        }
    }
}
