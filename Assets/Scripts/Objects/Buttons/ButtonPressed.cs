using System.Collections;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    private Transform buttonTransform;
    private Vector3 originalPosition;
    public Vector3 pressedPositionOffset = new Vector3(0f, -0.05f, 0f); 
    private bool pressed = false;
    public float pressDuration = 0.1f;
    public GameObject objectToMove;

    void Start()
    {
        buttonTransform = GetComponent<Transform>();
        originalPosition = buttonTransform.localPosition;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !pressed)
        {
            OnButtonPress();
        }
    }

    public void OnButtonPress()
    {
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPosition + pressedPositionOffset));
    }

    private IEnumerator MoveButton(Vector3 targetPosition)
    {
        Vector3 startPosition = buttonTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < pressDuration)
        {
            buttonTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / pressDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pressed = true;
        buttonTransform.localPosition = targetPosition;  
    }

    /* public void OnButtonRelease()
    {
        StopAllCoroutines();  
        StartCoroutine(MoveButton(originalPosition));
    } */
}
