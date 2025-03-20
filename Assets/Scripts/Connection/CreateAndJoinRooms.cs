using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Collections;

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
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Sala encontrada: " + room.Name);
            if(room.RemovedFromList) //si ha estat eliminada
            {
                cachedRoomList.RemoveAll(r => r.Name == room.Name); //la eliminem de la llista de sales actives
            }
            else
            {
                cachedRoomList.Add(room);
            }
        }
        
    }

    public void CreateRoom()
    {
        string roomName = createInput.text;
        string roomCode = GenerateRoomCode();
        if (roomCode == null)
        {
            Debug.LogError("No se pudo generar un código único");
            return;
        }

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable 
            { 
                { "Code", roomCode },
                { "RoomName", roomName },
                { "Level", 0 }, 
                { "Player1", PhotonNetwork.LocalPlayer.UserId }, 
                { "Player2", "" } 
            },
            CustomRoomPropertiesForLobby = new string[] { "Code", "RoomName" }
        };
        PhotonNetwork.CreateRoom(roomCode, roomOptions);
        Debug.Log("Creando sala: " + roomName + " con código: " + roomCode);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Sala creada correctamente. Estableciendo propiedades...");

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Code", PhotonNetwork.CurrentRoom.Name } });

        PhotonNetwork.LoadLevel("Waiting");
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
        Debug.Log("Tu ID: " + localPlayerID);

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player1", out object player1) &&
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2))
        {
            if (string.IsNullOrEmpty((string)player2) && (string)player1 != localPlayerID)
            {
                Debug.Log("Estableciendo jugador 2...");

                Debug.Log("Player2 antes del cambio: " + (string)player2);

                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { "Player2", localPlayerID } };

                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                StartCoroutine(WaitForPropertyUpdate());
            }
            else if ((string)player1 != localPlayerID && (string)player2 != localPlayerID)
            {
                Debug.LogError("No tienes permiso para entrar en esta sala.");
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    // 🔄 Espera y vuelve a comprobar la propiedad
    private IEnumerator WaitForPropertyUpdate()
    {
        yield return new WaitForSeconds(1f); //Esperar a que Photon propague la actualización

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2)) //Comprobar si se ha actualizado
        {
            Debug.Log("Después de 1 segundo, Player2 es: " + (string)player2);
            PhotonNetwork.LoadLevel("Waiting");

        }
        else
        {
            Debug.LogError("Error: Player2 sigue sin actualizarse.");
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) 
    {
        Debug.Log("Propiedades de la sala actualizadas");
        if (propertiesThatChanged.ContainsKey("Player2")) // Si se ha actualizado el jugador 2
        {
            ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            Debug.Log("Jugador 2: " + roomProperties["Player2"]);
            if (roomProperties.ContainsKey("Player1") && roomProperties.ContainsKey("Player2") &&
                !string.IsNullOrEmpty((string)roomProperties["Player1"]) &&
                !string.IsNullOrEmpty((string)roomProperties["Player2"]))
            {
                string roomCode = (string)roomProperties["Code"];
                string roomName = roomProperties.ContainsKey("RoomName") ? (string)roomProperties["RoomName"] : "Sala guardada";

                Debug.Log("La sala ahora tiene 2 jugadores, guardando...");
                SaveRoom(roomCode, roomName);
            }
        }
    }


    private void SaveRoom(string roomCode, string roomName)
    {
        PlayerPrefs.SetString("SavedRoomCode", roomCode);   // Guardamos el código
        PlayerPrefs.SetString("SavedRoomName", roomName);   // Guardamos el nombre de la sala
        PlayerPrefs.Save();
        Debug.Log("Última sala guardada: " + roomName + " (" + roomCode + ")");
    }

    public void LoadSavedRooms()
    {
        string savedRoomCode = PlayerPrefs.GetString("SavedRoomCode", "");
        string savedRoomName = PlayerPrefs.GetString("SavedRoomName", "Sala guardada");

        if (string.IsNullOrEmpty(savedRoomCode)) return;

        GameObject btn = Instantiate(roomButtonPrefab, savedRoomsPanel);
        btn.GetComponentInChildren<TMP_Text>().text = savedRoomName;
        btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => JoinRoomByCode(savedRoomCode));
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
}
