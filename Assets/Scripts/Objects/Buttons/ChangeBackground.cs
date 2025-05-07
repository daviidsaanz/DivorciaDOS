using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{

    [SerializeField] GameObject planeToActivate;
    [SerializeField] GameObject planeToDeactivate;


    public void OnTriggerEnter(Collider other) 
    {
        if ((other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            planeToActivate.SetActive(true);
            planeToDeactivate.SetActive(false);
        }
    }
}
