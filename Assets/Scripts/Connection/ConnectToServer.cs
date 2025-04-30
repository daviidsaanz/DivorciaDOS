using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject userNamePanel;
    public TMP_InputField userNameInput;
    public TMP_Text connected;
    public TMP_Text noConnection;

    void Start()
    {
        if(PlayerPrefs.HasKey("PhotonUserId")) //si ja te un id guardat
        {
            ConnectToPhoton();
            Debug.Log("PhotonUserId: " + PlayerPrefs.GetString("PhotonUserId"));
           
        }
        else
        {
            userNamePanel.SetActive(true);
        }
    }

    public void SetUserName()
    {
        if (userNameInput.text.Length < 3) return;

        string username = userNameInput.text;
        string uniqueID = username + "_" + System.Guid.NewGuid().ToString(); //crea un id unic

        PlayerPrefs.SetString("PhotonUsername", username); //guarda el nom d'usuari
        PlayerPrefs.SetString("PhotonUserId", uniqueID); //guarda l'id unic
        PlayerPrefs.Save();

        userNamePanel.SetActive(false);
        ConnectToPhoton();

    }

    private void ConnectToPhoton()
    {
        PhotonNetwork.AuthValues = new AuthenticationValues(PlayerPrefs.GetString("PhotonUserId")); //crea un objecte de autenticacio amb l'id unic
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        connected.gameObject.SetActive(true);
        StartCoroutine(LoadSceneWithDelay());

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        noConnection.gameObject.SetActive(true);
    }

    public IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Lobby");
    }
}
