using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPointP1 : MonoBehaviour
{
    public bool player1IsOnPoint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1IsOnPoint = true;
            Debug.Log("Player 1 ha llegado al punto.");

            if (FinalPointController.instance != null)
            {
                FinalPointController.instance.CheckIfBothPlayersReachedCheckpoint();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1IsOnPoint = false;
            Debug.Log("Player 1 ha salido del punto.");
        }
    }
}
