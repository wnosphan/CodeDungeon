using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    void Update()
    {
       Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(0, speed);
        }
        else
        {
            rb.velocity = new Vector2(0, -speed);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.1f && currentPoint == pointB.transform)
        {
            
                currentPoint = pointA.transform;
            
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f && currentPoint == pointA.transform)
        {

            currentPoint = pointB.transform;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

}
