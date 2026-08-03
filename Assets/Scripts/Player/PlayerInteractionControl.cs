using System;
using UnityEngine;

public class PlayerInteractionControl : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion

    [Header("Audio")] 
    [SerializeField] private AudioClip findCLue;
    
    [Header("Input")]
    [SerializeField] private KeyCode interactionButton = KeyCode.E;
    
    private bool _isClueOnRange;
    private bool _isDoorOnRange;

    public event Action<bool> OnPistaRangeChanged;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista dentro do range");

            _isClueOnRange = true;
            OnPistaRangeChanged?.Invoke(true);
        }

        if (other.CompareTag("Door"))
        {
            DebugMessage("porta dentro do range");
            
            _isDoorOnRange = true;
            OnPistaRangeChanged?.Invoke(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(interactionButton) && _isClueOnRange)
        {
            DebugMessage("Pista maneira");
            
            //Precisamos passar um ItemSO nesse AddItem.
            //PlayerInventory.Instance.AddItem(other);
            AudioManager.Instance.PlaySfx(findCLue);
        }
        
        if (Input.GetKeyDown(interactionButton) && _isDoorOnRange)
        {
            DebugMessage("Player abrindo a porta");
            
            CustomSceneManager.GoToAssembleia();
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista saiu do range");
            
            _isClueOnRange = false;
            OnPistaRangeChanged?.Invoke(false);
        }
        
        if (other.CompareTag("Door"))
        {
            DebugMessage("porta saiu do range");
            
            _isDoorOnRange = false;
            OnPistaRangeChanged?.Invoke(false);
        }
    }
}
