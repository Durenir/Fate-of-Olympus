using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Items", menuName = "Collectable Item")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> Items;

    public Item GetItem(string itemName)
    {
        return Items.Find(x=>x.itemName == itemName);
    }

    public Item GetItem(int itemIndex)
    {
        return Items[itemIndex];
    }
}
[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;

    public Item(string name, string description)
    {
        itemName = name;
        itemDescription = description;
    }
}