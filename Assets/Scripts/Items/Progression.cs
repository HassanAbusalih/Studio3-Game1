using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [SerializeField] int itemCount;
    int collected;

    private void OnEnable()
    {
        ItemsUI.RemoveItem += DeactivateObject;
    }

    private void OnDisable()
    {
        ItemsUI.RemoveItem -= DeactivateObject;
    }
    void DeactivateObject(Item item)
    {
        collected++;
        if (collected == itemCount)
        {
            //Should probably be an animation
            Destroy(gameObject);
        }
    }    
}
