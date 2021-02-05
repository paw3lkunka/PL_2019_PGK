using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            LocationManager.Instance.isExiting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            LocationManager.Instance.isExiting = false;
        }
    }

    private static bool allChildren = true;
    private static Color color = Color.red;

    private void OnDrawGizmos()
    {
        BoxCollider[] boxColliders;
        if (allChildren)
        {
            boxColliders = GetComponentsInChildren<BoxCollider>();

        }
        else
        {
            boxColliders = GetComponents<BoxCollider>();
        }

        foreach (var collider in boxColliders)
        {
            DrawBoxCollider(collider.transform, color, collider);
        }
    }

    public static void DrawBoxCollider(Transform transform, Color gizmoColor, BoxCollider boxCollider, float alphaForInsides = 0.3f)
    {
        //Save the color in a temporary variable to not overwrite changes in the inspector (if the sent-in color is a serialized variable).
        var color = gizmoColor;

        //Change the gizmo matrix to the relative space of the boxCollider.
        //This makes offsets with rotation work
        //Source: https://forum.unity.com/threads/gizmo-rotation.4817/#post-3242447
        Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(boxCollider.center), transform.rotation, transform.lossyScale);

        //Draws the edges of the BoxCollider
        //Center is Vector3.zero, since we've transformed the calculation space in the previous step.
        Gizmos.color = color;
        Gizmos.DrawWireCube(Vector3.zero, boxCollider.size);

        //Draws the sides/insides of the BoxCollider, with a tint to the original color.
        color.a *= alphaForInsides;
        Gizmos.color = color;
        Gizmos.DrawCube(Vector3.zero, boxCollider.size);
    }
}
