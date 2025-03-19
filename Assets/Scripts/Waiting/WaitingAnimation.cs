using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaitingAnimation : MonoBehaviour
{
    private PlayerController player1;
    private PlayerController player2;

    [SerializeField] private Navigable[] nodes;

    public void AnimationPlayer1()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<PlayerController>();
        player1.isEnabled = false;
        player1.GoToPoint(nodes[0]);
        player1.isEnabled = true;
    }

    public void AnimationPlayer2()
    {
        player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<PlayerController>();
        player2.isEnabled = false;
        player1.GoToPoint(nodes[1]);
        player2.isEnabled = true;
    }

}
