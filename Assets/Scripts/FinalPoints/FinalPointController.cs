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

    private bool hasFinished = false;

    public IEnumerator CheckIfBothPlayersReachedCheckpoint()
    {
        if (hasFinished) yield break;  // Evitar ejecución repetida

        if (finalPointP1 != null && finalPointP2 != null)
        {
            if (finalPointP1.playerIsOnPoint && finalPointP2.playerIsOnPoint)
            {
                hasFinished = true; //Evita que s'executui més d'una vegada

                //inicia el fade
                Debug.Log("¡Ambos jugadores han llegado al checkpoint!");
                fade.FadeOut(); //fadeout 
                yield return new WaitForSeconds(fade.fadeDuration); //espera el rato del fade

                //Guarda el lvl i carrega la següent escena
                //int newLevel = PlayerPrefs.GetInt("Level") + 1;
                int newLevel = 2; //per testing
                PlayerPrefs.SetInt("Level", newLevel);
                PlayerPrefs.Save();
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Level", newLevel } });
                Debug.Log("Nivel guardado: " + PlayerPrefs.GetInt("Level"));

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel(nextScene); //carrega la seguent escena
                }
                else
                {
                    //si no es el master, fa una petició al master perque carregui la següent escena
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

