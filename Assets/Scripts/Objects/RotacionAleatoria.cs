using UnityEngine;

public class RotacionConstante : MonoBehaviour
{
    // Velocidades de rotación para cada eje
    private Vector3 rotacionPorSegundo;

    // Factores de aleatoriedad para cada eje
    public float maxRotacionPorSegundo = 100f;

    void Start()
    {
        // Generar velocidades aleatorias para los ejes X, Y y Z
        rotacionPorSegundo = new Vector3(
            Random.Range(-maxRotacionPorSegundo, maxRotacionPorSegundo),
            Random.Range(-maxRotacionPorSegundo, maxRotacionPorSegundo),
            Random.Range(-maxRotacionPorSegundo, maxRotacionPorSegundo)
        );
    }

    void Update()
    {
        // Rotar el objeto de forma continua y fluida
        transform.Rotate(rotacionPorSegundo * Time.deltaTime);
    }
}
