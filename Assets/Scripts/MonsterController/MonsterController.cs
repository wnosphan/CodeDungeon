using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 8f; // Movement speed of the monster
    private Rigidbody2D rb; // Rigidbody2D component of the monster

    public Transform player; // Reference to the player
    public float chaseRange = 15f; // Range within which the monster starts chasing the player
    public LayerMask solidObjectLayer; // Layer mask for solid objects
    private CapsuleCollider2D monsterCapsuleCollider; // Collider component of the monster
    public float contactDistance = 0.1f; // Distance to check for collisions
    private Vector2 startPosition; // Starting position of the monster
    private bool isChasing; // Flag to track if the monster is chasing the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        monsterCapsuleCollider = GetComponent<CapsuleCollider2D>(); // Assuming the monster has a CapsuleCollider2D component
        startPosition = rb.position; // Store the initial position as the start position
    }

    void Update()
    {
        if (!isChasing)
        {
            // Check the distance between the monster and the player
            float distanceToPlayer = Vector2.Distance(rb.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                // If the player is within the chase range, start chasing the player
                isChasing = true;
            }
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = player.position;
        Vector2 directionToPlayer = (targetPosition - currentPosition).normalized;

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the new position is walkable before moving
        if (IsWalkable(newPosition))
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            // If the new position is not walkable, find an alternate direction
            Vector2 alternateDirection = FindAlternateDirection(directionToPlayer);
            newPosition = Vector2.MoveTowards(currentPosition, currentPosition + alternateDirection, moveSpeed * Time.deltaTime);

            // Check if the alternate position is walkable before moving
            if (IsWalkable(newPosition))
            {
                rb.MovePosition(newPosition);
            }
            else
            {
                // If both direct and alternate paths are blocked, stop chasing
                isChasing = false;
            }
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        // Use CapsuleCollider2D to check for collisions
        Vector2 direction = (targetPos - rb.transform.position).normalized;
        float distance = Vector2.Distance(rb.position, targetPos) + contactDistance;
        RaycastHit2D hit = Physics2D.CapsuleCast(rb.position, monsterCapsuleCollider.size, monsterCapsuleCollider.direction, 0f, direction, distance, solidObjectLayer);

        return hit.collider == null;
    }

    private Vector2 FindAlternateDirection(Vector2 originalDirection)
    {
        // Check directions to the left and right of the original direction
        Vector2 leftDirection = new Vector2(-originalDirection.y, originalDirection.x);
        Vector2 rightDirection = new Vector2(originalDirection.y, -originalDirection.x);

        // Check if the left direction is walkable
        if (IsWalkable(rb.position + leftDirection))
        {
            return leftDirection;
        }

        // Check if the right direction is walkable
        if (IsWalkable(rb.position + rightDirection))
        {
            return rightDirection;
        }

        // If both sides are blocked, continue trying in the original direction
        return originalDirection;
    }

    public void ResetPosition()
    {
        rb.position = startPosition; // Reset the monster's position to the start position
        isChasing = false; // Stop chasing the player
    }
}
