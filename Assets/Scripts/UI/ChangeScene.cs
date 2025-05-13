using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject movingObject;
    [SerializeField] private GameObject panelUI;
    [SerializeField] private float movmentUI;
    [SerializeField] private float movmentObj;


    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Loading")
        {
            Debug.Log("Cambiando a la escena: Lobby");
            StartCoroutine(TransitionOfScene("Lobby", false));
        }
        else if (SceneManager.GetActiveScene().name == "Lobby")
        {
            StartCoroutine(TransitionOfScene("aaa", false));
        }
    }


    public void CambiarEscena(string nombreEscena)
    {
        Debug.Log("Cambiando a la escena: " + nombreEscena);
        StartCoroutine(TransitionOfScene(nombreEscena, true));
    }

    public IEnumerator TransitionOfScene(string nombreEscena, bool changeScene)
    {
        float duration = 2f;
        float elapsed = 0f;

        RectTransform rect = panelUI.GetComponent<RectTransform>();
        Vector2 startPanelPos = rect.anchoredPosition;
        Vector2 endPanelPos = startPanelPos + new Vector2(0, movmentUI);

      
        Vector3 startObjPos = movingObject.transform.position;
        Vector3 endObjPos = startObjPos + new Vector3(0, movmentObj, 0); 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            rect.anchoredPosition = Vector2.Lerp(startPanelPos, endPanelPos, t);

            movingObject.transform.position = Vector3.Lerp(startObjPos, endObjPos, t);

            yield return null;
        }

        rect.anchoredPosition = endPanelPos;
        movingObject.transform.position = endObjPos;

        if(changeScene)
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }

    /*public IEnumerator TransitionOfSceneWithParameters(string nombreEscena, bool changeScene, float movmentAmountUI, float movmentAmountObj)
    {
        float duration = 2f;
        float elapsed = 0f;

        RectTransform rect = panelUI.GetComponent<RectTransform>();
        Vector2 startPanelPos = rect.anchoredPosition;
        Vector2 endPanelPos = startPanelPos + new Vector2(0, movmentAmountUI);


        Vector3 startObjPos = movingObject.transform.position;
        Vector3 endObjPos = startObjPos + new Vector3(0, movmentAmountObj, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            rect.anchoredPosition = Vector2.Lerp(startPanelPos, endPanelPos, t);

            movingObject.transform.position = Vector3.Lerp(startObjPos, endObjPos, t);

            yield return null;
        }

        rect.anchoredPosition = endPanelPos;
        movingObject.transform.position = endObjPos;

        if (changeScene)
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }*/
}
