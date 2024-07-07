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
        // Determine the direction to move
        Vector2 direction = currentPoint.position - transform.position;
        direction.Normalize();

        // Set the velocity based on the direction
        rb.velocity = direction * speed;

        // Update the animator parameters based on the direction
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);        

        // Check the distance to the current point
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            // Stop the enemy
            rb.velocity = Vector2.zero;

            // Switch the target point
            if (currentPoint == pointB.transform)
            {
                currentPoint = pointA.transform;
            }
            else if (currentPoint == pointA.transform)
            {
                currentPoint = pointB.transform;
            }
        }   
       
      
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.DrawSphere(pointA.transform.position, 0.5f);
            Gizmos.DrawSphere(pointB.transform.position, 0.5f);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }

  
}
