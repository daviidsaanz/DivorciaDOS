using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FinalPointController : MonoBehaviourPunCallbacks
{
    public static FinalPointController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public FinalPoint finalPointP1;
    public FinalPoint finalPointP2;
    public string nextScene;

    public void CheckIfBothPlayersReachedCheckpoint()
    {
        if (finalPointP1 != null && finalPointP2 != null)
        {
            if (finalPointP1.playerIsOnPoint && finalPointP2.playerIsOnPoint)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("¡Ambos jugadores han llegado al checkpoint!");
                    PhotonNetwork.LoadLevel(nextScene); // Cargar la siguiente escena
                }
                else
                {
                    Debug.Log("¡Ambos jugadores han llegado al checkpoint! (No soy el MasterClient)");
                    photonView.RPC("RPC_RequestLevelChange", RpcTarget.MasterClient);
                }

            }
        }
    }

    [PunRPC]
    public void RPC_RequestLevelChange()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(nextScene);
        }
    }
}

