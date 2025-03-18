using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
<<<<<<< HEAD
    public TMP_InputField createInput; //NOM DE LA SALA
    public TMP_InputField joinInput; //CODI DE LA SALA
    public Transform savedRoomsPanel;
    public GameObject roomButtonPrefab;
=======
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text playWithFriendText; // Boto de "Jugar amb X"
    public GameObject invitePanel; // Panel de invitació per acceptar o rebutjar
    public TMP_Text inviteText; // Text de la invitació
>>>>>>> d6f30857688709e545f7e68d0eb9ef63baafb283

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>(); //llista de sales actives

    void Start()
    {
<<<<<<< HEAD
        PhotonNetwork.JoinLobby(); // Nos unimos al lobby para recibir lista de salas
        LoadSavedRooms();
    }

    private string GenerateRoomCode()
    {
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int i = 0; i < 10; i++)
        {
            string numbers = Random.Range(100, 999).ToString();
            string letter = letters[Random.Range(0, letters.Length)].ToString();
            string code = numbers + letter;

            if (!RoomExists(code)) return code; //si la sala no existeix, retorna el codi
=======
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
>>>>>>> d6f30857688709e545f7e68d0eb9ef63baafb283
        }
        return null;
    }

<<<<<<< HEAD
    private bool RoomExists(string code)
    {
        foreach (RoomInfo room in cachedRoomList) //per cada sala a la llista de sales
        {
            if (room.CustomProperties.ContainsKey("Code") && (string)room.CustomProperties["Code"] == code) //si la sala ja existeix amb el codi generat
            {
                return true;
            }
        }
        return false;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) //quan la llista de sales s'actualitza
=======


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
>>>>>>> d6f30857688709e545f7e68d0eb9ef63baafb283
    {
        cachedRoomList = roomList;
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
<<<<<<< HEAD
        string roomCode = GenerateRoomCode();
        if (roomCode == null)
        {
            Debug.LogError("Failed to generate room code");
            return;
        }

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = new Hashtable
            {
                { "Code", roomCode },
                { "Level", 0 },
                { "Player1", PhotonNetwork.LocalPlayer.UserId }, //jugador1
                { "Player2", "" } //jugador2 (buit)
            },
            CustomRoomPropertiesForLobby = new string[] { "Code" }
        };
        PhotonNetwork.CreateRoom(roomCode, roomOptions); //crea la sala amb el codi generat i les opcions de la sala
    }

    public void JoinRoom()
=======
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
>>>>>>> d6f30857688709e545f7e68d0eb9ef63baafb283
    {
        string inputCode = joinInput.text; //codi de la sala
        foreach(RoomInfo room in cachedRoomList) //per cada sala a la llista de sales
        {
            if (room.CustomProperties.ContainsKey("Code") && (string)room.CustomProperties["Code"] == inputCode) //si la sala te el codi introduit
            {
                PhotonNetwork.JoinRoom(room.Name); //s'afegeix a la sala
                return;
            }
        }
        Debug.LogError("Room not found");
    }

    public override void OnJoinedRoom()
    {
<<<<<<< HEAD
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        string localPlayerID = PhotonNetwork.LocalPlayer.UserId; //id del jugador que s'ha unit

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player1", out object player1) && PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2)) //si la sala te els jugadors 1 i 2
        {
            if(string.IsNullOrEmpty((string)player2) && (string)player1 != localPlayerID) //si el jugador 2 es buit i el jugador 1 no es el jugador local
=======
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
>>>>>>> d6f30857688709e545f7e68d0eb9ef63baafb283
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "Player2", localPlayerID } }); //afegeix el jugador local com a jugador 2
                SaveRoom((string)PhotonNetwork.CurrentRoom.CustomProperties["Code"]); //guarda la sala
            }
            else if ((string)player1 != localPlayerID && (string)player2 != localPlayerID) //si el jugador 1 i 2 no son el jugador local
            {
                Debug.LogError("You are not allowed to join this room.");
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    private void SaveRoom(string roomCode)
    {
        List<string> savedRooms = new List<string>(PlayerPrefs.GetString("SavedRooms", "").Split(',')); //llegeix les sales guardades a la memoria del dispositiu i les guarda a una llista de strings
        if (!savedRooms.Contains(roomCode)) //si la sala no esta guardada
        {
            savedRooms.Add(roomCode); //afegeix la sala a la llista
            PlayerPrefs.SetString("SavedRooms", string.Join(",", savedRooms)); //guarda la llista de sales a la memoria del dispositiu
            PlayerPrefs.Save();
        }
    }

    public void LoadSavedRooms()
    {
        string savedRoomsStr = PlayerPrefs.GetString("SavedRooms", "");
        if (string.IsNullOrEmpty(savedRoomsStr)) { return; }

        string[] savedRooms = savedRoomsStr.Split(',');
        foreach (string roomCode in savedRooms)
        {
            GameObject btn = Instantiate(roomButtonPrefab, savedRoomsPanel); //s'instancia un boto de sala
            btn.GetComponentInChildren<TMP_Text>().text = roomCode; //es posa el codi de la sala al text del boto
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => JoinRoomByCode(roomCode)); //quan es clica al boto, s'afegeix la sala amb el codi a la llista de sales
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    private void JoinRoomByCode(string code)
    {
        joinInput.text = code;
        JoinRoom();
    }
}
