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
    public void Shake()
    {
        if (cameraShaker != null)
        {
            CameraShakeInstance earthquakeShake = CameraShakePresets.Earthquake;
            cameraShaker.Shake(earthquakeShake);
        }
    }
}
