using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public static class SGUtils
{
    public static void SafeDestroy(Object obj)
    {
#      if UNITY_EDITOR
            if (Application.isEditor)
            {
                Object.DestroyImmediate(obj);
            }
            else
#      endif
            {
                Object.Destroy(obj);
            }
    }

    public static bool CameraToGrounRaycast(Camera camera, float distance, ref Vector3 vec)
    {
        var inputValue = Mouse.current.position.ReadValue();
        var ray = camera.ScreenPointToRay(inputValue);

        foreach (var hit in Physics.RaycastAll(ray, distance))
        {
            // TODO: replace tag with layer mask
            if (hit.collider.CompareTag("Ground"))
            {
                vec = hit.point;
                return true;
            }
        }
        return false;
    }
    public static void DrawNavLine(LineRenderer lineRenderer, Vector3 from, Vector3 to)
    {
        var path = new NavMeshPath();
        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);

        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
    }
}
