using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
        yield return new WaitForSeconds(1);
        player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
        player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();
    }*/ //comentat per testing


    void Update()
    {
        /*if(player1 == null || player2 == null)
        {
            return;
        }*/ //commentat per testing

        foreach (PathCondition pc in pathConditions) //per cada condicio de cami
        {
            int count = 0;
            for (int i = 0; i < pc.conditions.Count; i++)
            {
                if (pc.conditions[i].conditionObject.eulerAngles == pc.conditions[i].eulerAngle) // si la rotacio de l'objecte es igual a la rotacio de la condicio
                {
                    count++;

                }
            }
            foreach (SinglePath sp in pc.paths) //per cada cami
            {
                if (sp.block == null || sp.block.possiblePaths == null)
                {
                    continue; // Si no hay posibles caminos, saltar
                }
                if (sp.index < 0 || sp.index >= sp.block.possiblePaths.Count)
                {
                    Debug.LogWarning($"Índice fuera de rango: sp.index = {sp.index}, pero possiblePaths tiene {sp.block.possiblePaths.Count} elementos.");
                    continue; // Evita que el índice sea inválido
                }
                Debug.Log($"Path: {sp.block.name}, Índice: {sp.index}, Tamaño de possiblePaths: {sp.block.possiblePaths.Count}");
                sp.block.possiblePaths[sp.index].active = (count == pc.conditions.Count); //activar o desactivar el cami segons si totes les condicions es compleixen
            }
                
        }

        /*if (player1.walking || player2.walking)
            return;*/ //comentat per testing

        if(player.walking)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.CompareTag("interactuable"))
                {
                    pivots[0].DOComplete();
                    pivots[0].DORotate(new Vector3(0, 90 * 1, 0), .6f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack); //rotar el pivot 90 graus en l'eix y
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

    public void RotateRightPivot()
    {
        pivots[1].DOComplete();
        pivots[1].DORotate(new Vector3(0, 0, 90), .6f).SetEase(Ease.OutBack);
    }
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

}
[System.Serializable]
public class SinglePath
{
    public Navigable block;
    public int index;
}
