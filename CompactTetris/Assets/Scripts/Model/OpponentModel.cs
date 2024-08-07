using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OpponentModel : MonoBehaviour
{
    private bool isEnd;
    private string userId;
    private int time;
    private int score;

    //
    public static event Action OnOpponentDataChanged;


    public OpponentModel()
    {
        isEnd = false;
        userId = "Unknown";
        time = 0;
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
                OnOpponentDataChanged?.Invoke(); // 데이터 변경을 알림
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
    public bool IsEnd
    {
        get => isEnd;
        set
        {
            if(isEnd != value)
            {
                isEnd = value;
                OnOpponentDataChanged?.Invoke();
            }
        }
    }
}
