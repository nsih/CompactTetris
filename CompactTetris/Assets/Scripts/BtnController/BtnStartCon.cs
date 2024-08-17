using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnStartCon : MonoBehaviour
{
    PlayerModel playerModel;
    public void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;

        PlayerModel.OnPlayerDataChanged += BtnActivateSwitch;
    }
    public void OnClickAction()
    {
        GameManager.Instance.SwitchTime();
    }

    public void BtnActivateSwitch()
    {
        if(playerModel.IsPlay)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
