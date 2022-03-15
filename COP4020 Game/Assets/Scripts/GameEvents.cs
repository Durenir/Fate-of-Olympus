using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static System.Action<Item> ItemCollected;
    public static System.Action SaveInitiated;
    public static System.Action LoadInitiated;
    public static System.Action DeleteAllSaveFilesInitiated;

    public static void OnItemCollected(Item item)
    {
        ItemCollected?.Invoke(item);
    }

    public static void OnSaveInitiated()
    {
        SaveInitiated?.Invoke();
    }
}
