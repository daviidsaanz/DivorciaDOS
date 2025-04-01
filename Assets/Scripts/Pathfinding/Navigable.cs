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
    public bool isChildrenOfInteractuable;
    public bool forceClimbing;
    public bool blockPlayer;
    public bool ifPlayerWalksDisable;
    public Interactuable neighbourInteractuable;

    public float walkPointOffset = .5f;
    public float stairOffset = .4f;
    public Vector3 customRotation;

    public bool GetInclination()
    {
        Interactuable objectInteractuable = GetComponentInParent<Interactuable>();

        if (objectInteractuable != null)
        {
            Quaternion currentRotation = objectInteractuable.transform.rotation;
            Quaternion savedRotation = Quaternion.Euler(objectInteractuable.VerticalRotation);

            float angleDifference = Quaternion.Angle(currentRotation, savedRotation);

            return angleDifference < 1f;
        }
        else if(forceClimbing)
        {
            return true;
        }
        return false;
    }

    public void DesactivateInteractuable()
    {
        Interactuable objectInteractuable = GetComponentInParent<Interactuable>();
        if (objectInteractuable != null)
        {
            objectInteractuable.isEnabled = false;
            objectInteractuable.UpdateAnimator(false);
        }
    }

    public void ActivateInteractuable()
    {
        if (neighbourInteractuable != null)
        {
            neighbourInteractuable.isEnabled = true;
            neighbourInteractuable.UpdateAnimator(true);
        }
    }

    public Vector3 GetWalkPoint()
    {
        float stair = isStair ? stairOffset : 0;
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    private void OnDrawGizmos()
    {
        //dibuixa una linea groga del vector3.up del objecte
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.up + transform.position, -transform.up + transform.position);

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
