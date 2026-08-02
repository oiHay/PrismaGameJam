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
    
    [SerializeField] private KeyCode interactionButton = KeyCode.E;

    private bool _isClueOnRange;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista dentro do range");

            _isClueOnRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(interactionButton) && _isClueOnRange)
        {
            //Precisamos passar um ItemSO nesse AddItem.
            //PlayerInventory.Instance.AddItem(other);
            DebugMessage("Pista maneira");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            DebugMessage("pista saiu do range");
            _isClueOnRange = false;
        }
    }
}
