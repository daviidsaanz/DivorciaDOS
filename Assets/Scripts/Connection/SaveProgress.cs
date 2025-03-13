using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class SaveProgress : MonoBehaviourPunCallbacks
{
    private int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("SavedLevel", 0); // Carga el nivel guardado
    }

    public void CompleteLevel()
    {
        currentLevel++; // Sube al siguiente nivel
        PlayerPrefs.SetInt("SavedLevel", currentLevel);
        PlayerPrefs.Save();

        // Actualizar el nivel en la sala de Photon
        Hashtable newProperties = new Hashtable { { "SavedLevel", currentLevel } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(newProperties);

        Debug.Log("Nivel actualizado a: " + currentLevel);
    }
}
