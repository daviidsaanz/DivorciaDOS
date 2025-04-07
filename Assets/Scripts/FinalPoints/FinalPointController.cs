using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FinalPointController : MonoBehaviourPunCallbacks
{
    public static FinalPointController instance;
    public Fade fade;

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

    public IEnumerator CheckIfBothPlayersReachedCheckpoint()
    {
        if (finalPointP1 != null && finalPointP2 != null)
        {
            if (finalPointP1.playerIsOnPoint && finalPointP2.playerIsOnPoint)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("¡Ambos jugadores han llegado al checkpoint!");
                    fade.FadeOut(); // Iniciar la transición de fade
                    yield return new WaitForSeconds(1f); // Esperar un segundo para que el fade se complete
                    int newLevel = PlayerPrefs.GetInt("Level") + 1;
                    PlayerPrefs.SetInt("Level", newLevel);
                    PlayerPrefs.Save();
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Level", newLevel } });
                    Debug.Log("Nivel guardado: " + PlayerPrefs.GetInt("Level")); // Guardar el nivel en PlayerPrefs
                    PhotonNetwork.LoadLevel(nextScene); // Cargar la siguiente escena

                }
                else
                {
                    Debug.Log("¡Ambos jugadores han llegado al checkpoint! (No soy el MasterClient)");
                    fade.FadeOut();
                    yield return new WaitForSeconds(1f); // Esperar un segundo para que el fade se complete
                    int newLevel = PlayerPrefs.GetInt("Level") + 1;
                    PlayerPrefs.SetInt("Level", newLevel);
                    PlayerPrefs.Save();
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Level", newLevel } });
                    Debug.Log("Nivel guardado: " + PlayerPrefs.GetInt("Level")); // Guardar el nivel en PlayerPrefs
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

