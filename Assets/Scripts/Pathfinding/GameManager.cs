using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton

    public PlayerController player;

    public List<PathCondition> pathConditions = new List<PathCondition>(); //la llista de condicions de camí
    public List<Transform> pivots; //la llista de pivots per moure'ls

    public Transform[] objectsToCover; //els objectes a cobrir quan es mogui algun pivot

    private Vector2 touchPosition; //la posició del touch

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckIfPathConditionsAreMet(); //comprova si les condicions de camí estan complertes
        if (player.isWalking) return; //si el jugador esta caminant, no es pot fer res
        //ListenClicksOnObjects(touchPosition); //escolta els clicks als objectes
    }

    private void CheckIfPathConditionsAreMet()
    {
        foreach (PathCondition pathCondition in pathConditions) //per cada condició de camí
        {
            int count = 0; //comptador de condicions complertes
            for (int i = 0; i < pathCondition.conditions.Count; i++) //per cada condició de la llista de condicions
            {
                if (pathCondition.conditions[i].conditionObject.eulerAngles == pathCondition.conditions[i].eulerAngle) //si la rotació de l'objecte es correspon amb la rotació de la condició
                {
                    count++; //augmenta el comptador
                }
            }
            foreach (SinglePath singlePath in pathCondition.paths) //per cada single path de la llista de paths de la condició
            {
                singlePath.block.possiblePaths[singlePath.index].active = (count == pathCondition.conditions.Count); //activa o desactiva el camí segons si totes les condicions estan complertes o no
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context) //escolta els inputs del jugador
    {
        if(context.performed && !player.isWalking) //si s'ha fet un input i el jugador no esta caminant
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            ListenClicksOnObjects(touchPosition);
        }
    }

    private void ListenClicksOnObjects(Vector2 touchPosition)
    {
        pivots[0].DOComplete(); //cancel·la la animació del pivot 0
        pivots[0].DORotate(new Vector3(0, 90, 0), .6f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack); //rota el pivot 0 90 graus en l'eix Y

        foreach(Transform obj in objectsToCover) //per cada objecte a cobrir
        {
            obj.gameObject.SetActive(pivots[0].eulerAngles.y > 45 && pivots[0].eulerAngles.y < 90 + 45);//activa o desactiva l'objecte segons la rotació del pivot 0 (si esta cobert o no) 
        }
    }

    public void RotateRightPivot()
    {
        pivots[1].DOComplete(); //cancel·la la animació del pivot 1
        pivots[1].DORotate(new Vector3(0, 0, 90), .6f).SetEase(Ease.OutBack); //rota el pivot 1 90 graus en l'eix Z

    }
}

//Classes necessaries

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
