using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    private ItemDatabase database;
    private CollectibleItemSet collectibleItemSet;
    private UniqueID uniqueID;

    // Start is called before the first frame update
    void Start()
    {
        uniqueID = GetComponent<UniqueID>();
        // database = FindObjectOfType<ItemDatabase>();
        collectibleItemSet = FindObjectOfType<CollectibleItemSet>();
        if (collectibleItemSet.CollectedItems.Contains(uniqueID.ID))
        {
            Destroy(this.gameObject);
            return;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision Detected with: " + uniqueID.name);
            Debug.Log("Unique ID: " + uniqueID.ID);
            collectibleItemSet.CollectedItems.Add(uniqueID.ID);
            Destroy(gameObject);
        }
        Debug.Log("We have " + collectibleItemSet.CollectedItems.Count + " coins");
    }
}