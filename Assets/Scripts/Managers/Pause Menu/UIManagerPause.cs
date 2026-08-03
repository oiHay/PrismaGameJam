using UnityEngine;

public class UIManagerPause : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if (debugMode)
            Debug.Log(message);
    }

    #endregion
    
    [Header("Panels")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject exitConfirmationPanel;
    [SerializeField] private InventoryUI inventoryPanel;
    [SerializeField] private GameObject gameoverPanel;

    [Header("Audio")]
    [SerializeField] private AudioClip inventorySound;
    [SerializeField] private AudioClip openPanel;
    [SerializeField] private AudioClip closePanel;

    private bool _isInventoryOpen;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CustomSceneManager.IsPaused)
                Resume();
            else
                Pause();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!_isInventoryOpen)
            {
                inventoryPanel.OpenWithoutDialogue();
                _isInventoryOpen = true;
                AudioManager.PlaySound(inventorySound);
            }
            else
            {
                inventoryPanel.Close();
                _isInventoryOpen = false;
                AudioManager.PlaySound(inventorySound);
            }
        }
    }

    public void Pause()
    {
        DebugMessage("Jogo pausado.");

        CustomSceneManager.Pause();
        pauseUI.SetActive(true);
        AudioManager.PlaySound(openPanel);
    }

    public void Resume()
    {
        DebugMessage("Jogo retomado.");

        CustomSceneManager.Resume();
        pauseUI.SetActive(false);
        exitConfirmationPanel.SetActive(false);
        AudioManager.PlaySound(closePanel);
    }

    public void TryQuit()
    {
        exitConfirmationPanel.SetActive(true);
    }

    public void GameOver()
    {
        GameManager.Instance.ChangeState(GameState.GameOver);
        gameoverPanel.SetActive(true);
    }
}
