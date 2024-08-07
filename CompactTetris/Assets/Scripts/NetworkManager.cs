using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.IO;

public class NetworkManager : MonoBehaviour
{
    public PlayerModel playerModel;

    private string uri = "http://192.168.35.195:8080/";


    [System.Serializable]
    public class GameState
    {
        public string playerId;
        public int time;
        public int score;
        public bool gameOver;
        public string imageUrl;

        public GameState(string playerId,int time, int score, bool gameOver, string imageUrl)
        {
            this.playerId = playerId;
            this.time = time;
            this.score = score;
            this.gameOver = gameOver;
            this.imageUrl = imageUrl;
        }
    }

    // Start() 메서드에서 POST 요청 호출
    void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;
        
        GETReqTemp();
    }

    #region 'GET'
    void GETReqTemp()
    {
        string playerId = playerModel.UserId;
        StartCoroutine(GetGameStatetemp());
    }

    // GET 요청을 보내는 코루틴
    IEnumerator GetGameStatetemp()
    {
        // UnityWebRequest GET 요청 생성
        UnityWebRequest request = UnityWebRequest.Get(uri);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        // 응답 확인
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GameState retrieved");
            string jsonResponse = request.downloadHandler.text;
            //playerModel.
            Debug.Log(jsonResponse);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }



    void GETReq()
    {
        string playerId = playerModel.UserId;
        StartCoroutine(GetGameState(playerId));
    }

    // GET 요청을 보내는 코루틴
    IEnumerator GetGameState(string playerId)
    {
        // UnityWebRequest GET 요청 생성
        UnityWebRequest request = UnityWebRequest.Get(uri + "state" + playerId);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        // 응답 확인
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GameState retrieved");
            string jsonResponse = request.downloadHandler.text;

            // JSON 응답을 GameState 객체로 변환
            GameState gameState = JsonConvert.DeserializeObject<GameState>(jsonResponse);


            //playerModel.

            Debug.Log("Player ID: " + gameState.playerId);
            Debug.Log("Score: " + gameState.score);
            Debug.Log("Game Over: " + gameState.gameOver);
            Debug.Log("Image URL: " + gameState.imageUrl);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
    
    #endregion

    #region 'POST'
    void POSTReq()
    {
        //player id
        GameState gameState = new GameState(
            playerModel.UserId, 
            playerModel.Time, 
            playerModel.Score,
            playerModel.IsEnd, 
            "https://example.com/image.png"
            );

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

        // wait response
        yield return request.SendWebRequest();

        // debug
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GameState POSTED");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }



    // 매칭 대기 요청
    void POSTJoinMatchmaking()
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
            Debug.Log("Player added to matchmaking queue");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    // 매칭 시작 요청
    void POSTStartMatch()
    {
        StartCoroutine(StartMatch());
    }


    //상대방 id뱉기
    IEnumerator StartMatch()
    {
        UnityWebRequest request = new UnityWebRequest(uri + "start-match", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Match started");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
    #endregion
}