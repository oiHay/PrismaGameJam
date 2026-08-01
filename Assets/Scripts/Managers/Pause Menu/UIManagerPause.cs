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

    [SerializeField] private GameObject pauseUI;

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
    }
}
