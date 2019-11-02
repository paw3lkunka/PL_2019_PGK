using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Obsolete]
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

    private void OnEnable()
    {
        if (!playerControlled)
        {
            GameManager.Instance.enemies.Add(gameObject);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.enemies.Remove(gameObject);
    }
    
}
