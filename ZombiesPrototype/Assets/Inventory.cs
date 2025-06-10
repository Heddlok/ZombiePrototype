using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [Tooltip("Maximum number of items you can hold")]
    public int capacity = 20;

    [Header("Starting Items")]
    [Tooltip("Assign your Item assets here to populate the inventory at start")]
    public List<Item> startingItems = new List<Item>();

    // Internal runtime list
    private List<Item> items = new List<Item>();

    private void Awake()
    {
        // Seed with any starting items
        if (startingItems != null && startingItems.Count > 0)
        {
            items.AddRange(startingItems);
            Debug.Log($"Inventory initialized with {items.Count}/{capacity} items.");
        }
    }

    /// <summary>
    /// Attempts to add an Item asset to the inventory.
    /// </summary>
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

    /// <summary>
    /// Removes an Item asset from the inventory.
    /// </summary>
    public bool RemoveItem(Item item)
    {
        bool removed = items.Remove(item);
        if (removed)
            Debug.Log($"Removed {item.itemName}. Now you have {items.Count}/{capacity} items.");
        return removed;
    }

    /// <summary>
    /// Read-only access to the current inventory contents.
    /// </summary>
    public IReadOnlyList<Item> Items => items.AsReadOnly();
}
