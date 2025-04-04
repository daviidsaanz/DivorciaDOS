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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(); //ens unim al lobby per rebre les sales disponibles de la llista de photon
        LoadSavedRooms(); //carregar la sala guardada
    }

    private string GenerateRoomCode()
    {
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int i = 0; i < 10; i++) //tindra 10 intents per generar un codi unic
        {
            string numbers = Random.Range(100, 999).ToString(); //genera un numero aleatori de 3 xifres
            string letter = letters[Random.Range(0, letters.Length)].ToString(); //genera una lletra aleatoria
            string code = numbers + letter; //genera el codi amb el numero i la lletra

            if (!RoomExists(code)) return code; //si el codi no existeix ja, retorna el codi
        }
        return null; //si falla 10 cops, retorna null
    }

    private bool RoomExists(string code) //comprovar si la room existeix
    {
        foreach (RoomInfo room in cachedRoomList) //per cada sala de la llista de sales actives
        {
            if (room.CustomProperties.ContainsKey("Code") && (string)room.CustomProperties["Code"] == code) //si el codi de la sala es igual al codi que volem comprovar
            {
                return true; //retornem que ja existeix
            }
        }
        return false; //si no existeix, retornem que no existeix
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) //quan la llista de sales s'actualitza
    {
        foreach (RoomInfo room in roomList) //per cada room de la llista de sales
        {
            Debug.Log("Sala encontrada: " + room.Name);
            if(room.RemovedFromList) //si ha estat eliminada
            {
                cachedRoomList.RemoveAll(r => r.Name == room.Name); //la eliminem de la llista de sales actives
            }
            else
            {
                cachedRoomList.Add(room); //si no ha estat eliminada, l'afegim a la llista de sales actives
            }
        }
        
    }

    public void CreateRoom() //crea una sala amb el nom introduit i un codi generat
    {
        string roomName = createInput.text; //nom de la sala
        string roomCode = GenerateRoomCode(); //genera un codi unic per la sala
        if (roomCode == null) //si la funcio retorna null, fem return
        {
            Debug.LogError("No se pudo generar un código único");
            return;
        }

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable  //propietats personalitzades de la sala
            { 
                { "Code", roomCode }, //codi de la sala
                { "RoomName", roomName }, //nom de la sala
                { "Level", 1 }, //nivell de la sala
                { "Player1", PhotonNetwork.LocalPlayer.UserId }, //jugador 1 sera l'usuari local
                { "Player2", "" } //jugador 2 sera buit (de moment)
            },
            CustomRoomPropertiesForLobby = new string[] { "Code", "RoomName", "Level" } //propietats de la sala que es mostraran al lobby
        };
        PhotonNetwork.CreateRoom(roomCode, roomOptions); //crea la sala amb el codi generat i les opcions de la sala
        Debug.Log("Creando sala: " + roomName + " con código: " + roomCode);
    }

    public override void OnCreatedRoom() //s'executa quan la sala s'ha creat correctament
    {
        Debug.Log("Sala creada correctamente. Estableciendo propiedades...");

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Code", PhotonNetwork.CurrentRoom.Name } }); //estableix les propietats de la sala

        //posem algo de esperant a que player2 s'uneixi
    }

    public void JoinRoom() //funcio per unir-se a una sala amb el codi introduit
    {
        string inputCode = joinInput.text; //codi introduit
        foreach (RoomInfo room in cachedRoomList) //per cada sala de la llista de sales actives
        {
            if (room.CustomProperties.ContainsKey("Code") && (string)room.CustomProperties["Code"] == inputCode) //si el codi de la sala es el mateix que el codi introduit
            {
                PhotonNetwork.JoinRoom(room.Name); //cridem a la funcio per unir-se a la sala amb el nom de la sala
                return;
            }
        }
        Debug.Log("No se encontró una sala con ese código.");
    }

    public override void OnJoinedRoom() //s'executa quan ens unim a una sala
    {
        Debug.Log("Te has unido a la sala: " + PhotonNetwork.CurrentRoom.Name);

        string localPlayerID = PhotonNetwork.LocalPlayer.UserId;
        Debug.Log("Tu ID: " + localPlayerID);

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player1", out object player1) &&
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2)) //comprovar si hi ha jugadors a la sala
        {
            if (string.IsNullOrEmpty((string)player2) && (string)player1 != localPlayerID) //si el jugador 2 esta buit i el jugador 1 no es l'usuari local (ja esta dins)
            {
                Debug.Log("Estableciendo jugador 2...");

                Debug.Log("Player2 antes del cambio: " + (string)player2);

                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { "Player2", localPlayerID } }; //posem el jugador 2 per l'usuari local (el que s'uneix)

                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                StartCoroutine(WaitForPropertyUpdate()); //fem la corrutina per esperar a que es propagui la actualització de la sala (avegades tarda 2 segons o aixi)
            }
            else if ((string)player1 != localPlayerID && (string)player2 != localPlayerID) //si el jugador 1 i el jugador 2 no son l'usuari local (no tenim permis per entrar)
            {
                Debug.LogError("No tienes permiso para entrar en esta sala.");
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    private IEnumerator WaitForPropertyUpdate() //esperar a que es propagui la actualització de la sala
    {
        yield return new WaitForSeconds(1f); //espera a que photon propagui la actualització de la sala

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Player2", out object player2)) //comprovar si s'ha actualitzat el jugador 2
        {
            Debug.Log("Después de 1 segundo, Player2 es: " + (string)player2);
            //PhotonNetwork.LoadLevel("Waiting");

        }
        else
        {
            Debug.LogError("Error: Player2 sigue sin actualizarse.");
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)  //s'executa quan les propietats de la sala s'actualitzen
    {
        Debug.Log("Propiedades de la sala actualizadas");
        if (propertiesThatChanged.ContainsKey("Player2")) //si s'ha actualitzat el jugador 2
        {
            ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            Debug.Log("Jugador 2: " + roomProperties["Player2"]);
            if (roomProperties.ContainsKey("Player1") && roomProperties.ContainsKey("Player2") &&
                !string.IsNullOrEmpty((string)roomProperties["Player1"]) &&
                !string.IsNullOrEmpty((string)roomProperties["Player2"])) //si hi ha 2 jugadors a la sala
            {
                string roomCode = (string)roomProperties["Code"]; //agafem el codi de la sala
                string roomName = roomProperties.ContainsKey("RoomName") ? (string)roomProperties["RoomName"] : "Sala guardada"; //agafem el nom de la sala, si no hi ha, posem "Sala guardada"

                Debug.Log("La sala ahora tiene 2 jugadores, guardando...");
                SaveRoom(roomCode, roomName); //cridem a la funció per guardar la sala
                PhotonNetwork.LoadLevel("LVL" + (int)roomProperties["Level"]);
            }
        }
    }


    private void SaveRoom(string roomCode, string roomName) //funcio per guardar la sala
    {
        PlayerPrefs.SetString("SavedRoomCode", roomCode); //guardem el codi de la sala
        PlayerPrefs.SetString("SavedRoomName", roomName); //guardem el nom de la sala
        PlayerPrefs.Save();
        Debug.Log("Última sala guardada: " + roomName + " (" + roomCode + ")");
    }

    public void LoadSavedRooms() //carrega la sala guardada
    {
        string savedRoomCode = PlayerPrefs.GetString("SavedRoomCode", ""); //agafa el codi de la sala guardada
        string savedRoomName = PlayerPrefs.GetString("SavedRoomName", "Sala guardada"); //agafa el nom de la sala guardada

        if (string.IsNullOrEmpty(savedRoomCode)) return; //si no hi ha cap sala guardada, no fem res

        GameObject btn = Instantiate(roomButtonPrefab, savedRoomsPanel); //si hi ha una sala guardada, creem un botó per unir-s'hi
        btn.GetComponentInChildren<TMP_Text>().text = savedRoomName; //posem el nom de la sala al botó
        btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => JoinRoomByCode(savedRoomCode)); //quan es clica el botó, es crida a la funció JoinRoomByCode amb el codi de la sala guardada
    }

    public void JoinOrCreateSavedRoom() //funcio per unir-se o crear una sala amb el codi guardat (la genera amb els parametres guardats)
    {
        string savedRoomCode = PlayerPrefs.GetString("SavedRoomCode", ""); //agafa el codi de la sala guardada

        if (string.IsNullOrEmpty(savedRoomCode)) //si no hi ha cap sala guardada, no fem res (comprovem si el codi de la sala guardada és buit)
        {
            Debug.LogError("No hay sala guardada.");
            return;
        }

        string roomName = PlayerPrefs.GetString("SavedRoomName", "Sala guardada"); //si hi ha una sala guardada, agafem el nom de la sala guardada, default "Sala guardada"
        int level = PlayerPrefs.GetInt("SavedRoomLevel", 1); //agafem el nivell de la sala guardada, default 1

        RoomOptions roomOptions = new RoomOptions //creem les opcions de la sala amb els parametres guardats
        {
            MaxPlayers = 2,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Code", savedRoomCode }, //el codi sera el codi de la sala guardada
            { "RoomName", roomName }, //el nom sera el nom de la sala guardada
            { "Level", level }, //el nivell sera el nivell de la sala guardada
            { "Player1", PhotonNetwork.LocalPlayer.UserId }, //el jugador 1 sera l'usuari local
            { "Player2", "" } //el jugador 2 sera buit (de moment)
        },
            CustomRoomPropertiesForLobby = new string[] { "Code", "RoomName", "Level" } //les propietats de la sala que es mostraran al lobby
        };

        PhotonNetwork.JoinOrCreateRoom(savedRoomCode, roomOptions, TypedLobby.Default); //intentem unir-nos o crear la sala amb els parametres guardats
        Debug.Log("Intentando unirse o crear la sala guardada: " + savedRoomCode);

    }


    private void JoinRoomByCode(string code) //funcio per unir-se a una sala amb un codi concret guardat
    {
        joinInput.text = code; //posem el codi de la sala al input
        JoinOrCreateSavedRoom(); //cridem a la funció JoinOrCreateSavedRoom que intenta unir-se a la sala amb el codi guardat
    }
}
