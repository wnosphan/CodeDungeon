using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Vector2 startPosition; // Starting position of the patrol
    public Vector2 endPosition; // Ending position of the patrol
    public float moveSpeed = 3f; // Movement speed of the monster
    private Rigidbody2D rb; // Rigidbody2D component of the monster
    private Vector2 currentTarget; // Current target position for the monster to move towards

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = endPosition; // Start by moving towards the end position
    }

    void Update()
    {
        // Move towards the current target position
        Vector2 currentPosition = rb.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, currentTarget, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Check if the monster has reached the current target position
        if (Vector2.Distance(currentPosition, currentTarget) < 0.1f)
        {
            // Switch target position to create a patrol loop
            if (currentTarget == startPosition)
                currentTarget = endPosition;
            else
                currentTarget = startPosition;
        }
    }
}
