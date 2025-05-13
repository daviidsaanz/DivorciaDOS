using System.Collections;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject object1; // Asigna el primer GameObject en el Inspector
    public GameObject object2; // Asigna el segundo GameObject en el Inspector
    public float interval = 1.0f; // Intervalo en segundos entre activaciones

    private void Start()
    {
        // Aseguramos que ambos objetos estén desactivados al inicio
        if (object1 != null)
            object1.SetActive(false);
        if (object2 != null)
            object2.SetActive(false);

        // Iniciamos la coroutine para alternar activaciones
        StartCoroutine(ToggleObjectsCoroutine());
    }

    private IEnumerator ToggleObjectsCoroutine()
    {
        while (true) // Bucle infinito para alternar activaciones
        {
            // Alternamos el estado de los objetos
            if (object1 != null && object2 != null)
            {
                // Activamos el primer objeto y desactivamos el segundo
                object1.SetActive(true);
                object2.SetActive(false);
            }

            // Esperamos el intervalo antes de cambiar
            yield return new WaitForSeconds(interval);

            if (object1 != null && object2 != null)
            {
                // Activamos el segundo objeto y desactivamos el primero
                object1.SetActive(false);
                object2.SetActive(true);
            }

            // Esperamos el intervalo antes de cambiar nuevamente
            yield return new WaitForSeconds(interval);
        }
    }
}
