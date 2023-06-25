using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string itemName;
    bool collected;
    bool inRange;
    public string ItemName { get => itemName; }

    void Start()
    {
        if (itemName != null)
        {
            ItemsUI.RegisterItem.Invoke(this);
        }
        else
        {
            Debug.Log("Gimmie a name!");
        }
    }

    void Update()
    {
        if (inRange && !collected && Input.GetKeyDown(KeyCode.Space))
        {
            if (itemName != null)
            {
                ItemsUI.RemoveItem.Invoke(this);
            }
            collected = true;
        }
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
