// Item.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ProjectUndead/Weapon")]
public class Item : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    public int    id;

    [Header("Stats")]
    public int    damage;
    public float  fireRate;
    public Sprite icon;

    // …any other shared data fields
}
