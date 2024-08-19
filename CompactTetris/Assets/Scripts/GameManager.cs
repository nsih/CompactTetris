using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerModel playerModel;
    public PlayerModel PlayerModel
    {
        get
        {
            if (playerModel == null)
            {
                Debug.LogError("PlayerModel instance not exist");
            }
            return playerModel;
        }
    }

    private OpponentModel opponentModel;
    public OpponentModel OpponentModel
    {
        get
        {
            if (playerModel == null)
            {
                Debug.LogError("PlayerModel instance not exist");
            }
            return opponentModel;
        }
    }

    private NetworkStateModel networkStateModel;
    public NetworkStateModel NetworkStateModel
    {
        get
        {
            if (networkStateModel == null) 
            {
                Debug.LogError("Network State Model instance not exist");
            }
            return networkStateModel;
        }
    }

    //
    public GameObject grid;
    public GameObject blockHolder;

    // 
    public GameObject resultUI;
    public TMP_Text wolResult;
    public TMP_Text scoreResult;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerModel = new PlayerModel();
            opponentModel = new OpponentModel();
            networkStateModel = new NetworkStateModel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        resultUI = GameObject.Find("Result");
        wolResult = GameObject.Find("WoLResult").GetComponent<TMP_Text>();
        scoreResult = GameObject.Find("ScoreResult").GetComponent<TMP_Text>();

        resultUI.SetActive(false);

        grid = GameObject.Find("Grid");
        blockHolder = GameObject.Find("BlockHolder");

        //PlayerModel.OnPlayerDataChanged += GameOver;
        PlayerModel.OnPlayerDataChanged += Timer;

        playerModel.IsPlay = false;
        Time.timeScale = 0f;

        //Debug.Log(networkStateModel.NetworkState);
    }

    public void SwitchTime(bool _switch)
    {
        if (_switch)
        {
            Time.timeScale = 1;
            StartCoroutine(TimeControlCoroutine());

            Debug.Log("Time Switch On");
        }
            
        else
        {
            Time.timeScale = 0;
            StopCoroutine(TimeControlCoroutine());

            Debug.Log("Time Switch Off");
        }
    }

    public void Timer()
    {
        if (playerModel.Time <= 0 )
        {
            playerModel.IsPlay = false;
        }
    }

    // 코루틴으로 시간 감소
    private IEnumerator TimeControlCoroutine()
    {
        playerModel.IsPlay = true;

        while (Time.timeScale > 0 && playerModel.IsPlay)
        {
            yield return new WaitForSeconds(2f); // 1초 대기
            PlayerModel.DecreaseTime();

            if (opponentModel.UserId != null)
            {
                gameObject.GetComponent<ScreenshotManager>().DoCapture();
                
                gameObject.GetComponent<NetworkManager>().POSTReq();

                gameObject.GetComponent<NetworkManager>().GETReq();
            }
        }

        playerModel.IsPlay = false;
        GameOver();
    }

    public void GameOver()
    {
        if(!playerModel.IsPlay)
        {
            blockHolder.GetComponent<BlockCon>().ClearGrid();
            grid.SetActive(false);
            
            SwitchTime(false);

            resultUI.SetActive(true);

            if(opponentModel.UserId != null)
            {
                wolResult.gameObject.SetActive(true);
                if(playerModel.Score > opponentModel.Score)
                {
                    wolResult.text = "You Win!!";
                }
                else if (playerModel.Score == opponentModel.Score)
                {
                    wolResult.text = "Draw";
                }
                else
                {
                    wolResult.text = "You Lose...";
                }
            }
            else
            {
                wolResult.gameObject.SetActive(false);
            }

            scoreResult.text = "Score : "+ playerModel.Score;
        }
    }
    
}