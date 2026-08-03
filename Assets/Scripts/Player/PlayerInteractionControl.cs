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
    
    private CollectibleItem _clueInRange;
    private bool _isDoorOnRange;

    public event Action<bool> OnPistaRangeChanged;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista dentro do range");

            _clueInRange = other.GetComponent<CollectibleItem>();
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
        if (Input.GetKeyDown(interactionButton) && _clueInRange != null)
        {
            DebugMessage("Pista maneira");

            PlayerInventory.Instance.AddItem(_clueInRange.Item);
            AudioManager.Instance.PlaySfx(findCLue);

            Destroy(_clueInRange.gameObject);
            _clueInRange = null;
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

            _clueInRange = null;
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
