using System.Collections;
using UnityEngine;

public class PanelBG : MonoBehaviour
{
    public Material panelMaterial; // Asigna el material en el Inspector
    public float fadeDuration = 1.0f; // Duración del fade en segundos
    private Coroutine fadeCoroutine;
    public bool fadeInWhenPlayerEnter = true;

    private void Start()
    {
        // Aseguramos que el alpha inicial sea 0
        if (panelMaterial != null)
        {
            Color color = panelMaterial.color;
            color.a = 0f; // Establecer alpha a 0
            panelMaterial.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            if (fadeInWhenPlayerEnter)
            {
                fadeCoroutine = StartCoroutine(FadeIn());
            }
            else
            {
                fadeCoroutine = StartCoroutine(FadeOut());
            }
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

    private IEnumerator FadeOut()
    {
        if (panelMaterial == null)
            yield break;

        Color color = panelMaterial.color;
        float startAlpha = color.a;
        float targetAlpha = 0.0f; // Totalmente invisible
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
