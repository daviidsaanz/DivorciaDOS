using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void ListenClicks(Vector2 touchPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPoint); //tira un raycast des de la posició del touch

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) //si toca alguna cosa
        {
            if(hit.transform.GetComponent<Navigable>()) //si toca un node
            {
                clickedNode = hit.transform; //el node al que volem anar es el que toca
            }
        }

    }
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        ListenClicks(touchPosition);
    }

    private void FindPath()
    {
        List <Transform> NextCubes = new List<Transform>(); //la llista de nodes que toquen al node actual
        List<Transform> VisitedCubes = new List<Transform>(); //la llista de nodes visitats

        foreach(TransitablePath path in currentNode.GetComponent<Navigable>().possiblePaths) //per cada camí possible
        {
           if(path.active) //si el camí esta actiu
            {
                NextCubes.Add(path.target); //afegim el node al que porta a la llista de nodes següents
                path.target.GetComponent<Navigable>().PrevoiusNode = currentNode; //el node anterior del node al que porta es el node actual
            }
        }

        VisitedCubes.Add(currentNode); //afegim el node actual a la llista de nodes visitats
    }

    private void ExplreCube(List<Transform> NextCubes, List<Transform> VisitedCubes)
    {
        Transform current = NextCubes.First(); //el node actual es el primer de la llista de nodes següents
        NextCubes.Remove(current); //eliminem el node actual de la llista de nodes següents

        if(current == clickedNode)
        {
            return;
        }

        foreach (TransitablePath path in currentNode.GetComponent<Navigable>().possiblePaths) //per cada camí possible
        {   
            if(!VisitedCubes.Contains(path.target) && path.active) //si el node ja esta visitat
            {
                
            }

          
            
        }

    }



}
