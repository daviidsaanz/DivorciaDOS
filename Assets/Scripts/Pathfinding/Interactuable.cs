using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactuable : MonoBehaviour
{
    public Vector3 rotationAmount; //si volem que roti en lloc de moure
    public Vector3 moveAmount; //si volem que es mogui en lloc de rotar
    public float duration = 0.6f; //duracio de l'animacio
    public bool useRotation = true; //si cal que roti o no

    private bool toggled = false; //si volem alternar entre dos estats

    public void Interact()
    {
        if (useRotation)
        {
            transform.DOComplete(); //para la animacio actual
            transform.DORotate(toggled ? Vector3.zero : rotationAmount, duration, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack); //rota l'objecte com volem
        }
        else
        {
            transform.DOComplete();
            transform.DOMove(toggled ? transform.position : transform.position + moveAmount, duration).SetEase(Ease.OutBack); //mou l'objecte com volem
        }

        toggled = !toggled; //canvia l'estat de l'objecte (si estava activat, ara està desactivat i viceversa)
    }
}
