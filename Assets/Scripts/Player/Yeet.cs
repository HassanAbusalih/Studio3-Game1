using System;
using UnityEngine;
using UnityEngine.UI;

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
    float colliderSize;
    AudioSource source;
    [SerializeField] int uses = 1;
    [SerializeField] float cooldown = 5;
    [SerializeField] Image cooldownImage;
    [SerializeField] float cooldownTimer;

    private void Start()
    {
        cooldownTimer = cooldown * uses;
        colliderSize = GetComponentInParent<BoxCollider2D>().size.x;
    }

    void Update()
    {
        if (!active) { return; }
        LookAtMouse();
        cooldownTimer += Time.deltaTime;
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = Mathf.Clamp(cooldownTimer/ (cooldown * uses), 0, 1);
        }
        if (cooldownTimer >= cooldown && Input.GetMouseButtonDown(0))
        {
            cooldownTimer -= cooldown;
            ThrowProjectile();
        }
        cooldownTimer = Mathf.Clamp(cooldownTimer, 0, cooldown * uses);
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
        else if (Mathf.Max(distance, colliderSize) == colliderSize)
        {
            target = (Vector2)transform.position + (direction.normalized * colliderSize);
        }
        else
        {
            target = mousePos;
        }
        GameObject gameObject = Instantiate(projectilePrefab, transform.position + transform.up, Quaternion.identity);
        gameObject.GetComponent<Projectile>().SetParameters(target, speed, maxDistance, source);
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
