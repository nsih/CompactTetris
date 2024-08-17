using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayView : MonoBehaviour
{
    public GameObject nextBlockImg;

    public TMP_Text scoreText;
    public TMP_Text timerText;

    public PlayerModel playerModel;
    public OpponentModel opponentModel;
    public NetworkStateModel networkStateModel;


    /////
    public GameObject opponentImage;
    public TMP_Text opponentId;
    public TMP_Text opponentScore;

    //
    public TMP_Text gameState;



    public Sprite CaptureImg;


    void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;
        opponentModel = GameManager.Instance.OpponentModel;
        networkStateModel = GameManager.Instance.NetworkStateModel;

        //player
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
        timerText = GameObject.Find("Timer").GetComponent<TMP_Text>();
        nextBlockImg = GameObject.Find("NextBlock");

        //opponent
        opponentImage = GameObject.Find("OpponentImage");   
        opponentId = GameObject.Find("MatchState").GetComponent<TMP_Text>();
        opponentScore = GameObject.Find("EnemyScore").GetComponent<TMP_Text>();

        //state
        gameState = GameObject.Find("GameState").GetComponent<TMP_Text>();



        //event subscribe
        PlayerModel.OnPlayerDataChanged += UpdateView;
        OpponentModel.OnOpponentDataChanged += UpdateOpponentView;
        NetworkStateModel.OnNetworkStateChanged += UpdateStateView;

        //init
        UpdateView();
    }

    public void UpdateView()
    {
        scoreText.text = "Score : "+playerModel.Score.ToString();
        timerText.text = "Time  : "+playerModel.Time.ToString()+ "Sec";

        nextBlockImg.GetComponent<Image>().sprite = playerModel.NextBlockImg;

        Debug.Log("Update View");
    }

    public void UpdateOpponentView()
    {
        if(opponentModel.UserId == null)
        {
            opponentImage.GetComponent<Image>().sprite = null;
            opponentId.text = "not matched";
            opponentScore.text = "";
        }
        else
        {
            if (opponentModel.GameSceneImg != null)
            {
                CaptureImg = gameObject.GetComponent<ScreenshotManager>().decriptCapture(opponentModel.GameSceneImg.ToString());
                opponentImage.GetComponent<Image>().sprite = CaptureImg;

                Debug.Log(opponentModel.GameSceneImg);
            }
            else
            {
                Debug.Log("opponentModel.GameSceneImg == null");
            }
            opponentId.text = "Matched : " + opponentModel.UserId.ToString();
            opponentScore.text = "Score : " + opponentModel.Score.ToString();

            Debug.Log("Update OpponentView");
        }
    }

    public void UpdateStateView()
    {
        if(networkStateModel.NetworkState == NetworkState.Single)
        {
            gameState.text = "Single";
        }
        else if(networkStateModel.NetworkState == NetworkState.Matching)
        {
            gameState.text = "Matching";
        }
        else if(networkStateModel.NetworkState == NetworkState.Matched)
        {
            gameState.text = "Match Start";
        }
    }
}
