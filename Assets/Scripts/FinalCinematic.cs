using DG.Tweening;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class FinalCinematic : MonoBehaviourPunCallbacks
{
    public Navigable nodeP1;
    public Navigable lastnodeP1;
    public Navigable nodeP2;
    public Navigable lastnodeP2;

    private bool isP1 = false;
    private bool isP2 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            isP1 = true;
        }
        else if (other.CompareTag("Player2"))
        {
            isP2 = true;
        }

        // Solo el MasterClient lanza el RPC para todos
        if (isP1 && isP2 && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPC_StartEndingCinematic", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_StartEndingCinematic()
    {
        StartCoroutine(EndingCinematic());
    }

    private IEnumerator EndingCinematic()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

        if (player1 == null || player2 == null)
        {
            Debug.LogError("No se encontraron los jugadores en la escena.");
            yield break;
        }

        PlayerController p1Controller = player1.GetComponent<PlayerController>();
        PlayerController p2Controller = player2.GetComponent<PlayerController>();
        CameraFollowXY cameraFollow = Camera.main.GetComponent<CameraFollowXY>();

        // Desactivar controles
        if (p1Controller != null) p1Controller.isEnabled = false;
        if (p2Controller != null) p2Controller.isEnabled = false;

        // Ir al primer punto
        if (p1Controller != null) p1Controller.GoToPoint(nodeP1);
        if (p2Controller != null) p2Controller.GoToPoint(nodeP2);

        yield return new WaitForSeconds(3f);

        // Cinemática de cámara (individual para cada cliente)
        if (Camera.main != null)
        {
            Camera.main.GetComponent<Camera>().DOOrthoSize(5, 2f);
            if (cameraFollow != null)
            {
                cameraFollow.offsetCalculated = false;
                cameraFollow.transform.DOMoveY(cameraFollow.transform.position.y - 3, 2f);
            }
        }

        yield return new WaitForSeconds(3f);

        // Rotaciones de personajes (localmente, solo visual)
        player1.transform.DORotate(player1.transform.eulerAngles + new Vector3(0, 90, 0), 0.5f);
        player2.transform.DORotate(player2.transform.eulerAngles + new Vector3(0, -90, 0), 0.5f);

        yield return new WaitForSeconds(5f);

        // Movimiento final
        if (p1Controller != null) p1Controller.GoToPoint(lastnodeP1);
        if (p2Controller != null) p2Controller.GoToPoint(lastnodeP2);

        yield return new WaitForSeconds(2f);
    }
}
