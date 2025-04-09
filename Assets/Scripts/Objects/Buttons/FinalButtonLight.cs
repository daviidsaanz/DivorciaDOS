using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class FinalButtonLight : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject light;

    void Start()
    {
        light.SetActive(false);
        panel.SetActive(true);
    }
    public void OnTriggerEnter(Collider other) //lo llama el GameManager
    {
        if ((other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            light.SetActive (true);
            panel.SetActive (false);
        }
    }

}
