using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Dialogue/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
}