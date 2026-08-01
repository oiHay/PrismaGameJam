using System;
using System.Collections;
using UnityEngine;

public class EnemyQTEManager : MonoBehaviour
{
    public static event Action OnQTEStarted;
    public static event Action OnQTESuccess;
    public static event Action OnQTEFail;

    private bool running;

    private void OnEnable()
    {
        EnemyArea.OnPlayerDetected += StartQTE;
    }

    private void OnDisable()
    {
        EnemyArea.OnPlayerDetected -= StartQTE;
    }

    private void StartQTE(EnemyAreaData enemyArea)
    {
        if (running)
            return;

        StartCoroutine(RunQTE(enemyArea));
    }

    private IEnumerator RunQTE(EnemyAreaData enemyArea)
    {
        running = true;

        OnQTEStarted?.Invoke();

        Debug.Log($"Pressione {enemyArea.key}");

        float timer = 0f;

        while (timer < enemyArea.reactionTime)
        {
            if (Input.GetKeyDown(enemyArea.key))
            {
                OnQTESuccess?.Invoke();

                running = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        OnQTEFail?.Invoke();

        running = false;
    }
}