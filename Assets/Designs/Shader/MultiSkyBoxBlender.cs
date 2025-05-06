using UnityEngine;
using System.Collections;

public class MultiSkyboxBlender : MonoBehaviour
{
    [Header("Skyboxes (Cubemaps)")]
    public Cubemap[] skyboxes;

    [Header("Change Points in World")]
    public Transform[] changePoints;

    [Header("Blending Settings")]
    public Material blendedSkyboxMaterial;
    public float blendDuration = 2f;
    public float triggerDistance = 5f;

    private int currentIndex = 0;
    private Transform cameraTransform;
    private bool isBlending = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        // Inicializa con el primer par de skyboxes
        if (skyboxes.Length >= 2)
        {
            blendedSkyboxMaterial.SetTexture("_Skybox1", skyboxes[0]);
            blendedSkyboxMaterial.SetTexture("_Skybox2", skyboxes[1]);
            blendedSkyboxMaterial.SetFloat("_Blend", 0f);
            RenderSettings.skybox = blendedSkyboxMaterial;
        }
    }

    void Update()
    {
        if (isBlending || currentIndex >= changePoints.Length) return;

        float distance = Vector3.Distance(cameraTransform.position, changePoints[currentIndex].position);

        if (distance < triggerDistance)
        {
            StartCoroutine(BlendToNextSkybox());
        }
    }

    IEnumerator BlendToNextSkybox()
    {
        isBlending = true;

        Cubemap fromSkybox = skyboxes[currentIndex];
        Cubemap toSkybox = skyboxes[currentIndex + 1];

        blendedSkyboxMaterial.SetTexture("_Skybox1", fromSkybox);
        blendedSkyboxMaterial.SetTexture("_Skybox2", toSkybox);

        float time = 0f;

        while (time < blendDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / blendDuration);
            blendedSkyboxMaterial.SetFloat("_Blend", t);
            yield return null;
        }

        currentIndex++;
        isBlending = false;
    }
}
