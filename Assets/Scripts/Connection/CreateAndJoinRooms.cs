using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UIElements;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        string secondPlayerID = PlayerPrefs.GetString("SecondPlayerID", ""); //busca el id del segon jugador
        int saveLevel = PlayerPrefs.GetInt("Level", 0); //busca el nivell guardat

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        Hashtable roomProperties = new Hashtable(); //crea una taula de propietats
        roomProperties["FirstPlayerID"] = PhotonNetwork.LocalPlayer.UserId; //guarda el id del primer jugador
        roomProperties["SecondPlayerID"] = secondPlayerID; //guarda el id del segon jugador
        roomProperties["Level"] = saveLevel; //guarda el nivell guardat

        roomOptions.CustomRoomProperties = roomProperties; //guarda les propietats a la sala
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "FirstPlayerID", "SecondPlayerID", "Level" }; //mostra les propietats a la llista de sales

        PhotonNetwork.CreateRoom(createInput.text, roomOptions); //crea la sala amb el nom introduit
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("FirstPlayerID")) //si hi ha un jugador a la sala
        {
            string firstPlayerID = PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerID"].ToString(); //guarda el id del primer jugador
            string secondPlayerID = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SecondPlayerID")
            ? PhotonNetwork.CurrentRoom.CustomProperties["SecondPlayerID"].ToString() : ""; //guarda el id del segon jugador

            if(secondPlayerID == "") //si no hi ha player 2
            {
                PlayerPrefs.SetString("SecondPlayerID", firstPlayerID); //guarda el id del segon jugador
                PlayerPrefs.Save();
            }

            if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Level1")) //si hi ha un nivell guardat
            {
                int level = (int)PhotonNetwork.CurrentRoom.CustomProperties["Level1"]; //guarda el nivell
                PlayerPrefs.SetInt("Level", level); //guarda el nivell
                PlayerPrefs.Save();
                Debug.Log("Level syncronized " + level);
            }
        }

        PhotonNetwork.LoadLevel("Level1"); //carrega el nivell

    }

}
