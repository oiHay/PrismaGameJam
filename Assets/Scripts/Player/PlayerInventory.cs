using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public List<ItemSO> collectedItems = new List<ItemSO>();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemSO item)
    {
        if (!collectedItems.Contains(item))
            collectedItems.Add(item);
    }
}