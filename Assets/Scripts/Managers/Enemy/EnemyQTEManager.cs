using System;
using System.Collections;
using UnityEngine;

public class EnemyQTEManager : MonoBehaviour
{
    [Header("Detecção")] [SerializeField] private PlayerDetectionSystem detectionSystem;
    
    public static event Action<EnemyArea> OnQteStarted;
    public static event Action<EnemyArea, bool> OnQteFinished;

    private bool _running;

    private void OnEnable()
    {
        EnemyArea.OnPlayerDetected += StartQte;
    }

    private void OnDisable()
    {
        EnemyArea.OnPlayerDetected -= StartQte;
    }

    private void StartQte(EnemyArea enemyArea)
    {
        if (_running)
            return;

        if (UnityEngine.Random.value > GetQteChance(enemyArea.Data))
        {
            OnQteFinished?.Invoke(enemyArea, true);
            return;
        }
        
        StartCoroutine(RunQte(enemyArea));
    }
    
    private float GetQteChance(EnemyAreaData data)
    {
        if (detectionSystem == null)
            return data.attackChance;

        float detectionFactor = detectionSystem.DetectionMeter / 100f;
        return Mathf.Clamp01(data.attackChance * detectionFactor);
    }

    private IEnumerator RunQte(EnemyArea enemyArea)
    {
        _running = true;

        OnQteStarted?.Invoke(enemyArea);

        Debug.Log($"Pressione {enemyArea.Data.key}");

        float timer = 0f;
        bool success = false;

        while (timer < enemyArea.Data.reactionTime)
        {
            if (Input.GetKeyDown(enemyArea.Data.key))
            {
                success = true;
                Debug.Log($"Fugiu");
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log($"Morreu");
        OnQteFinished?.Invoke(enemyArea, success);

        _running = false;
    }
}