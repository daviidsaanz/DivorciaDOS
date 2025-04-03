using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FinalPointController : MonoBehaviour
{
    public static FinalPointController instance;

    public FinalPoint finalPointP1;
    public FinalPoint finalPointP2;
    public string nextScene;

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

        DontDestroyOnLoad(gameObject);
    }

    public void CheckIfBothPlayersReachedCheckpoint()
    {
        if (finalPointP1 != null && finalPointP2 != null)
        {
            if (finalPointP1.playerIsOnPoint && finalPointP2.playerIsOnPoint)
            {
                Debug.Log("¡Ambos jugadores han llegado al checkpoint!");
                PhotonNetwork.LoadLevel(nextScene);
            }
        }
    }
}
