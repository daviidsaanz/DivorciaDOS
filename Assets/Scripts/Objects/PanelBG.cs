using System.Collections;
using UnityEngine;

public class PanelBG : MonoBehaviour
{
    public Material panelMaterial; // Asigna el material en el Inspector
    public float fadeDuration = 1.0f; // Duración del fade en segundos
    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        if (panelMaterial == null)
            yield break;

        Color color = panelMaterial.color;
        float startAlpha = color.a;
        float targetAlpha = 1.0f; // Totalmente visible
        float time = 0;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            panelMaterial.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        panelMaterial.color = color;
    }
}
