using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Animator objectToMove;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectToMove.SetBool("move", true);
        }
    }
}
