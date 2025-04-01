using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform[] spawnPositions; // Array con diferentes posiciones de spawn

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(playerPrefab1.name, spawnPositions[0].position, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(playerPrefab2.name, spawnPositions[1].position, Quaternion.identity);
            }
        }
    }
}
