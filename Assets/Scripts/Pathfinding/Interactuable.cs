using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactuable : MonoBehaviour
{
    public Vector3 _rotationAmount;
    public Vector3 RotationAmount { get { return _rotationAmount; } set { _rotationAmount = value; } } //si volem que rote

    public Vector3 _moveAmount;
    public Vector3 MoveAmount { get { return _moveAmount; } set { _moveAmount = value; } } //si volem que es mogui

    public float duration = 0.6f; //duracio de l'animacio
    public bool useRotation = true; //si cal que roti o no
    public bool useMove = true;

    public bool useToggle; //si volem alternar entre dos estats
    private bool toggled = false; //si volem alternar entre dos estats

    private Quaternion initialRotation;


    private void Start()
    {
        initialRotation = transform.rotation;
    }

    public void Interact()
    {
        transform.DOComplete();

        if (useRotation && !useMove)
        {
            transform.DORotate(useToggle && toggled ? initialRotation.eulerAngles : initialRotation.eulerAngles + _rotationAmount, duration, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
        }
        else if (useMove && !useRotation)
        {
            transform.DOMove(useToggle && toggled ? transform.position - _moveAmount : transform.position + _moveAmount, duration).SetEase(Ease.OutBack);
        }
        else if (useMove && useRotation)
        {
            transform.DORotate(useToggle && toggled ? initialRotation.eulerAngles : initialRotation.eulerAngles + _rotationAmount, duration, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
            transform.DOMove(useToggle && toggled ? transform.position - _moveAmount : transform.position + _moveAmount, duration).SetEase(Ease.OutBack);
        }

        if (useToggle)
        {
            toggled = !toggled;
        }
    }
}
