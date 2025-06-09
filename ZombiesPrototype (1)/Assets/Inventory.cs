using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int capacity = 20;
    public List<Item> items = new List<Item>();

    public bool AddItem(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }
        items.Add(item);
        Debug.Log($"Picked up {item.itemName}. Now you have {items.Count}/{capacity} items.");
        return true;
    }

    public bool RemoveItem(Item item)
    {
        return items.Remove(item);
    }
}
