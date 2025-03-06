using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigable : MonoBehaviour
{
    public List<TransitablePath> possiblePaths = new List<TransitablePath>(); //llista de canins disponibles

    public Transform PrevoiusNode; //Node anterior

    public bool isStair = false;
    public bool movingGround = false;
    public bool isButton;
    public bool dontRotate;

    public float walkOffset = 0.5f;
    public float stairOffset = 0.4f;
   
    public Vector3 GetWalkPoint()
    {
        float stair = isStair ? stairOffset : 0;
        return transform.position + Vector3.up * (walkOffset - stair); //retorna la posició del punt de cami
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        float stair = isStair ? 0.4f : 0f;
        Gizmos.DrawSphere(GetWalkPoint(), 0.1f); //dibuija una esfera en el punt de cada cami

        if (possiblePaths == null) return;

        foreach (TransitablePath path in possiblePaths)
        {
            if (path.target == null) return;
            Gizmos.color =path.active ? Color.green : Color.clear; //si el cami esta actiu el dibuixa de color verd si no no
            Gizmos.DrawLine(GetWalkPoint(), path.target.GetComponent<Navigable>().GetWalkPoint()); //sibuiza una linia entre els punts de cami
        }
    }
}

[System.Serializable]
public class TransitablePath //classe de cami transitable
{
    public Transform target;
    public bool active = true;
}