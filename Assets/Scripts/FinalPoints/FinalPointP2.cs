using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPointP2 : MonoBehaviour
{
    public bool player2IsOnPoint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            player2IsOnPoint = true;
            Debug.Log("Player 2 ha llegado al punto.");

            if (FinalPointController.instance != null)
            {
                FinalPointController.instance.CheckIfBothPlayersReachedCheckpoint();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            player2IsOnPoint = false;
            Debug.Log("Player 2 ha salido del punto.");
        }
    }
}
