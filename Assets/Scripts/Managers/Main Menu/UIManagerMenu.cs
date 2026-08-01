using UnityEngine;

public class UIManagerMenu : MonoBehaviour
{
    private GameObject _currentPanel;
    
    public void Play()
    {
        CustomSceneManager.LoadNextScene();
    }
    
    public void OpenPanel(GameObject panel)
    {
        if (panel == null) return;
        
        if(_currentPanel != null)
            _currentPanel.SetActive(false);

        _currentPanel = panel;
        _currentPanel.SetActive(true);
    }

    public void CloseCurrentPanel()
    {
        if(_currentPanel == null) return;
        
        _currentPanel.SetActive(false);
        _currentPanel = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCurrentPanel();
        }
    }
}
