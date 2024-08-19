using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnReset : MonoBehaviour
{
    PlayerModel playerModel;
    OpponentModel opponentModel;
    NetworkStateModel networkStateModel;

    void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;
        opponentModel = GameManager.Instance.OpponentModel;
        networkStateModel = GameManager.Instance.NetworkStateModel;
    }

    
    public void OnClickAction()
    {
        playerModel.Init();
        opponentModel.Init();
        networkStateModel.NetworkState = NetworkState.Single;

        GameObject.Find("Result").SetActive(false);
        GameManager.Instance.grid.SetActive(true);
    }
}
