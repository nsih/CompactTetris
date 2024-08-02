using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerModel
{
    private bool isEnd;
    private string userId;
    private int time;
    private int score;

    public static event Action OnDataChanged;


    public PlayerModel()
    {
        isEnd = true;
        userId = Environment.UserName;
        time = 100;
        score = 0;
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

    //funt
    public void AddScore(int points)
    {
        Score += points;
    }

    public void DecreaseTime()
    {
        Time -= 1;
    }
}
