using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigable : MonoBehaviour
{
    public List<TransitablePath> possiblePaths = new List<TransitablePath>();

    public Transform previousNode;

    public bool isStair = false;
    public bool isCurved = false;
    public bool movingGround = false;
    public bool isButton;
    public bool dontRotate;

    public float walkPointOffset = .5f;
    public float stairOffset = .4f;
    public Vector3 customRotation;

    public Vector3 GetWalkPoint()
    {
        float stair = isStair ? stairOffset : 0;
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        float stair = isStair ? .4f : 0;
        Gizmos.DrawSphere(GetWalkPoint(), .1f);

        if (possiblePaths == null)
            return;

        foreach (TransitablePath p in possiblePaths)
        {
            if (p.target == null)
                return;
            Gizmos.color = p.active ? Color.black : Color.clear;
            Gizmos.DrawLine(GetWalkPoint(), p.target.GetComponent<Navigable>().GetWalkPoint());
        }
    }
}

[System.Serializable]
public class TransitablePath
{
    public Transform target;
    public bool active = true;
}
