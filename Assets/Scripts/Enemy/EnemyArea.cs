using System;
using System.Collections;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    public static event Action<EnemyArea> OnPlayerDetected;
    public static event Action OnPlayerLeftArea;
    [SerializeField] private EnemyAreaData enemyArea;

    [Header("Ataque")]
    [SerializeField] private GameObject attackSprite;
    [SerializeField] private float attackDuration = 0.3f;

    private bool triggered;

    private void Awake()
    {
        attackSprite.SetActive(false);
    }

    private void OnEnable()
    {
        EnemyQTEManager.OnQteFinished += OnQTEFinished;
    }

    private void OnDisable()
    {
        EnemyQTEManager.OnQteFinished -= OnQTEFinished;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("EnemyDetect"))
            return;

        triggered = true;

        other.GetComponentInParent<PlayerDetectionSystem>()?.SetInGap(true);
        
        OnPlayerDetected?.Invoke(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("EnemyDetect"))

            return;

        triggered = false;
        
        other.GetComponentInParent<PlayerDetectionSystem>()?.SetInGap(false);
        
        OnPlayerLeftArea?.Invoke();
    }

    private void OnQTEFinished(EnemyArea area, bool success)
    {
        // Ignora eventos de outros inimigos
        if (area != this)
            return;

        StartCoroutine(Attack(success));
    }

    private IEnumerator Attack(bool success)
    {
        attackSprite.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackSprite.SetActive(false);

        if (!success)
        {
            UIManagerPause.Instance.GameOver();
        }
    }

    public EnemyAreaData Data => enemyArea;
}