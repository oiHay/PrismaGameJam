using System;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    public static event Action<EnemyAreaData> OnPlayerDetected;

    [SerializeField] private EnemyAreaData enemyArea;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        OnPlayerDetected?.Invoke(enemyArea);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        triggered = false;
    }
}
