using UnityEngine;

public class CameraFollowXY : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float smoothSpeed = 0.125f;

    private float xOffsetFromMiddle;
    private float yOffsetFromMiddle;

    void Start()
    {
        if (player1 == null || player2 == null) return;

        // Calcular el punto medio en X y Y
        Vector3 middlePosition = (player1.position + player2.position) / 2f;

        // Calcular el offset inicial de la cámara respecto al punto medio
        xOffsetFromMiddle = transform.position.x - middlePosition.x;
        yOffsetFromMiddle = transform.position.y - middlePosition.y;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

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
}
