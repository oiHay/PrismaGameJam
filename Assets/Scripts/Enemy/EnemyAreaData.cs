using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAreaData", menuName = "Scriptable Objects/EnemyAreaData")]
public class EnemyAreaData : ScriptableObject
{
    public KeyCode key = KeyCode.LeftShift;
    public float reactionTime = 2f;
}
