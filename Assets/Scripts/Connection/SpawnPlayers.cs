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
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform[] spawnPositions; // Array con diferentes posiciones de spawn

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameObject player = null;

            if (PhotonNetwork.IsMasterClient)
            {
                player = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPositions[0].position, Quaternion.identity);
                var personalizacion = player.GetComponent<PersonalizacionPersonaje>();

                Debug.Log("PersonalizacionPersonaje instance no es null");
                AsignarAccesoriosDesdePrefab(player, personalizacion);
                //el personaje es un children llamado Sphere dentro de la instancia con tag Player1
                personalizacion.CargarPersonalizacion();
                player.GetComponent<PlayerPersonalizacionRPC>().EnviarPersonalizacion();
                
            }
            else
            {
                player = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPositions[1].position, Quaternion.identity);
                var personalizacion = player.GetComponent<PersonalizacionPersonaje>();
                
                Debug.Log("Cliente NO master: asignando accesorios y enviando personalización");
                AsignarAccesoriosDesdePrefab(player, personalizacion);
                //personalizacion.CargarPersonalizacion();
                //player.GetComponent<PlayerPersonalizacionRPC>().EnviarPersonalizacion();
            }
        }
    }

    private void AsignarAccesoriosDesdePrefab(GameObject player, PersonalizacionPersonaje personalizacion)
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
