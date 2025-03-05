using System.Collections;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    private Transform spawnPosition;

    private void Awake()
    {
        spawnPosition = GetComponent<Transform>();   
    }

    public void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
    }
}


