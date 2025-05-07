using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    // Parámetros de flotación
    public float floatAmplitude = 0.5f;  // Cuánto se mueve arriba/abajo
    public float floatFrequency = 1f;    // Qué tan rápido flota

    // Parámetros de rotación
    public Vector3 rotationSpeed; // Velocidad de rotación por eje

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Asignar velocidad de rotación aleatoria si no se configura manualmente
        if (rotationSpeed == Vector3.zero)
        {
            rotationSpeed = new Vector3(
                Random.Range(-30f, 30f),
                Random.Range(-30f, 30f),
                Random.Range(-30f, 30f)
            );
        }

        // También puedes aleatorizar amplitud y frecuencia si quieres más variedad
        floatAmplitude *= Random.Range(0.8f, 1.2f);
        floatFrequency *= Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        // Movimiento de flotación
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotación
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
