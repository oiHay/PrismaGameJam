using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameStateEventSO gameStateEventSo;

    private PlayerMovementControl _control;

    private void Awake()
    {
        _control = GetComponent<PlayerMovementControl>();
    }

    private void OnEnable()
    {
        gameStateEventSo.OnRaised += HandleStateChanged;
    }

    private void OnDisable()
    {
        gameStateEventSo.OnRaised -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        Time.timeScale = state == GameState.Pause ? 0f : 1f;
        
        _control.SetGameState(state);
    }

    // Fazer quando tiver o estado de game over
    // private void HandlePlayerDeath()
    // {
    //     
    // }
}
