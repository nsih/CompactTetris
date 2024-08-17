using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnMatchCon : MonoBehaviour
{
    PlayerModel playerModel;
    OpponentModel opponentModel;
    NetworkStateModel networkStateModel;



    public void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;
        opponentModel = GameManager.Instance.OpponentModel;
        networkStateModel = GameManager.Instance.NetworkStateModel;

        PlayerModel.OnPlayerDataChanged += BtnActivateSwitch;
    }
    public void OnClickAction()
    {
        StartCoroutine(WaitingOpponenet());
    }

    IEnumerator WaitingOpponenet()
    {
        GameManager.Instance.GetComponent<NetworkManager>().JoinMatchmakingReq();
        yield return new WaitForSecondsRealtime(1f);

        while (networkStateModel.NetworkState == NetworkState.Matching)
        {
            GameManager.Instance.GetComponent<NetworkManager>().WaitForMatchReq();

            yield return new WaitForSecondsRealtime(1f);
        }
        GameManager.Instance.SwitchTime();
    }

    public void BtnActivateSwitch()
    {
        if(networkStateModel.NetworkState == NetworkState.Single || networkStateModel.NetworkState == NetworkState.Matching)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }

        else
        {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
