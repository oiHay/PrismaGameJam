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
        EnemyQTEManager.OnQTEStarted += Show;
        EnemyQTEManager.OnQTESuccess += Hide;
        EnemyQTEManager.OnQTEFail += Hide;
    }

    private void OnDisable()
    {
        EnemyQTEManager.OnQTEStarted -= Show;
        EnemyQTEManager.OnQTESuccess -= Hide;
        EnemyQTEManager.OnQTEFail -= Hide;
    }

    private void Show()
    {
        qteUIObject.SetActive(true);
    }

    private void Hide()
    {
        qteUIObject.SetActive(false);
    }
}