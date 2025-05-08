using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ResetLVL : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public static bool isReset = false;
    public static string previousScene = "";
    public void ResetLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isReset = true;
            previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel("Auxiliar");
        }
        else
        {
            photonView.RPC("RPC_RequestLevelChange", RpcTarget.MasterClient);
        }

    }

    [PunRPC]
    public void RPC_RequestLevelChange()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isReset = true;
            previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel("Auxiliar");

        }
    }
}

    
  
