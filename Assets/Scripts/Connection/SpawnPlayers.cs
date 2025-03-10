using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform[] spawnPositions; // Array con diferentes posiciones de spawn

    private void Start()
    {
        StartCoroutine(WaitForPlayers());
    }

    private IEnumerator WaitForPlayers()
    {
        // Esperar a que se instancien los jugadores
        yield return new WaitForSeconds(1);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab1.name, spawnPositions[0].position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab2.name, spawnPositions[1].position, Quaternion.identity);
        }
        if (GameObject.Find("Player1") != null && GameObject.Find("Player2") != null)
        {
            StopCoroutine(WaitForPlayers());
        }
    }
}
