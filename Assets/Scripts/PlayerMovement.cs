using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Vector2 direction;
    Rigidbody2D rb;
    bool active;

    void Start()
    {
        active = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        NPCDialogue.dialogueStarted += DialogueStarted;
        NPCDialogue.dialogueEnded += DialogueEnded;
    }

    private void OnDisable()
    {
        NPCDialogue.dialogueStarted -= DialogueStarted;
        NPCDialogue.dialogueEnded -= DialogueEnded;
    }

    private void DialogueEnded()
    {
        active = true;
    }

    private void DialogueStarted()
    {
        active = false;
    }

    void FixedUpdate()
    {
        if (active)
        {
            Move();
            LookAtMouse();
        }
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePos - (Vector2)transform.position;
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        direction = new Vector2(x, y).normalized;
        if (direction.magnitude > 0.01f)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
