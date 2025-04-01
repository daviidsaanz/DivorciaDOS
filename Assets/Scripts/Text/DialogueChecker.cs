using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueChecker : MonoBehaviour
{
    public TextManager textManager;

    private CanvasGroup canvasGroup; // For fade effect on the first canvas
    public CanvasGroup secondCanvasGroup; // Reference to the second canvas's CanvasGroup
    private bool isFading = false;   // Track if fading is in progress
    public float fadeDuration = 2f;  // Duration of the fade effect
    public float delayBetweenFades = 2f; // Delay between fade-in and fade-out
    public float delayBeforeFadeIn = 0.5f; // Delay before the first canvas fades in

    void Start()
    {
        // Ensure the GameObject is active
        gameObject.SetActive(true);

        // Get or add a CanvasGroup component for the fade effect on the first canvas
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.Log("Adding CanvasGroup component to " + gameObject.name);
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Set initial alpha to 0 (fully transparent) for the first canvas
        canvasGroup.alpha = 0f;

        // Ensure the second canvas's CanvasGroup is assigned
        if (secondCanvasGroup == null)
        {
            Debug.LogError("Second CanvasGroup is not assigned. Please assign it in the Inspector.");
        }
        else
        {
            // Set initial alpha to 1 for the second canvas (visible at the start)
            secondCanvasGroup.alpha = 1f;
        }

        Debug.Log("DialogueChecker initialized. First canvas is hidden, second canvas is visible.");
    }

    void Update()
    {
        // Check if the dialogue is finished
        if (textManager != null && textManager.isDialogueFinished && !isFading)
        {
            Debug.Log("Dialogue has finished! Starting fade-in for the first canvas.");

            // Start the fade-in effect for the first canvas
            StartCoroutine(FadeInAndOut());

            // Reset the flag if needed for repeated checks
            textManager.isDialogueFinished = false;
        }
    }

    private IEnumerator FadeInAndOut()
    {
        isFading = true; // Mark fading as in progress

        // Wait for the specified delay before starting the fade-in
        yield return new WaitForSeconds(delayBeforeFadeIn);

        // Fade in the first canvas
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, fadeDuration));

        Debug.Log("Fade-in complete for the first canvas. Waiting for " + delayBetweenFades + " seconds before fading out.");

        // Wait for the specified delay
        yield return new WaitForSeconds(delayBetweenFades);

        // Fade out the first canvas
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, fadeDuration));

        Debug.Log("Fade-out complete for the first canvas. Starting fade-out for the second canvas.");

        // Fade out the second canvas
        yield return StartCoroutine(FadeCanvasGroup(secondCanvasGroup, 1f, 0f, fadeDuration));

        Debug.Log("Fade-out complete for both canvases. Both are now hidden.");

        isFading = false; // Mark fading as complete
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
       /* PlayerController p1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<PlayerController>();
        PlayerController p2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<PlayerController>();
        p1.isEnabled = true;
        p2.isEnabled = true;*/

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value based on elapsed time
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is set to the end value at the end
        group.alpha = endAlpha;

    }
}