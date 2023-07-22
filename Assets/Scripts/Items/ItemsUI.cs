using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemsUI : MonoBehaviour
{
    public static Action<Item> RegisterItem;
    public static Action<Item> RemoveItem;
    public static Action LevelComplete;
    [SerializeField] GameObject itemUIPrefab;
    [SerializeField] GameObject itemsUI;
    [SerializeField] GameObject levelComplete;
    List<Item> items = new();
    List<TextMeshProUGUI> itemUIList = new();

    private void OnEnable()
    {
        RegisterItem += OnRegisterItem;
        RemoveItem += OnRemoveItem;
        LevelExit.Exited += OnLevelExit;
    }

    private void OnDisable()
    {
        RegisterItem -= OnRegisterItem;
        RemoveItem -= OnRemoveItem;
        LevelExit.Exited -= OnLevelExit;
    }

    void OnRegisterItem(Item item)
    {
        items.Add(item);
        if (itemsUI != null)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, itemsUI.transform);
            itemUIList.Add(itemUI.GetComponent<TextMeshProUGUI>());
            itemUIList[itemUIList.Count - 1].text = item.ItemName;
        }
    }

    void OnRemoveItem(Item item)
    {
        items.Remove(item);
        if (itemUIList.Count > 0)
        {
            foreach (var itemUI in itemUIList)
            {
                if (itemUI == null)
                {
                    break;
                }
                if (itemUI.text == item.ItemName)
                {
                    itemUI.text = "<s>" + item.ItemName + "</s>";
                    break;
                }
            }
        }
        Destroy(item.transform.parent.gameObject);
        if (items.Count == 0)
        {
            Time.timeScale = 0;
            LevelComplete?.Invoke();
        }
    }

    void OnLevelExit()
    {
        //This might actually be the most lazy code I've ever written
        levelComplete.SetActive(true);
    }
}
