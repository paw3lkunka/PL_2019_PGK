using System.Collections;
using UnityEngine;

public class DropAnimation : MonoBehaviour
{
    [HideInInspector]
    public Vector3 endPosition = Vector3.zero;

    [HideInInspector]
    public Vector2 durationRange;

    private float duration;
    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        if(endPosition != Vector3.zero)
        {
            duration = UnityEngine.Random.Range(durationRange.x, durationRange.y);
            rb.AddForce(endPosition / duration, ForceMode.Impulse); 
        }
    }
}
