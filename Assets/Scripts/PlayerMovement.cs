using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Vector2 direction;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        direction = new Vector2(moveX, moveY).normalized;
        if (direction.magnitude > 0.01f)
        {
            rb.velocity = direction * moveSpeed;
            transform.up = rb.velocity;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
