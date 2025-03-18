using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput; //NOM DE LA SALA
    public TMP_InputField joinInput; //CODI DE LA SALA
    public Transform savedRoomsPanel;
    public GameObject roomButtonPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>(); //llista de sales actives

    void Start()
    {
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
        }
        return null;
    }

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
    {
        cachedRoomList = roomList;
    }

    public void CreateRoom()
    {
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
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        string localPlayerID = PhotonNetwork.LocalPlayer.UserId; //id del jugador que s'ha unit

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player1", out object player1) && PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2)) //si la sala te els jugadors 1 i 2
        {
            if(string.IsNullOrEmpty((string)player2) && (string)player1 != localPlayerID) //si el jugador 2 es buit i el jugador 1 no es el jugador local
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
