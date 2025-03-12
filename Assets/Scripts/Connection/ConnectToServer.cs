using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("PhotonUserId"))
        {
            string uniqueID = System.Guid.NewGuid().ToString(); //genera un id unic
            PlayerPrefs.SetString("PhotonUserId", uniqueID);
            PlayerPrefs.Save();
        }

        PhotonNetwork.AuthValues = new AuthenticationValues(PlayerPrefs.GetString("PhotonUserId"));

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
