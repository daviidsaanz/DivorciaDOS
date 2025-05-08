using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class DialogueData
{
    public float startDelay;
    public string dialogue;
    public float readTime;
    public Color textColor; // Nueva propiedad para el color
}


public class TextDialogues : MonoBehaviourPunCallbacks
{
    public TextManager TextManager;
    public bool isAuxiliar = false; 


    [SerializeField] private List<DialogueData> dialogues = new List<DialogueData>();

    private void Start()
    {
        StartCoroutine(HandleDialogues());
    }

    private IEnumerator HandleDialogues()
    {
        foreach (DialogueData data in dialogues)
        {
            TextManager.TextRequest(data.startDelay, data.dialogue, data.readTime, data.textColor);
            yield return new WaitForSeconds(data.startDelay + data.readTime);
        }

        // Esperar un pequeño tiempo extra si quieres
        yield return new WaitForSeconds(1f);

        // Volver al nivel anterior si es auxiliar
        if (isAuxiliar)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(ResetLVL.previousScene);
            }
            else
            {
                photonView.RPC("RPC_LoadPreviousScene", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    public void RPC_LoadPreviousScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(ResetLVL.previousScene);
        }
    }

}
