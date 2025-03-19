using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform[] spawnPositions; // Array con diferentes posiciones de spawn
    public WaitingAnimation waitingAnimation;

    private void Start()
    {
        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator SpawnPlayerWithDelay()
    {
        // Esperar a que se instancien los jugadores
        yield return new WaitForSeconds(1);
        GameObject prefabToSpawn = PhotonNetwork.IsMasterClient ? playerPrefab1 : playerPrefab2; 
        int spawnIndex = PhotonNetwork.IsMasterClient ? 0 : 1;

        PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPositions[spawnIndex].position, Quaternion.identity);

        Debug.Log(PhotonNetwork.PlayerList.Length);

        if (SceneManager.GetActiveScene().name == "Waiting")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                waitingAnimation.AnimationPlayer1();
            }
            else
            {
                waitingAnimation.AnimationPlayer2();
            }

        }

    }
}
