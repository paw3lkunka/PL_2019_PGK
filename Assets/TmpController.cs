using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TmpController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private NavMeshAgent agent;

    public bool playerControlled = false;

    public GameObject target;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( target != null && !playerControlled )
        {
            agent.SetDestination(target.transform.position);
        }
    }

    public void GoToMousePosition()
    {
        agent.destination = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
