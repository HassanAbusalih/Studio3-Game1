using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public bool interactable;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;

    void Start()
    {
        if (transform.childCount == 0)
        {
            Debug.LogError($"{name} needs a child GameObject.");
            return;
        }
        door = transform.GetChild(0).gameObject;
        gameObject.layer = 0;
    }

    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.Space))
        {
            Toggle();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            interactable = false;
        }
    }

    private void Toggle()
    {
        door.SetActive(!door.activeSelf);
        if (source == null)
        {
            return;
        }
        if (door.activeSelf)
        {
            source.clip = openSound;
        }
        else
        {
            source.clip = closeSound;
        }
        source.Play();
    }
}
