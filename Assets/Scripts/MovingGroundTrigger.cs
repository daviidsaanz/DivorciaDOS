using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGroundTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToMove1;  // Primer objeto que se moverá
    [SerializeField] private GameObject objectToMove2;  // Segundo objeto que se moverá
    [SerializeField] private float moveSpeed = 2f;      // Velocidad de movimiento suave
    [SerializeField] private float moveQuantity = -7f;   // Distancia a mover en el eje Y (bajar a -7)
    private Vector3 targetPosition1;                    // Posición objetivo del primer objeto
    private Vector3 targetPosition2;                    // Posición objetivo del segundo objeto
    public bool player1OnTrigger;
    public bool player2OnTrigger;
    private bool isMoving = false;
    private bool isCoroutineRunning = false;
    private bool hasMoved = false; // Asegura que el movimiento solo ocurra una vez

    private void Start()
    {
        // Establece la posición inicial de ambos objetos
        targetPosition1 = objectToMove1.transform.position;
        targetPosition2 = objectToMove2.transform.position;
    }

    private void Update()
    {
        // Verifica si ambos jugadores están en el trigger, pero solo comienza si no se ha movido aún
        ShouldMoveGround();

        // Si el movimiento ha comenzado, mueve ambos objetos
        if (isMoving)
        {
            objectToMove1.transform.position = Vector3.Lerp(objectToMove1.transform.position, targetPosition1, moveSpeed * Time.deltaTime);
            objectToMove2.transform.position = Vector3.Lerp(objectToMove2.transform.position, targetPosition2, moveSpeed * Time.deltaTime);
        }
    }

    public void ShouldMoveGround()
    {
        // Solo activar el movimiento si ambos jugadores están en el trigger y no ha comenzado el movimiento aún
        if (player1OnTrigger && player2OnTrigger && !hasMoved && !isCoroutineRunning)
        {
            Debug.Log("Ambos jugadores en el trigger, moviendo el suelo.");
            // Iniciar la corutina para esperar 1 segundo antes de mover
            StartCoroutine(MoveGroundWithDelay());
        }
    }

    private IEnumerator MoveGroundWithDelay()
    {
        // Marcar que la corutina está corriendo
        isCoroutineRunning = true;

        // Esperar 1 segundo
        yield return new WaitForSeconds(0.1f);  // Espera de 1 segundo

        // Establecer la nueva posición objetivo para ambos objetos
        targetPosition1 = new Vector3(objectToMove1.transform.position.x, moveQuantity, objectToMove1.transform.position.z);
        targetPosition2 = new Vector3(objectToMove2.transform.position.x, moveQuantity, objectToMove2.transform.position.z);

        // Iniciar el movimiento suave
        isMoving = true;

        // Marcar que el movimiento ha ocurrido
        hasMoved = true;

        // Terminar la corutina
        isCoroutineRunning = false;
    }
}
