using System;
using UnityEngine;

public class Yeet : MonoBehaviour
{
    bool active = true;
    Vector2 mousePos;
    Vector2 target;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float maxDistance;
    [SerializeField] float speed;
    public static Action<Vector2> SoundGenerated;

    void Update()
    {
        if (!active) { return; }
        LookAtMouse();
        if (Input.GetMouseButtonDown(0))
        {
            ThrowProjectile();
        }
    }

    void LookAtMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePos - (Vector2)transform.position;
    }

    void ThrowProjectile()
    {
        Vector2 direction = mousePos - (Vector2)transform.position;
        float distance = Mathf.Min(maxDistance, direction.magnitude);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, mask);
        if (hit.collider != null)
        {
            target = hit.point;
        }
        else if (direction.magnitude > maxDistance)
        {
            target = (Vector2)transform.position + (direction.normalized * maxDistance);
        }
        else
        {
            target = mousePos;
        }
        GameObject gameObject = Instantiate(projectilePrefab, transform.position + transform.up, Quaternion.identity);
        gameObject.GetComponent<Projectile>().SetParameters(target, speed);
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
}
