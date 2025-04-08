using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class SwiftLightController : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToLight;
    public void OnTriggerEnter(Collider other) //lo llama el GameManager
    {
        if ((other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            StartCoroutine(LightBlocks());
        }
    }

    public IEnumerator LightBlocks()
    {
        foreach (GameObject obj in objectsToLight)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }

    }

}
   
