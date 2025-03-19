using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections.Generic;
/*
public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public Transform savedRoomsPanel;
    public GameObject roomButtonPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>(); // Lista de salas activas

    private void Start()
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

            if (!RoomExists(code)) return code; // Si el código es único, lo usamos
        }
        return null; // Si falla después de 10 intentos
    }

    private bool RoomExists(string code)
    {
        foreach (RoomInfo room in cachedRoomList)
        {
            if (room.CustomProperties.ContainsKey("Code") && (string)room.CustomProperties["Code"] == code)
            {
                return true; // El código ya existe
            }
        }
        return false;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList; // Guardamos la lista de salas activas
    }

    public void CreateRoom()
    {
        string roomCode = GenerateRoomCode();
        if (roomCode == null)
        {
            Debug.LogError("No se pudo generar un código único");
            return;
        }

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = new Hashtable
            {
                { "Code", roomCode },
                { "Level", 0 },
                { "Player1", PhotonNetwork.LocalPlayer.UserId }, // Jugador 1
                { "Player2", "" } // Jugador 2 aún vacío
            },
            CustomRoomPropertiesForLobby = new string[] { "Code" }
        };
        PhotonNetwork.CreateRoom(roomCode, roomOptions);
    }

    public void JoinRoom()
    {
        string inputCode = joinInput.text;
        foreach (RoomInfo room in cachedRoomList)
        {
            if (room.CustomProperties.ContainsKey("Code") && (string)room.CustomProperties["Code"] == inputCode)
            {
                PhotonNetwork.JoinRoom(room.Name);
                return;
            }
        }
        Debug.Log("No se encontró una sala con ese código.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Te has unido a la sala: " + PhotonNetwork.CurrentRoom.Name);
        string localPlayerID = PhotonNetwork.LocalPlayer.UserId;

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player1", out object player1) &&
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2))
        {
            if (string.IsNullOrEmpty((string)player2) && (string)player1 != localPlayerID)
            {
                // Si hay espacio y el jugador es el segundo en entrar
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "Player2", localPlayerID } });
            }
            if (!string.IsNullOrEmpty((string)player1) && !string.IsNullOrEmpty((string)player2))
            {
                SaveRoom((string)PhotonNetwork.CurrentRoom.CustomProperties["Code"]);
            }
            else if ((string)player1 != localPlayerID && (string)player2 != localPlayerID)
            {
                Debug.LogError("No tienes permiso para entrar en esta sala.");
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    private void SaveRoom(string roomCode)
    {
        List<string> savedRooms = new List<string>(PlayerPrefs.GetString("SavedRooms", "").Split(','));
        if (!savedRooms.Contains(roomCode))
        {
            savedRooms.Add(roomCode);
            PlayerPrefs.SetString("SavedRooms", string.Join(",", savedRooms));
            PlayerPrefs.Save();
        }
    }

    public void LoadSavedRooms()
    {
        string savedRoomsStr = PlayerPrefs.GetString("SavedRooms", "");
        if (string.IsNullOrEmpty(savedRoomsStr)) return;

        string[] savedRooms = savedRoomsStr.Split(',');
        foreach (string roomCode in savedRooms)
        {
            GameObject btn = Instantiate(roomButtonPrefab, savedRoomsPanel);
            btn.GetComponentInChildren<TMP_Text>().text = roomCode;
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => JoinRoomByCode(roomCode));
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al unirse a la sala: " + message);
    }

    private void JoinRoomByCode(string code)
    {
        joinInput.text = code;
        JoinRoom();
    }
}*/
