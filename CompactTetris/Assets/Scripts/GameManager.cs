using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerModel = new PlayerModel();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        StartCoroutine(TimeControlCoroutine());
    }

    // 코루틴으로 시간 감소
    private IEnumerator TimeControlCoroutine()
    {
        while (PlayerModel.Time > 0)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            PlayerModel.DecreaseTime(); // 시간 감소
        }

        GameOver();
    }

    private void GameOver()
    {
        Time.timeScale = 0f;

        //끝난상태 업데이트
        //UI 띄우기
        //API 호출
    }
    
}