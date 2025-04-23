using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AutomaticMove : MonoBehaviour
{
    [SerializeField] private GameObject objectCondition;
    [SerializeField] private Transform targetRotation;
    [SerializeField] private GameObject platform1; //Parent
    [SerializeField] private GameObject platform2; //Parent1

    [SerializeField] private GameObject end1; 
    [SerializeField] private GameObject end2; 

    private bool automaticMoveStarted = false;
    public float rotationTolerance = 1f; // Grados de tolerancia

    void Update()
    {
        if (!automaticMoveStarted && IsRotationMatch(objectCondition.transform.rotation, targetRotation.rotation))
        {
            automaticMoveStarted = true;
            StartCoroutine(StartAutomaticMove());
        }
    }

    bool IsRotationMatch(Quaternion a, Quaternion b)
    {
        return Quaternion.Angle(a, b) <= rotationTolerance;
    }

    IEnumerator StartAutomaticMove()
    {
        
        platform1.transform.DORotate(new Vector3(0, 45, 0), 1.5f);

        yield return new WaitForSeconds(1f); 

        platform2.transform.DORotate(new Vector3(0, -45, 0), 1.5f);

        yield return new WaitForSeconds(1f);

        platform2.transform.DOMove(new Vector3(-14.5f, -9.5f, 20.5f), 1.5f);
        platform2.transform.DORotate(new Vector3(0, -45, 0), 1.5f);
        platform1.transform.DORotate(new Vector3(0, 180, 0), 1.5f);

        yield return new WaitForSeconds(1f);

        platform1.transform.DOMove(new Vector3(-2f, -0.5f, 8f), 1.5f);
        platform1.transform.DORotate(new Vector3(0, -180, 0), 1.5f);

        platform2.transform.DOMove(new Vector3(-14f, -5f, 20f), 1.5f);
        platform2.transform.DORotate(new Vector3(0, -90, 0), 1.5f);

        yield return new WaitForSeconds(1f);

        end1.SetActive(true);
        end2.SetActive(true);


    }
}
