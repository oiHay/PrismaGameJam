using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion

    [SerializeField] private GameStateEventSO gameStateEventSo; // Referencia direta ao GameStateEventSO, permite que o código saiba qual é o estado atual do jogo e que o mesmo possa ser alterado
    [SerializeField] private GameState initialState;
    
    public static GameManager Instance { get; private set; }

    private void Awake() // Singleton, permite que o game object que possui esse código persista durante loads da cena
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeState(initialState);
    }

    public void ChangeState(GameState newState) // público para permitir que outros scripts alterem o estado
    {
        gameStateEventSo.Raise(newState); // Método que permite que o gameManager mude o valor do estado atual da cena
        
        DebugMessage("Estado Atual do jogo: " + gameStateEventSo.gameStateAtual.ToString()); // Debug para saber o estado atual do jogo
    }
}
