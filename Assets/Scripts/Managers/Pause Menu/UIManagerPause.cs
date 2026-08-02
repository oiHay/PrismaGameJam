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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CustomSceneManager.IsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        DebugMessage("Jogo pausado.");

        CustomSceneManager.Pause();
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        DebugMessage("Jogo retomado.");

        CustomSceneManager.Resume();
        pauseUI.SetActive(false);
        exitConfirmationPanel.SetActive(false);
    }

    public void TryQuit()
    {
        exitConfirmationPanel.SetActive(true);
    }
}
