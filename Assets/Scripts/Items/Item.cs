using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string itemName;
    bool collected;
    bool inRange;
    public string ItemName { get => itemName; }
    NPCDialogue dialogue;
    PlayerDialogue playerDialogue;

    void Start()
    {
        if (itemName != null)
        {
            ItemsUI.RegisterItem.Invoke(this);
            dialogue = GetComponent<NPCDialogue>();
            playerDialogue = FindObjectOfType<PlayerDialogue>();
        }
        else
        {
            Debug.Log("Gimmie a name!");
        }
    }

    void Update()
    {
        if (dialogue == null && inRange && !collected && Input.GetKeyDown(KeyCode.Space))
        {
            CollectItem();
        }
    }

    public void CollectItem()
    {
        if (itemName != null)
        {
            ItemsUI.RemoveItem.Invoke(this);
        }
        collected = true;
        playerDialogue.AudioSource.PlayOneShot(playerDialogue.Start);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null && !collected)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null) 
        {
            inRange = false;
        }
    }
}
