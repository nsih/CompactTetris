using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
        PlayerModel.OnPlayerDataChanged += GameOver;

        playerModel.IsPlay = false;
        Time.timeScale = 0f;

        //Debug.Log(networkStateModel.NetworkState);
    }

    public void SwitchTime()
    {
        if (!playerModel.IsPlay)
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

    // 코루틴으로 시간 감소
    private IEnumerator TimeControlCoroutine()
    {
        playerModel.IsPlay = true;

        while (Time.timeScale > 0 && playerModel.IsPlay)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
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

    private void GameOver()
    {
        if(playerModel.IsPlay == false || opponentModel.IsPlay == false)
        {
            //끝난상태 업데이트
            //UI 띄우기
            //API 호출
        }
    }
    
}