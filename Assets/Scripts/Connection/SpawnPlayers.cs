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
    public WaitingAnimation waitingAnimation;
    public TMP_Text lobbyCode;

    private void Start()
    {
        StartCoroutine(WaitForRoomReady());

        // Si el código aún no ha llegado, intentamos recuperarlo después de un pequeño delay
        StartCoroutine(WaitForRoomCode());

        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator WaitForRoomReady()
    {
        while (PhotonNetwork.CurrentRoom == null || !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Code"))
        {
            Debug.Log("Esperando que la sala se sincronice...");
            yield return new WaitForSeconds(0.5f);
        }

        lobbyCode.text = PhotonNetwork.CurrentRoom.CustomProperties["Code"].ToString();
        Debug.Log("Código de la sala sincronizado: " + lobbyCode.text);

        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator WaitForRoomCode()
    {
        while (PhotonNetwork.CurrentRoom == null || !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Code"))
        {
            yield return new WaitForSeconds(0.5f);
        }

        lobbyCode.text = PhotonNetwork.CurrentRoom.CustomProperties["Code"].ToString();
        Debug.Log("Código de la sala sincronizado: " + lobbyCode.text);
    }


    // Este método se ejecuta cuando las propiedades de la sala cambian (incluyendo "Code")
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("Code"))
        {
            string roomCode = PhotonNetwork.CurrentRoom.CustomProperties["Code"].ToString();
            lobbyCode.text = roomCode;
            Debug.Log("Código de la sala actualizado correctamente: " + roomCode);
        }
    }

    private IEnumerator SpawnPlayerWithDelay()
    {
        yield return new WaitForSeconds(1);

        GameObject prefabToSpawn = PhotonNetwork.IsMasterClient ? playerPrefab1 : playerPrefab2;
        int spawnIndex = PhotonNetwork.IsMasterClient ? 0 : 1;

        GameObject spawnedPlayer = PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPositions[spawnIndex].position, Quaternion.identity);

        Debug.Log("Jugador instanciado: " + spawnedPlayer.name);

        // Esperar hasta que ambos jugadores estén en la sala
        while (PhotonNetwork.PlayerList.Length < 2)
        {
            Debug.Log("Esperando a que ambos jugadores se unan...");
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Número de jugadores en la sala: " + PhotonNetwork.PlayerList.Length);

        if (SceneManager.GetActiveScene().name == "Waiting")
        {
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                Debug.Log("Ambos jugadores han llegado a la sala. Iniciando animación...");
                waitingAnimation.AnimationBothPlayers();
            }
        }
    }
}
