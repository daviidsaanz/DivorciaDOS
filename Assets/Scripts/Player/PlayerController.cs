using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public bool walking = false;

    [Space]

    public Transform currentNode;
    public Transform clickedNode;
    public Transform marker;

    [Space]

    public List<Transform> finalPath = new List<Transform>();

    private float blend;

    void Start()
    {
        GetInfoOfCurrentNode();
    }

    void Update()
    {

        GetInfoOfCurrentNode();

        if (currentNode.GetComponent<Navigable>().movingGround)
        {
            transform.parent = currentNode.parent;
        }
        else
        {
            transform.parent = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Navigable>() != null)
                {
                    clickedNode = mouseHit.transform;
                    DOTween.Kill(gameObject.transform);
                    finalPath.Clear();
                    FindPath();

                    blend = transform.position.y - clickedNode.position.y > 0 ? -1 : 1;

                    marker.position = mouseHit.transform.GetComponent<Navigable>().GetWalkPoint();
                    Sequence s = DOTween.Sequence();
                    s.AppendCallback(() => marker.GetComponentInChildren<ParticleSystem>().Play());
                    s.Append(marker.GetComponent<Renderer>().material.DOColor(Color.white, .1f));
                    s.Append(marker.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f));
                    s.Append(marker.GetComponent<Renderer>().material.DOColor(Color.clear, .3f));

                }
            }
        }
    }

    void FindPath()
    {
        List<Transform> nodesToVisit = new List<Transform>();
        List<Transform> visitedNodes = new List<Transform>();

        foreach (TransitablePath path in currentNode.GetComponent<Navigable>().possiblePaths)
        {
            if (path.active)
            {
                nodesToVisit.Add(path.target);
                path.target.GetComponent<Navigable>().previousNode = currentNode;
            }
        }

        visitedNodes.Add(currentNode);

        ExploreNode(nodesToVisit, visitedNodes);
        BuildPath();
    }

    void ExploreNode(List<Transform> nodesToVisit, List<Transform> visitedNodes)
    {
        Transform current = nodesToVisit.First();
        nodesToVisit.Remove(current);

        if (current == clickedNode)
        {
            return;
        }

        foreach (TransitablePath path in current.GetComponent<Navigable>().possiblePaths)
        {
            if (!visitedNodes.Contains(path.target) && path.active)
            {
                nodesToVisit.Add(path.target);
                path.target.GetComponent<Navigable>().previousNode = current;
            }
        }

        visitedNodes.Add(current);

        if (nodesToVisit.Any())
        {
            ExploreNode(nodesToVisit, visitedNodes);
        }
    }

    void BuildPath()
    {
        Transform node = clickedNode;
        while (node != currentNode)
        {
            finalPath.Add(node);
            if (node.GetComponent<Navigable>().previousNode != null)
                node = node.GetComponent<Navigable>().previousNode;
            else
                return;
        }

        finalPath.Insert(0, clickedNode);

        FollowPath();
    }

    void FollowPath()
    {
        Sequence s = DOTween.Sequence();
        walking = true;

        for (int i = finalPath.Count - 1; i > 0; i--)
        {
            Navigable nav = finalPath[i].GetComponent<Navigable>();
            float time = nav.isStair ? 1.5f : 1; // Si es escalera, más lento

            // Movimiento del jugador
            s.Append(transform.DOMove(nav.GetWalkPoint(), .2f * time).SetEase(Ease.Linear));

            // Si el nodo permite rotación, aplicamos la rotación personalizada
            if (nav.isCurved)
            {
                s.Join(transform.DORotate(nav.customRotation, .3f).SetEase(Ease.OutSine));
            }
            else if (!nav.dontRotate) // Si no es curva, simplemente rota hacia la dirección normal
            {
                s.Join(transform.DOLookAt(nav.transform.position, .1f, AxisConstraint.Y, Vector3.up));
            }
        }

        // Si el nodo final es un botón, ejecutamos su acción
        if (clickedNode.GetComponent<Navigable>().isButton)
        {
            s.AppendCallback(() => GameManager.instance.RotateRightPivot());
        }

        // Limpieza al terminar el movimiento
        s.AppendCallback(() => Clear());
    }


    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Navigable>().previousNode = null;
        }
        finalPath.Clear();
        walking = false;
    }

    public void GetInfoOfCurrentNode()
    {

        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Navigable>() != null)
            {
                currentNode = playerHit.transform;

                if (playerHit.transform.GetComponent<Navigable>().isStair)
                {
                    DOVirtual.Float(GetBlend(), blend, .1f, SetBlend);
                }
                else
                {
                    DOVirtual.Float(GetBlend(), 0, .1f, SetBlend);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.GetChild(0).position, -transform.up);
        Gizmos.DrawRay(ray);
    }

    float GetBlend()
    {
        return GetComponentInChildren<Animator>().GetFloat("Blend");
    }
    void SetBlend(float x)
    {
        GetComponentInChildren<Animator>().SetFloat("Blend", x);
    }

}
