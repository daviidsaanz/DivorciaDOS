using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    public bool isWalking = false;

    public Transform currentNode; //el node en el que estem
    public Transform clickedNode; //el node al que volem anar
    public Transform marker; //el marker que es mou

    public List<Transform> finalPath = new List<Transform>(); //la llista de nodes que formen el camí
    private float mix;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMove;




        TakePositionInfo();
    }



    private void Update()
    {
        TakePositionInfo();
        UpdateParent();
        ListenClicks();

    }

    private void TakePositionInfo()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up); //tira raycast cap avall
        RaycastHit hit;

        if(Physics.Raycast(playerRay, out hit)) //si toca alguna cosa
        {
            if(hit.transform.GetComponent<Navigable>()) //si toca un node
            {
                currentNode = hit.transform; //el node actual es el que toca (en el que estem)

                //SI ES UNA ESCALA APLICAREM UNA ANIMACIO DE MOVIMENT DE ESCALES
                //SI NO, APLICAREM UNA ANIMACIO DE MOVIMENT NORMAL

            }
        }
    }

    private void UpdateParent()
    {
        if (currentNode.GetComponent<Navigable>().movingGround) //si el node en el que estem es mou 
        {
            transform.parent = currentNode;
        }
        else
        {
            transform.parent = null;
        }
    }

    private void ListenClicks()
    {

    }
    private void OnMove(InputAction.CallbackContext context)
    {
        
    }

}
