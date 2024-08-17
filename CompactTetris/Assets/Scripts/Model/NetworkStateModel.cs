using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkStateModel
{
    private NetworkState networkState;

    public static event Action OnNetworkStateChanged;


    public NetworkStateModel()
    {
        networkState = NetworkState.Single;

        //Debug.Log("nwm instance");
    }

    public NetworkState NetworkState
    {
        get => networkState;
        set
        {
            if (networkState != value)
            {
                networkState = value;
                OnNetworkStateChanged?.Invoke();
            }
        }
    }
}


public enum NetworkState
{
    Single,
    Matching,
    Matched
}
