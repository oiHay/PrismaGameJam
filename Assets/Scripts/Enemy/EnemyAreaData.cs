using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAreaData", menuName = "Scriptable Objects/EnemyAreaData")]
public class EnemyAreaData : ScriptableObject
{
    [Header("Quick Time Event de Ataque")]

    [Header("Tecla que o jogador deve pressionar para escapar do ataque.")]
    public KeyCode key = KeyCode.LeftShift;

    [Header("Tempo máximo, em segundos, para o jogador reagir.")]
    public float reactionTime = 2f;

    [Header("Chance de o ataque acontecer quando o jogador entrar na área.\n 0 = nunca, 1 = sempre.")]
    [Range(0f, 1f)]
    public float attackChance = 0.5f;
}
