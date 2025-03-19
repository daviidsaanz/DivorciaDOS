using UnityEngine;
using EZCameraShake;

public class CameraShakeController : MonoBehaviour
{
    // Referencia al CameraShaker
    private CameraShaker cameraShaker;

    void Start()
    {
        // Obtener el CameraShaker en la escena
        cameraShaker = CameraShaker.Instance;
    }

    void Update()
    {
        // Detectar cuando se presiona la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Iniciar el shake de tipo "Earthquake" al presionar espacio
            if (cameraShaker != null)
            {
                CameraShakeInstance earthquakeShake = CameraShakePresets.Earthquake;
                cameraShaker.Shake(earthquakeShake);
            }
        }
    }
}
