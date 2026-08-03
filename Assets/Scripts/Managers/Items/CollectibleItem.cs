using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ItemSO item;

    public ItemSO Item => item;
}
