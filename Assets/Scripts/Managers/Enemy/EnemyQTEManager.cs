using System;
using System.Collections;
using UnityEngine;

public class EnemyQTEManager : MonoBehaviour
{
    public static event Action<EnemyArea> OnQTEStarted;
    public static event Action<EnemyArea, bool> OnQTEFinished;

    private bool running;

    private void OnEnable()
    {
        EnemyArea.OnPlayerDetected += StartQTE;
    }

    private void OnDisable()
    {
        EnemyArea.OnPlayerDetected -= StartQTE;
    }

    private void StartQTE(EnemyArea enemyArea)
    {
        if (running)
            return;

        StartCoroutine(RunQTE(enemyArea));
    }

    private IEnumerator RunQTE(EnemyArea enemyArea)
    {
        running = true;

        OnQTEStarted?.Invoke(enemyArea);

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
        OnQTEFinished?.Invoke(enemyArea, success);

        running = false;
    }
}