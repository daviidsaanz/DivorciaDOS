using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Photon.Pun.Demo.PunBasics;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    public bool isWalking = false;

    public Transform currentNode; //el node en el que estem
    public Transform clickedNode; //el node al que volem anar
    public Transform marker; //el marker que es mou

    public List<Transform> finalPath = new List<Transform>(); //la llista de nodes que formen el camí
    private float blend;

    private Vector2 touchPosition;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        TakePositionInfo();
    }

    private void Update()
    {
        TakePositionInfo();
        UpdateParent();
    }

    private void TakePositionInfo()
    {
        Ray playerRay = new Ray(transform.position + Vector3.up * 0.5f, -transform.up); //tira un raycast des de la posició del player cap avall
        RaycastHit hit;

        if (Physics.Raycast(playerRay, out hit)) //si toca alguna cosa
        {
            Debug.Log("Hitting " + hit.transform.name);
            if (hit.transform.GetComponent<Navigable>()) //si toca un node
            {
                Debug.Log("Hitting " + hit.transform.name); 
                currentNode = hit.transform; //el node actual es el que toca (en el que estem)

                if(hit.transform.GetComponent<Navigable>().isStair) //Si el node en el que estem es una escala
                {
                    DOVirtual.Float(GetBlend(), blend, 0.1f, SetBlend);
                }
                else
                {
                    DOVirtual.Float(GetBlend(), 0, .1f, SetBlend); //si no es una escala, la blend es 0
                }
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

        if (Physics.Raycast(ray, out hit)) //si toca alguna cosa
        {
            if (hit.transform.GetComponent<Navigable>()) //si toca un node
            {
                clickedNode = hit.transform; //el node al que volem anar es el que toca
                DOTween.Kill(gameObject.transform);
                finalPath.Clear(); //netejem la llista de nodes del camí
                FindPath(); //busquem el camí

                blend = transform.position.y - clickedNode.position.y > 0 ? -1 : 1; //si la posició del player es mes gran que la del node al que volem anar, la blend es -1, si no, es 1

                marker.position = hit.transform.GetComponent<Navigable>().GetWalkPoint(); //la posició del marker es la posició del walkpoint del node al que volem anar
                Sequence sequence = DOTween.Sequence();
                sequence.AppendCallback(() => marker.GetComponentInChildren<ParticleSystem>().Play()); //activem el particle system del marker
                sequence.Append(marker.GetComponent<Renderer>().material.DOColor(Color.white, 0.1f)); //canviem el color del marker a blanc
                sequence.Append(marker.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f)); //canviem el color del marker a negre
                sequence.Append(marker.GetComponent<Renderer>().material.DOColor(Color.clear, .3f)); //canviem el color del marker a transparent
            }
        }

    }
    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            ListenClicks(touchPosition);
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            // Clic del ratón
            touchPosition = Mouse.current.position.ReadValue();
        }

    }

    private void FindPath()
    {
        List<Transform> NextCubes = new List<Transform>(); //la llista de nodes que toquen al node actual
        List<Transform> VisitedCubes = new List<Transform>(); //la llista de nodes visitats

        foreach (TransitablePath path in currentNode.GetComponent<Navigable>().possiblePaths) //per cada camí possible
        {
            if (path.active) //si el camí esta actiu
            {
                NextCubes.Add(path.target); //afegim el node al que porta a la llista de nodes següents
                path.target.GetComponent<Navigable>().PrevoiusNode = currentNode; //el node anterior del node al que porta es el node actual
            }
        }

        VisitedCubes.Add(currentNode); //afegim el node actual a la llista de nodes visitats

        ExploreCube(NextCubes, VisitedCubes); //cridem a la funció que explora els nodes
        BuildPath(); //cridem a la funció que construeix el camí
    }

    private void ExploreCube(List<Transform> NextCubes, List<Transform> VisitedCubes)
    {
        Transform current = NextCubes.First(); //el node actual es el primer de la llista de nodes següents
        NextCubes.Remove(current); //eliminem el node actual de la llista de nodes següents

        if (current == clickedNode)
        {
            return;
        }

        foreach (TransitablePath path in currentNode.GetComponent<Navigable>().possiblePaths) //per cada camí possible
        {
            if (!VisitedCubes.Contains(path.target) && path.active) //si el node ja esta visitat
            {
                NextCubes.Add(path.target); //afegim el node al que porta a la llista de nodes següents
                path.target.GetComponent<Navigable>().PrevoiusNode = currentNode; //el node anterior del node al que porta es el node actual
            }
        }

        VisitedCubes.Add(current); //afegim el node actual a la llista de nodes visitats

        if (NextCubes.Any()) //si hi ha nodes següents a visitar, cridem a la funció amb els nodes següents
        {
            ExploreCube(NextCubes, VisitedCubes);
        }

    }


    private void BuildPath()
    {
        Transform node = clickedNode; //el node es el node al que volem anar

        while (node != currentNode) //mentre el node no sigui el node actual
        {
            finalPath.Add(node); //afegim el node a la llista de nodes del camí
            if(node.GetComponent<Navigable>().PrevoiusNode != null) //si el node te un node anterior
            {
                node = node.GetComponent<Navigable>().PrevoiusNode; //el node es el node anterior
            }
            else { return; }
        }

        finalPath.Insert(0, currentNode); //afegim el node actual al principi de la llista de nodes del camí (osigui que el primer node es el node actual)

        FollowPath(); //cridem a la funció que segueix el camí
    }

    private void FollowPath()
    {
        Sequence sequence = DOTween.Sequence(); //creem una sequencia de tweens

        isWalking = true; //estem caminant

        for (int i = finalPath.Count -1; i > 0; i--)
        {
            float time = finalPath[i].GetComponent<Navigable>().isStair ? 1.5f : 1f; //el temps que triga a anar d'un node a l'altre es 0.5 segons, si el node es una escala, es 1 segon

            sequence.Append(transform.DOMove(finalPath[i].GetComponent<Navigable>().GetWalkPoint(), 0.2f * time).SetEase(Ease.Linear)); //movem el player al walkpoint del node actual amb el temps que toca i amb l'ease lineal (sense acceleració)

            if (!finalPath[i].GetComponent<Navigable>().dontRotate) //si el node no te la variable dontRotate activada
            {
                sequence.Join(transform.DOLookAt(finalPath[i].position, 0.1f, AxisConstraint.Y, Vector3.up)); //girem el player cap al node actual
            }

            if (clickedNode.GetComponent<Navigable>().isButton) //si el node te un boto
            {
                sequence.AppendCallback(() => GameManager.instance.RotateRightPivot()); //girem el pivot a la dreta
            }

            sequence.AppendCallback(() => Clear()); //netejem el camí

        }
    }

    private void Clear()
    {
        foreach(Transform transform in finalPath)
        {
            transform.GetComponent<Navigable>().PrevoiusNode = null; //netejem la variable PrevoiusNode de tots els nodes del camí
        }
        finalPath.Clear(); //netejem la llista de nodes del camí
        isWalking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray playerRay = new Ray(transform.position + Vector3.up * 0.5f, -transform.up);
        Gizmos.DrawRay(playerRay);
    }

    private void SetBlend(float value)
    {
        GetComponentInChildren<Animator>().SetFloat("Blend", value); //Aixo ens servira per fer l'animació del personatge depenent del tipus de terreny
    }

    private float GetBlend()
    {
        return GetComponentInChildren<Animator>().GetFloat("Blend");
    }
}
