using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        Debug.Log("Cambiando a la escena: " + nombreEscena);
        SceneManager.LoadScene(nombreEscena);
    }
}
