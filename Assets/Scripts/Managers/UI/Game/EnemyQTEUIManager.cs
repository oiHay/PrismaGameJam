using UnityEngine;

public class EnemyQTEUIManager : MonoBehaviour
{
    [SerializeField] private GameObject qteUIObject;

    private void Awake()
    {
        qteUIObject.SetActive(false);
    }

    private void OnEnable()
    {
        EnemyQTEManager.OnQteStarted += Show;
        EnemyQTEManager.OnQteFinished += Hide;
        EnemyArea.OnPlayerLeftArea += Hide;
    }

    private void OnDisable()
    {
        EnemyQTEManager.OnQteStarted -= Show;
        EnemyQTEManager.OnQteFinished -= Hide;
        EnemyArea.OnPlayerLeftArea -= Hide;
    }

    private void Show(EnemyArea enemyArea)
    {
        qteUIObject.SetActive(true);
    }

    private void Hide(EnemyArea enemyArea, bool success)
    {
        qteUIObject.SetActive(false);
    }

        private void Hide()
    {
        qteUIObject.SetActive(false);
    }
}