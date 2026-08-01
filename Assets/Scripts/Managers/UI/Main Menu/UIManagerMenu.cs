using UnityEngine;

public class UIManagerMenu : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion
    
    private GameObject _currentPanel; // variável que salva qual o painel aberto no momento
    
    public void Play() // Método para iniciar o jogo - click do botão no menu inicial
    {
        CustomSceneManager.LoadNextScene();
    }

    public void Quit() // Método para sair do jogo
    {
        DebugMessage("Botão de sair foi clicado");
        
        CustomSceneManager.QuitGame();
        
        #if UNITY_EDITOR // Serve para verificar se o método funciona
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void OpenPanel(GameObject panel) // Método para abrir o painel clicado
    {
        if (panel == null) return; // Se não tiver painel, não faz nada
        
        if(_currentPanel != null) // Se não tiver painel, o painel é fechado
            _currentPanel.SetActive(false); 

        _currentPanel = panel;
        _currentPanel.SetActive(true); // Ativa o painel determinado
    }

    public void CloseCurrentPanel() // Método para fechar o painel
    {
        if(_currentPanel == null) return; // Se não tiver painel ativo, nada acontece
        
        _currentPanel.SetActive(false); // Fecha o painel
        _currentPanel = null; // Variável do painel atual é "zerado"
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Se o player clicar ESC, o painel é fechado, caso esteja aberto
        {
            CloseCurrentPanel();
        }
    }
}
