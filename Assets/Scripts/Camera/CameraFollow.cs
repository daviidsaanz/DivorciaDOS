using System.Collections;
using UnityEngine;

public class CameraFollowXY : MonoBehaviour
{
    private Transform player1;
    private Transform player2;

    public float smoothSpeed = 0.125f;

    private float xOffsetFromMiddle;
    private float yOffsetFromMiddle;

    private bool offsetCalculated = false;

    void Start()
    {
        StartCoroutine(FindPlayersAndSetOffset());
    }

    void LateUpdate()
    {
        if (!offsetCalculated) return;

        // Punto medio en X y Y
        Vector3 middlePosition = (player1.position + player2.position) / 2f;

        // Posición deseada, calculando los offsets en X y Y
        float targetX = middlePosition.x + xOffsetFromMiddle;
        float targetY = middlePosition.y + yOffsetFromMiddle;

        // Interpolación suave en X y Y
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed);
        float smoothedY = Mathf.Lerp(transform.position.y, targetY, smoothSpeed);

        // Aplicar nueva posición con Z original
        transform.position = new Vector3(smoothedX, smoothedY, transform.position.z);
    }

    private IEnumerator FindPlayersAndSetOffset()
    {
        // Esperar hasta que ambos jugadores estén disponibles
        while (player1 == null || player2 == null)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1")?.transform;
            player2 = GameObject.FindGameObjectWithTag("Player2")?.transform;

            yield return new WaitForSeconds(0.1f);
        }

        // Calcular el punto medio inicial y el offset desde la posición actual de la cámara
        Vector3 middlePosition = (player1.position + player2.position) / 2f;

        xOffsetFromMiddle = transform.position.x - middlePosition.x;
        yOffsetFromMiddle = transform.position.y - middlePosition.y;

        offsetCalculated = true;
    }
}
