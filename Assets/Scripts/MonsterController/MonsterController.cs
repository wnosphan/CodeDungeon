using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 3f; // Movement speed of the monster
    private Rigidbody2D rb; // Rigidbody2D component of the monster

    public Transform player; // Reference to the player
    public float chaseRange = 5f; // Range within which the monster starts chasing the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check the distance between the monster and the player
        float distanceToPlayer = Vector2.Distance(rb.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            // If the player is within the chase range, chase the player
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = player.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }
}
