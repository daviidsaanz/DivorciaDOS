using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_Text playWithFriendText; // Boto de "Jugar amb X"
    public GameObject invitePanel; // Panel de invitació per acceptar o rebutjar
    public TMP_Text inviteText; // Text de la invitació

    private string savedPartnerID;
    private string savedPartnerName;
    private int savedLevel;

    private void Start()
    {
        savedPartnerID = PlayerPrefs.GetString("LastPartnerID", ""); //agafa el id del segon jugador
        savedPartnerName = PlayerPrefs.GetString("LastPartnerName", ""); //guarda el nom del segon jugador
        savedLevel = PlayerPrefs.GetInt("LastLevel", 0); //guarda el nivell on es va quedar

        if(!string.IsNullOrEmpty(savedPartnerID)) //si no esta buit
        {
            playWithFriendText.text = "Play with " + savedPartnerName; //mostra el nom del segon jugador
            playWithFriendText.transform.parent.gameObject.SetActive(true); //activa el boto
        }
    }

    public void InviteFriend()
    {
        int targetActor = GetPlayerActorNumber(savedPartnerID); //guarda el numero d'actor del segon jugador
        if(targetActor != -1)
        {
            object[] data = new object[] { PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.LocalPlayer.NickName }; //guarda el id i el nom del jugador local
            PhotonNetwork.RaiseEvent(1, data, new RaiseEventOptions { TargetActors = new int[] { targetActor } }, SendOptions.SendReliable); //envia un event amb el id i el nom del jugador local al segon jugador
        }
    }

    public void AcceptInvite()
    {
        string lastRoom = PlayerPrefs.GetString("LastRoomName", ""); //agafa el nom de la ultima sala
        if(!string.IsNullOrEmpty(lastRoom)) //si no esta buit
        {
            PhotonNetwork.JoinRoom(lastRoom); //entra a la sala
        }
        invitePanel.SetActive(false); //tanca el panell de invitacio
    }

    public void RejectInvite()
    {
        invitePanel.SetActive(false); //tanca el panell de invitacio
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 }; //crea una sala amb 2 jugadors

        Hashtable roomProperties = new Hashtable(); //crea una taula de propietats
        roomProperties["FirstPlayerID"] = PhotonNetwork.LocalPlayer.UserId; //guarda el id del primer jugador
        roomProperties["SavedLevel"] = savedLevel; //guarda el nivell on es va quedar   

        roomOptions.CustomRoomProperties = roomProperties; //guarda les propietats a la sala
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "FirstPlayerID", "SavedLevel" }; //mostra les propietats a la llista de sales

        PhotonNetwork.CreateRoom(createInput.text, roomOptions); //crea la sala amb el nom introduit
        PlayerPrefs.SetString("LastRoomName", createInput.text); //guarda el nom de la sala
        PlayerPrefs.Save();
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
        string firstPlayerID = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("FirstPlayerID") ? PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerID"].ToString() : ""; //guarda el id del primer jugador, si no hi ha, guarda un string buit
        
        int roomLevel = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SavedLevel") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["SavedLevel"] : 0; //guarda el nivell on es va quedar, si no hi ha, guarda 0

        if(!string.IsNullOrEmpty(firstPlayerID) && firstPlayerID != PhotonNetwork.LocalPlayer.UserId) //si no esta buit i el id del primer jugador no es igual al del jugador local
        {
            PlayerPrefs.SetString("LastPartnerID", firstPlayerID); //guarda el id del segon jugador
            PlayerPrefs.SetString("LastPartnerName", PhotonNetwork.CurrentRoom.Name); //guarda el nom del segon jugador
            PlayerPrefs.SetInt("LastLevel", roomLevel); //guarda el nivell on es va quedar
            PlayerPrefs.Save();
        }

        int lastLevel = PlayerPrefs.GetInt("LastLevel", 0); //agafa el nivell on es va quedar
        PhotonNetwork.LoadLevel("Level" + lastLevel); //carrega el nivell on es va quedar
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
        if(photonEvent.Code == 1) //si el codi de l'event es 1
        {
            object[] data = (object[])photonEvent.CustomData; //guarda les dades de l'event
            string inviterID = (string)data[0]; //guarda el id del jugador que envia la invitacio
            string inviterName = (string)data[1]; //guarda el nom del jugador que envia la invitacio

            inviteText.text = inviterName + " wants to play with you"; //mostra el nom del jugador que envia la invitacio
            invitePanel.SetActive(true); //activa el panell de invitacio
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
