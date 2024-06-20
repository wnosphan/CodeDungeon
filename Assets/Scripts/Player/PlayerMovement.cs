using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            Debug.Log($"Input: {input}");

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    Debug.Log("Target position is walkable, starting movement.");
                    StartCoroutine(Move(targetPos));
                }
                else
                {
                    Debug.Log("Target position is not walkable.");
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer);
        if (hitCollider != null)
        {
            Debug.Log($"Hit collider: {hitCollider.name}");
            return false;
        }
        return true;
    }
}
