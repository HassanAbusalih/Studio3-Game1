using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemsUI : MonoBehaviour
{
    public static Action<Item> RegisterItem;
    public static Action<Item> RemoveItem;
    [SerializeField] GameObject itemUIPrefab;
    [SerializeField] GameObject itemsUI;
    [SerializeField] GameObject levelComplete;
    List<Item> items = new List<Item>();
    List<TextMeshProUGUI> itemUIList = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        RegisterItem += OnRegisterItem;
        RemoveItem += OnRemoveItem;
    }

    private void OnDisable()
    {
        RegisterItem -= OnRegisterItem;
        RemoveItem -= OnRemoveItem;
    }

    void OnRegisterItem(Item item)
    {
        items.Add(item);
        GameObject itemUI = Instantiate(itemUIPrefab, itemsUI.transform);
        itemUIList.Add(itemUI.GetComponent<TextMeshProUGUI>());
        itemUIList[itemUIList.Count - 1].text = item.ItemName;
    }

    void OnRemoveItem(Item item)
    {
        items.Remove(item);
        foreach (var itemUI in itemUIList)
        {
            if (itemUI.text == item.ItemName)
            {
                itemUI.text = "<s>" + item.ItemName + "</s>";
                break;
            }
        }
        Destroy(item.transform.parent.gameObject);
        if (items.Count == 0)
        {
            Time.timeScale = 0;
            levelComplete.SetActive(true);
        }
    }
}
