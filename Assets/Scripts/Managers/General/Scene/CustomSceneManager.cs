using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    #region Scene

    public static void LoadNextScene() // Método para passar para a próxima fase
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void MainMenu() // Método para direcionar o jogador ao Menu Inicial
    {
        Resume(GameState.Menu);
        SceneManager.LoadScene("MainMenu");
    }

    public static void QuitGame() // Método para fechar o jogo
    {
        Resume();
        Application.Quit();
    }

    #endregion

    #region Pause

    public static void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    public static void Resume(GameState state=GameState.Play)
    {
        IsPaused = false;
        Time.timeScale = 1f;
        GameManager.Instance.ChangeState(state);
    }

    #endregion

    #region GameScene

    public static void GoToAssembleia()
    {
        SceneManager.LoadScene("Assembleia");
    }

    public static void GoToScroller()
    {
        // SceneManager.LoadScene("Botar nome da cena quando tiver");
    }

    #endregion
}