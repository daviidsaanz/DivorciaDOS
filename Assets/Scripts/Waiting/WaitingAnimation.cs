using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaitingAnimation : MonoBehaviour
{
    private PlayerController player1;
    private PlayerController player2;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    [SerializeField] private Navigable[] nodes;

    public void AnimationBothPlayers()
    {
        StartCoroutine(WaitForPlayersAndAnimate());
    }

    private IEnumerator WaitForPlayersAndAnimate()
    {
        // Esperar hasta que los jugadores sean encontrados
        while (player1 == null || player2 == null)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<PlayerController>();
            player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<PlayerController>();
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Ambos jugadores encontrados. Iniciando animación...");

        // Llamar a un RPC para que cada jugador ejecute su propia animación
        photonView.RPC("StartPlayerAnimation", RpcTarget.All);
    }

    [PunRPC]
    private void StartPlayerAnimation()
    {
        // Cada jugador mueve su propio personaje
        if (player1.photonView.IsMine)
        {
            Debug.Log("Jugador 1 moviéndose...");
            player1.GoToPoint(nodes[0]);
        }
        else if (player2.photonView.IsMine)
        {
            Debug.Log("Jugador 2 moviéndose...");
            player2.GoToPoint(nodes[1]);
        }
    }


}
