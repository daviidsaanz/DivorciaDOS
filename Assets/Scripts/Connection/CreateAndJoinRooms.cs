using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text playWithFriendText; // Boto de "Jugar amb X"
    public GameObject invitePanel; // Panel de invitació per acceptar o rebutjar
    public TMP_Text inviteText; // Text de la invitació

    private string savedPartnerID;
    private string savedPartnerName;
    private int savedLevel;

    private void Start()
    {
        savedPartnerID = PlayerPrefs.GetString("LastPartnerID", "");
        savedPartnerName = PlayerPrefs.GetString("LastPartnerName", "");
        savedLevel = PlayerPrefs.GetInt("LastLevel", 1);

        if (!string.IsNullOrEmpty(savedPartnerID))
        {
            playWithFriendText.text = "Jugar con " + savedPartnerName;
            playWithFriendText.transform.parent.gameObject.SetActive(true);
        }
    }


    public void InviteFriend()
    {
        int targetActor = GetPlayerActorNumber(savedPartnerID);
        if (targetActor != -1)
        {
            string roomName = PlayerPrefs.GetString("LastRoomName", "");
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("No hay una sala guardada para invitar.");
                return;
            }

            object[] data = new object[] { PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.LocalPlayer.NickName, roomName };
            PhotonNetwork.RaiseEvent(1, data, new RaiseEventOptions { TargetActors = new int[] { targetActor } }, SendOptions.SendReliable);
        }
    }



    public void AcceptInvite()
    {
        string invitedRoom = PlayerPrefs.GetString("InvitedRoomName", "");
        if (!string.IsNullOrEmpty(invitedRoom))
        {
            PhotonNetwork.JoinRoom(invitedRoom);
            invitePanel.SetActive(false);
        }
    }



    public void RejectInvite()
    {
        invitePanel.SetActive(false); //tanca el panell de invitacio
    }

    public void SwitchOfCreateOrJoin()
    {
        //si joinInput té algo escrit i createInput tambe, no fa resw
        if (!string.IsNullOrEmpty(joinInput.text) && !string.IsNullOrEmpty(createInput.text))
        {
            return;
        }
        //si joinInput te algo escrit, i createInput no, crida a joinRoom
        if (!string.IsNullOrEmpty(joinInput.text))
        {
            JoinRoom();
        }

        //si joinInput no te algo escrit, i createInput si, crida a createRoom
        if (!string.IsNullOrEmpty(createInput.text))
        {
            CreateRoom();
        }

    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text))
        {
            Debug.LogError("Debes escribir un nombre para la sala.");
            return;
        }

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };

        Hashtable roomProperties = new Hashtable();
        roomProperties["FirstPlayerID"] = PhotonNetwork.LocalPlayer.UserId;
        roomProperties["SavedLevel"] = savedLevel > 0 ? savedLevel : 1;

        roomOptions.CustomRoomProperties = roomProperties;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "FirstPlayerID", "SavedLevel" };

        Debug.Log("Creando sala: " + createInput.text);
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);

        // Guarda el nombre de la sala para que el compañero lo pueda recordar después
        PlayerPrefs.SetString("LastRoomName", createInput.text);
        PlayerPrefs.Save();

        //carga el nivel 1
        PhotonNetwork.LoadLevel("Level1");



    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInput.text))
        {
            Debug.LogError("Debes escribir el nombre de la sala para unirte.");
            return;
        }

        Debug.Log("Intentando unirse a la sala: " + joinInput.text);
        PhotonNetwork.JoinRoom(joinInput.text);
        PhotonNetwork.LoadLevel("Level1");


    }




    public void JoinSavedPartner() //metode per unir-se al amic guardat
    {
        string lastRoom = PlayerPrefs.GetString("LastRoomName", ""); //agafa el nom de la ultima sala
        if(!string.IsNullOrEmpty(lastRoom)) //si no esta buit
        {
            PhotonNetwork.JoinRoom(lastRoom); //entra a la sala
        }
       
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Se ha unido a la sala correctamente: " + PhotonNetwork.CurrentRoom.Name);

        string firstPlayerID = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("FirstPlayerID")
            ? PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerID"].ToString()
            : "";

        int roomLevel = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SavedLevel")
            ? (int)PhotonNetwork.CurrentRoom.CustomProperties["SavedLevel"]
            : 1;

        if (!string.IsNullOrEmpty(firstPlayerID) && firstPlayerID != PhotonNetwork.LocalPlayer.UserId)
        {
            // Guardamos el compañero para futuras partidas
            PlayerPrefs.SetString("LastPartnerID", firstPlayerID);
            PlayerPrefs.SetString("LastPartnerName", PhotonNetwork.CurrentRoom.Name);
            PlayerPrefs.SetInt("LastLevel", roomLevel);
            PlayerPrefs.SetString("LastRoomName", PhotonNetwork.CurrentRoom.Name);
            PlayerPrefs.Save();
        }

        // Cargar nivel
        PhotonNetwork.LoadLevel("Level" + roomLevel);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al unirse a la sala: " + message);
    }


    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent; //afegeix un event per rebre invitacions
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent; //elimina l'event per rebre invitacions
    }

    private void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            object[] data = (object[])photonEvent.CustomData;
            string inviterID = (string)data[0];
            string inviterName = (string)data[1];
            string roomName = (string)data[2];

            inviteText.text = inviterName + " quiere jugar contigo en la sala " + roomName;
            invitePanel.SetActive(true);

            // Guardar el nombre de la sala en "LastRoomName"
            PlayerPrefs.SetString("LastRoomName", roomName);
            PlayerPrefs.Save();

            Debug.Log("Se recibió una invitación para unirse a la sala: " + roomName);
        }
    }



    private int GetPlayerActorNumber(string userID)
    {
        foreach(Player player in PhotonNetwork.PlayerList) //per cada jugador a la llista de jugadors
        {
            if(player.UserId == userID) //si el id del jugador es igual al id del jugador
            {
                return player.ActorNumber; //retorna el numero d'actor del jugador
            }
        }

        return -1; //si no troba el jugador, retorna -1
    }
    

}
