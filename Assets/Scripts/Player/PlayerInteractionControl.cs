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

    public event Action<bool> OnPistaRangeChanged;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista dentro do range");

            _isClueOnRange = true;
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista saiu do range");
            
            _isClueOnRange = false;
            OnPistaRangeChanged?.Invoke(false);
        }
    }
}
