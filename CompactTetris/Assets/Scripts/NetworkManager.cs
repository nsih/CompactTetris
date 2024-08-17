using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.IO;

public class NetworkManager : MonoBehaviour
{
    public delegate void MatchFoundHandler(string opponentId);
    public static event MatchFoundHandler OnMatchFound;


    public PlayerModel playerModel;
    public OpponentModel opponentModel;
    public NetworkStateModel networkStateModel;

    private string uri = "http://192.168.35.58:8080/";

    //192.168.35.58

    //http://192.168.35.58:8080/
    //172.29.34.195

    [System.Serializable]
    public class GameState
    {
        public string playerId;
        public string opponentId;
        public int time;
        public int score;
        public string gameSceneImg;
        public bool isPlay;

        public GameState(string playerId,int time, int score, bool isPlay, string gameSceneImg)
        {
            this.playerId = playerId;
            this.time = time;
            this.score = score;
            this.isPlay = isPlay;
            this.gameSceneImg = gameSceneImg;
        }
    }

    // Start() 메서드에서 POST 요청 호출
    void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;
        opponentModel = GameManager.Instance.OpponentModel;
        networkStateModel = GameManager.Instance.NetworkStateModel;
    }

    #region 'GET'

    //상대 정보 받아오기
    public void GETReq()
    {
        StartCoroutine(GetGameState(opponentModel.UserId));
    }

    // GET 요청을 보내는 코루틴
    IEnumerator GetGameState(string playerId)
    {
        // GET 요청 생성후 응답대기
        UnityWebRequest request = UnityWebRequest.Get(uri + "state/" + playerId);
        yield return request.SendWebRequest();

        //
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Get Opponent Complete");
            string jsonResponse = request.downloadHandler.text;
            
            GameState gameState = JsonConvert.DeserializeObject<GameState>(jsonResponse);

            Debug.Log(jsonResponse);

            //opponentModel update
            opponentModel.UserId = gameState.playerId;
            opponentModel.Time = gameState.time;
            opponentModel.Score = gameState.score;
            opponentModel.IsPlay = gameState.isPlay;
            opponentModel.GameSceneImg = gameState.gameSceneImg;

            //Debug.Log(opponentModel.GameSceneImg);
        }
        else
            Debug.LogError("Error: " + request.error);
    }    
    #endregion

    #region 'POST'
    public void POSTReq()
    {
        //player id
        GameState gameState = new GameState(
            playerModel.UserId,
            playerModel.Time, 
            playerModel.Score,
            playerModel.IsPlay, 
            playerModel.GameSceneImg
            );

        //Debug.Log(playerModel.UserId);
        StartCoroutine(POSTGameState(gameState));
    }

    // POST 요청을 보내는 코루틴
    IEnumerator POSTGameState(GameState gameState)
    {
        // GameState 객체를 JSON 형식으로 변환
        string jsonData = JsonConvert.SerializeObject(gameState);

        // UnityWebRequest POST 요청 생성
        UnityWebRequest request = new UnityWebRequest(uri + "save", "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        
        //Debug.Log(uri + "save");
        //Debug.Log(jsonData);

        // wait response
        yield return request.SendWebRequest();

        // debug
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("GameState POSTED");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }



    // 매칭 대기 요청
    public void JoinMatchmakingReq()
    {
        StartCoroutine(JoinMatchmaking(playerModel.UserId));
    }

    IEnumerator JoinMatchmaking(string playerId)
    {
        UnityWebRequest request = new UnityWebRequest(uri + "matchmaking?playerId=" + playerId, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            networkStateModel.NetworkState = NetworkState.Matching;
            StartCoroutine(WaitForMatch());
        }
        else
        {
            networkStateModel.NetworkState = NetworkState.Single;
            Debug.LogError("Error: " + request.error);
        }
    }


    public void WaitForMatchReq()
    {
        StartCoroutine(WaitForMatch());
    }

    IEnumerator WaitForMatch()
    {
        UnityWebRequest request = UnityWebRequest.Get(uri + "matchmaking-status?playerId=" + playerModel.UserId);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "text/plain");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string opponentId = request.downloadHandler.text;

            if (!string.IsNullOrEmpty(opponentId))
            {
                Debug.Log("Match found! Opponent ID: " + opponentId);
                opponentModel.UserId = opponentId;
                networkStateModel.NetworkState = NetworkState.Matched;
            }
            else
            {
                Debug.Log("Still waiting for opponent...");
            }
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }

        Debug.Log("WaitForMatch Cycle Complete");

        yield return new WaitForSecondsRealtime(1f);
    }

    #endregion
}