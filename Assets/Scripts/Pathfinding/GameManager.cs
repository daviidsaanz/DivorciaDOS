using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //private PlayerController player1; //comentat per testing
    //private PlayerController player2;

    public PlayerController player;

    public List<PathCondition> pathConditions = new List<PathCondition>(); //lista de condicions de camins
    public List<Transform> pivots; //pivots que es poden rotar

    public Transform[] objectsToHide; //objectes que es poden amagar

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        //esperar a que se instancien los jugadores
        //StartCoroutine(WaitForPlayers()); //comemntat per testing
    }

    /*IEnumerator WaitForPlayers()
    {
        while (player1 == null || player2 == null) // Espera hasta que ambos jugadores sean encontrados
        {
            player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<PlayerController>();
            player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<PlayerController>();
            yield return null; // Espera un frame antes de volver a comprobar
        }
    }*/



    void Update()
    {
        /*if(player1 == null || player2 == null)
        {
            return;
        }*/ //commentat per testing

        foreach (PathCondition pc in pathConditions) //per cada PathCondition que hi hagi a la llista
        {
            int count = 0;
            for (int i = 0; i < pc.conditions.Count; i++) //per cada condicio que hi hagi a la llista de condicions
            {
                if (IsRotationClose(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)
                    && Vector3.Distance(pc.conditions[i].conditionObject.localPosition, pc.conditions[i].position) < 0.01f)
                {
                    count++; // La condición se cumple
                }
            }
            foreach (SinglePath sp in pc.paths)
            {
                if (sp.index >= 0 && sp.index < sp.block.possiblePaths.Count) // Verifica que el índice sea válido
                {
                    sp.block.possiblePaths[sp.index].active = (count == pc.conditions.Count);
                }
                else
                {
                    Debug.LogError($"Índice fuera de rango: sp.index = {sp.index}, posiblesPaths.Count = {sp.block.possiblePaths.Count}");
                }
            }


        }

        /*if (player1.walking || player2.walking)
            return;*///comentat per testing

        if (player.walking)
        {
            return;
        } //comentat per testing

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                if(mouseHit.transform.CompareTag("interactuable"))
                {
                    Interactuable interactuable = mouseHit.transform.GetComponentInParent<Interactuable>();

                    if (interactuable != null)
                    {
                        interactuable.Interact(); //crudem a la funcio Interact de Interactuable
                    }
                }
            }
        }

        foreach (Transform t in objectsToHide)
        {
            t.gameObject.SetActive(pivots[0].eulerAngles.y > 45 && pivots[0].eulerAngles.y < 90 + 45);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

    }

    private bool IsRotationClose(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(Mathf.DeltaAngle(a.x, b.x)) < 1f &&
               Mathf.Abs(Mathf.DeltaAngle(a.y, b.y)) < 1f &&
               Mathf.Abs(Mathf.DeltaAngle(a.z, b.z)) < 1f;
    }

    public void RotateRightPivot() //ejemplo de rotacion de algun pivote al presionar un boton
    {
        pivots[1].DOComplete();
        pivots[1].DORotate(new Vector3(0, 0, 90), .6f).SetEase(Ease.OutBack); //podemos hacerlo asi o dandole el componente interactuable y llamando a la funcion Interact
    }

    /*public void ButtonPressed(ButtonPressed button) ARA MATEIX HO FEM DESDE EL BUTTONPRESSED
    {
        if(button.disabled)
        {
            return;
        }
        else 
        {
            button.SetMovmentAndRotation(); //fem el set a les variables de l'objecte que volem moure (depenent de cada boto)
            Debug.Log("Button pressed");
            button.objectToMove.GetComponent<Interactuable>().Interact(); //cridem a la funcio Interact de Interactuable
        }
    }*/

}

[System.Serializable]
public class PathCondition
{
    public string pathConditionName;
    public List<Condition> conditions;
    public List<SinglePath> paths;
}
[System.Serializable]
public class Condition
{
    public Transform conditionObject;
    public Vector3 eulerAngle;
    public Vector3 position;


}
[System.Serializable]
public class SinglePath
{
    public Navigable block;
    public int index;
}
