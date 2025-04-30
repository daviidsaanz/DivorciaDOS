using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public static SpawnPlayers instance;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform[] spawnPositions; // Array con diferentes posiciones de spawn

    void Awake()
    {
        // Asegurarse de que solo haya una instancia de SpawnPlayers
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destruye el objeto si ya existe una instancia
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                GameObject player = null;

                if (PhotonNetwork.IsMasterClient) //si es el player 1
                {
                    player = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPositions[0].position, Quaternion.identity); //instancia el prefab del player 1 en la pososicio 0
                    var personalizacion = player.GetComponent<PersonalizacionPersonaje>(); //agafa el script de personalitzacio del player 1

                    Debug.Log("Client master provant d'assignar la seva personalitzacio local");
                    AsignarAccesoriosDesdePrefab(player, personalizacion); //posa be els accessoris al prefab al inspector (busca i filtra en la jerarquia perque el script de personalitzacio agafi les referencies dinamicament)
                    personalizacion.CargarPersonalizacion(); //carrega la personalitzacio del player 1 del JSON i ho aplica (perque es vegi)

                    Debug.Log("Client master: enviant la seva personalizacio");

                    PlayerPersonalizacionRPC playerPersonalizacionRPC = player.GetComponent<PlayerPersonalizacionRPC>();
                    playerPersonalizacionRPC.EnviarPersonalizacion(player.GetComponent<PhotonView>().ViewID); //envia la personalitzacio al player 2

                }
                else //si es el player 2
                {
                    player = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPositions[1].position, Quaternion.identity); //instancia el prefab del player 2 en la pososicio 1
                    var personalizacion = player.GetComponent<PersonalizacionPersonaje>(); //agafa el script de personalitzacio del player 2

                    Debug.Log("Client NO master: provant d'assignar la seva personalitzacio local");
                    AsignarAccesoriosDesdePrefab(player, personalizacion); //posa be els accessoris al prefab al inspector (busca i filtra en la jerarquia perque el script de personalitzacio agafi les referencies dinamicament)
                    personalizacion.CargarPersonalizacion(); //carrega la personalitzacio del player 2 del JSON i ho aplica (perque es vegi)

                    Debug.Log("Client NO master: enviant la seva personalizacio");

                    PlayerPersonalizacionRPC playerPersonalizacionRPC = player.GetComponent<PlayerPersonalizacionRPC>();
                    playerPersonalizacionRPC.EnviarPersonalizacion(player.GetComponent<PhotonView>().ViewID); //envia la personalitzacio al player 1

                }

            }
            
        }
    }


    public void AsignarAccesoriosDesdePrefab(GameObject player, PersonalizacionPersonaje personalizacion)
    {
        // Asignar la referencia al personaje (ej: la malla que cambia color)
        personalizacion.personaje = player.GetComponentInChildren<SkinnedMeshRenderer>().gameObject;

        // Buscar el objeto "Capes" en toda la jerarquía del jugador
        Transform capesTransform = player.transform.FindDeepChild("Capes");
        if (capesTransform != null)
        {
            // Filtramos solo los objetos directos hijos de "Capes", no sus subcomponentes
            personalizacion.capas = capesTransform.GetComponentsInChildren<Transform>(true)
                .Where(t => t.parent == capesTransform) // Filtrar solo los hijos directos de "Capes"
                .Select(t => t.gameObject)
                .ToArray();
        }

        // Repetir el mismo proceso para los otros accesorios
        Transform headTransform = player.transform.FindDeepChild("Head");
        if (headTransform != null)
        {
            personalizacion.sombreros = headTransform.GetComponentsInChildren<Transform>(true)
                .Where(t => t.parent == headTransform) // Filtrar solo los hijos directos de "Head"
                .Select(t => t.gameObject)
                .ToArray();
        }

        Transform faceTransform = player.transform.FindDeepChild("Face");
        if (faceTransform != null)
        {
            personalizacion.caras = faceTransform.GetComponentsInChildren<Transform>(true)
                .Where(t => t.parent == faceTransform) // Filtrar solo los hijos directos de "Face"
                .Select(t => t.gameObject)
                .ToArray();
        }

        Transform ribbonsTransform = player.transform.FindDeepChild("Ribbons");
        if (ribbonsTransform != null)
        {
            personalizacion.pajaritas = ribbonsTransform.GetComponentsInChildren<Transform>(true)
                .Where(t => t.parent == ribbonsTransform) // Filtrar solo los hijos directos de "Ribbons"
                .Select(t => t.gameObject)
                .ToArray();
        }

        Transform wingsTransform = player.transform.FindDeepChild("Wings");
        if (wingsTransform != null)
        {
            personalizacion.alas = wingsTransform.GetComponentsInChildren<Transform>(true)
                .Where(t => t.parent == wingsTransform) // Filtrar solo los hijos directos de "Wings"
                .Select(t => t.gameObject)
                .ToArray();
        }
    }
}

public static class TransformExtensions
{
    // Este método busca un objeto hijo por nombre en toda la jerarquía
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        foreach (Transform child in aParent)
        {
            if (child.name == aName)
                return child;

            Transform result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
}
