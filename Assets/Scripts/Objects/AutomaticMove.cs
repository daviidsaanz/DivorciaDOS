using DG.Tweening;
using System.Collections;
using UnityEngine;
using Photon.Pun;

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

    private PhotonView view1;
    private PhotonView view2;

    void Start()
    {
        view1 = platform1.GetComponent<PhotonView>();
        view2 = platform2.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!automaticMoveStarted && view1.IsMine && view2.IsMine && IsRotationMatch(objectCondition.transform.rotation, targetRotation.rotation))
        {
            automaticMoveStarted = true;
            StartCoroutine(StartAutomaticMove());
        }
    }

    bool IsRotationMatch(Quaternion a, Quaternion b)
    {
        return Quaternion.Angle(a, b) <= rotationTolerance;
    }

    private IEnumerator StartAutomaticMove()
    {
        yield return new WaitForSeconds(1f);

        Sequence s = DOTween.Sequence();

        s.Append(platform1.transform
            .DORotate(new Vector3(0, 45, 0), 3f)
            .SetEase(Ease.InOutSine));

        s.Append(platform2.transform
            .DORotate(new Vector3(0, -45, 0), 3f)
            .SetEase(Ease.InOutSine));

        s.Append(platform2.transform
            .DOMove(new Vector3(-14.5f, -9.5f, 20.5f), 3f)
            .SetEase(Ease.InOutSine));

        s.Join(platform2.transform
            .DORotate(new Vector3(0, -45, 0), 3f)
            .SetEase(Ease.InOutSine));

        s.Append(platform1.transform
            .DORotate(new Vector3(0, 180, 0), 3f)
            .SetEase(Ease.InOutSine));

        s.Append(platform1.transform
            .DOMove(new Vector3(-2f, -0.5f, 8f), 1.5f)
            .SetEase(Ease.InOutSine));

        s.Join(platform1.transform
            .DORotate(new Vector3(0, -180, 0), 1.5f)
            .SetEase(Ease.InOutSine));

        s.Append(platform2.transform
            .DOMove(new Vector3(-14f, -5f, 20f), 1.5f)
            .SetEase(Ease.InOutSine));

        s.Join(platform2.transform
            .DORotate(new Vector3(0, -90, 0), 1.5f)
            .SetEase(Ease.InOutSine));

        s.AppendCallback(() =>
        {
            end1.SetActive(true);
            end2.SetActive(true);
        });

        yield return s.WaitForCompletion();

    }

}
