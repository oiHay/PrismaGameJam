using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static void LoadNextScene() // Método para passar para a próxima fase
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void MainMenu() // Método para direcionar o jogador ao Menu Inicial
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void QuitGame() // Método para fechar o jogo
    {
        Application.Quit();
    }
}
