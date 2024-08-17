using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerModel
{
    private bool isPlay;
    private string userId;
    private int time;
    private int score;


    //
    private Sprite nextBlockImg;

    private string gameSceneImg;

    public static event Action OnPlayerDataChanged;


    public PlayerModel()
    {
        isPlay = true;
        userId = Environment.UserName;
        time = 100;
        score = 0;

        nextBlockImg = null;

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
                OnPlayerDataChanged?.Invoke(); // 데이터 변경을 알림
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
                OnPlayerDataChanged?.Invoke();
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
                OnPlayerDataChanged?.Invoke();
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
                OnPlayerDataChanged?.Invoke();
            }
        }
    }
    public Sprite NextBlockImg
    {
        get => nextBlockImg;
        set
        {
            if (nextBlockImg != value)
            {
                nextBlockImg = value;
                OnPlayerDataChanged?.Invoke(); 
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
                OnPlayerDataChanged?.Invoke(); 
            }
        }
    }

    //funt
    public void AddScore(int points)
    {
        Score += points;
    }

    public void DecreaseTime()
    {
        Time -= 1;
    }

    public void ChangeNextBlockImg(Sprite sprite)
    {
        NextBlockImg = sprite;
    }
}
