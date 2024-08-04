using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerModel
{
    private bool isEnd;
    private string userId;
    private int time;
    private int score;


    //
    private Sprite nextBlockImg;

    public static event Action OnDataChanged;


    public PlayerModel()
    {
        isEnd = true;
        userId = Environment.UserName;
        time = 150;
        score = 0;

        nextBlockImg = null;
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
                OnDataChanged?.Invoke(); // 데이터 변경을 알림
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
                OnDataChanged?.Invoke();
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
                OnDataChanged?.Invoke();
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
                OnDataChanged?.Invoke(); 
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
