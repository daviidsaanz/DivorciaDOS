using UnityEngine;

public class CameraFollowXOnlyStable : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float smoothSpeed = 0.125f;

    private float xOffsetFromMiddle;

    void Start()
    {
        if (player1 == null || player2 == null) return;

        // Calcular el punto medio en X
        float middleX = (player1.position.x + player2.position.x) / 2f;

        // Calcular offset inicial de la cámara respecto al punto medio
        xOffsetFromMiddle = transform.position.x - middleX;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        // Punto medio en X
        float middleX = (player1.position.x + player2.position.x) / 2f;

        // Posición deseada solo en X
        float targetX = middleX + xOffsetFromMiddle;

        // Interpolación suave en X
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed);

        // Aplicar nueva posición con Y y Z originales
        transform.position = new Vector3(smoothedX, transform.position.y, transform.position.z);
    }
}
