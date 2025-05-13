using System.Collections;
using UnityEngine;

public class PanelBGObject : MonoBehaviour
{
    public GameObject panelObject; // Asigna el GameObject en el Inspector
    public float fadeDuration = 1.0f; // Duración del fade en segundos
    private Coroutine fadeCoroutine;
    public bool fadeInWhenPlayerEnter = true;

    private bool hasActivated = false; // Variable para asegurarse de que se activa solo una vez

    private Vector3 initialScale; // Variable para guardar la escala inicial del objeto

    private void Start()
    {
        // Aseguramos que el objeto empiece desactivado o con una escala 0
        if (panelObject != null)
        {
            panelObject.SetActive(false); // Iniciar como desactivado
            initialScale = panelObject.transform.localScale; // Guardar la escala inicial
            panelObject.transform.localScale = Vector3.zero; // Iniciar con escala 0
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasActivated) // Si ya se ha activado, no hacer nada
            return;

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

            hasActivated = true; // Marcar como activado
        }
    }

    private IEnumerator FadeIn()
    {
        if (panelObject == null)
            yield break;

        panelObject.SetActive(true); // Activar el objeto

        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = initialScale; // Usamos la escala original

        float time = 0;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            panelObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        panelObject.transform.localScale = targetScale; // Asegurarse de que esté en la escala inicial
    }

    private IEnumerator FadeOut()
    {
        if (panelObject == null)
            yield break;

        Vector3 startScale = panelObject.transform.localScale;
        Vector3 targetScale = Vector3.zero; // Escala mínima (casi invisible)

        float time = 0;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            panelObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        panelObject.transform.localScale = targetScale; // Asegurarse de que esté en escala 0
        panelObject.SetActive(false); // Desactivar el objeto cuando haya desaparecido
    }
}
