using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayView : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;

    public PlayerModel playerModel;

    void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;

        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
        timerText = GameObject.Find("Timer").GetComponent<TMP_Text>();


        //event subscribe
        PlayerModel.OnDataChanged += UpdateView;

        //init
        UpdateView();
    }

    public void UpdateView()
    {
        scoreText.text = "Score : "+playerModel.Score.ToString();
        timerText.text = "Time  : "+playerModel.Time.ToString()+ "Sec";
    }
}
