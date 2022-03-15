using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemSet : MonoBehaviour
{
    public HashSet<string> CollectedItems { get; set; } = new HashSet<string>();
    public bool saved = false;

    private void Awake()
    {
        GameEvents.SaveInitiated += Save;
    }

    void Save()
    {
        if(CollectedItems.Count != 0)
        {
            Debug.Log("Saving " + CollectedItems.Count + " coins");
            SaveSystem.Save(CollectedItems, "CollectedItems");
        }
    }
}
