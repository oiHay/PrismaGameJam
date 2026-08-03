using System;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerAlertState { Safe, Alert, Danger }
public enum MovementState { Idle, Walking, Running }

public class PlayerDetectionSystem : MonoBehaviour
{
    [Header("Thresholds")] 
    [SerializeField] private float safeToAlertThreshold = 30f;
    [SerializeField] private float alertToSafeThreshold = 20f;
    [SerializeField] private float alertToDangerThreshold = 70f;
    [SerializeField] private float dangerToAlertThreshold = 55f;
    [SerializeField] private float alertCap = 69.9f;

    [Header("Taxa de acúmulo")] 
    [SerializeField] private float rateFlashlight = 15f;
    [SerializeField] private float rateWalk = 10f;
    [SerializeField] private float rateRun = 12f;
    [SerializeField] private float rateRunGap = 40f;
    [SerializeField] private float rateEnemyAreaBase = 8f;
    [SerializeField] private float decayRate = 18f;

    public float DetectionMeter { get; private set; } = 0f;
    public PlayerAlertState CurrentState { get; private set; } = PlayerAlertState.Safe;
    public bool IsInGap { get; private set;  }
    public bool FlashlightOn { get; set; }
    public MovementState Movement { get; set; } = MovementState.Idle;
    public bool IsHiding { get; private set; }

    public event Action<PlayerAlertState, PlayerAlertState> OnStateChanged;
    public event Action<float> OnMeterChanged;
    public event Action OnHideBlocked;

    private int _gapOverlapCount = 0;

    private void Update()
    {
        UpdateDetection(Time.deltaTime);
    }

    public void SetInGap(bool entering)
    {
        _gapOverlapCount = Mathf.Max(0, _gapOverlapCount + (entering ? 1 : -1));
        IsInGap = _gapOverlapCount > 0;
    }

    private void UpdateDetection(float dt)
    {
        if (IsHiding)
        {
            DetectionMeter = Mathf.MoveTowards(DetectionMeter, 0f, decayRate * dt);
            EvaluateState();
            OnMeterChanged?.Invoke(DetectionMeter);
            return;
        }

        float delta = 0f;
        bool anySource = false;

        bool flashlightActive = FlashlightOn;
        bool walkingActive = Movement == MovementState.Walking;
        bool runningActive = Movement == MovementState.Running;

        // Lanterna ligada sempre conta
        if (flashlightActive)
        {
            delta += rateFlashlight * dt;
            anySource = true;
        }

        // Andar sempre conta
        if (walkingActive)
        {
            delta += rateWalk * dt;
            anySource = true;
        }

        // Correr sempre conta, e ainda mais dentro da área do inimigo
        if (runningActive)
        {
            delta += (IsInGap ? rateRunGap : rateRun) * dt;
            anySource = true;
        }

        // Estar na área de ataque do inimigo soma sozinho, mesmo parado e sem lanterna
        if (IsInGap)
        {
            delta += rateEnemyAreaBase * dt;
            anySource = true;
        }

        if (anySource)
        {
            DetectionMeter += delta;

            // Só andar, sem lanterna, correr ou área de risco: segura antes do Danger
            bool onlySoftWalk = walkingActive && !flashlightActive && !runningActive && !IsInGap;
            if (onlySoftWalk)
                DetectionMeter = Mathf.Min(DetectionMeter, alertCap);
        }
        else
            DetectionMeter = Mathf.MoveTowards(DetectionMeter, 0f, decayRate * dt);

        DetectionMeter = Mathf.Clamp(DetectionMeter, 0f, 100f);
        EvaluateState();
        OnMeterChanged?.Invoke(DetectionMeter);
    }

    private void EvaluateState()
    {
        PlayerAlertState newState = CurrentState;

        switch (CurrentState)
        {
            case PlayerAlertState.Safe:
                if (DetectionMeter >= safeToAlertThreshold)
                    newState = PlayerAlertState.Alert;
                break;
            
            case PlayerAlertState.Alert:
                if (DetectionMeter >= alertToDangerThreshold)
                    newState = PlayerAlertState.Danger;
                else if (DetectionMeter < alertToSafeThreshold)
                    newState = PlayerAlertState.Safe;
                break;
            
            case PlayerAlertState.Danger:
                if (DetectionMeter < dangerToAlertThreshold)
                    newState = PlayerAlertState.Alert;
                break;
        }

        if (newState != CurrentState)
            SetState(newState);
    }

    private void SetState(PlayerAlertState newState)
    {
        var lastState = CurrentState;
        CurrentState = newState;
        OnStateChanged?.Invoke(lastState, newState);
    }

    private void ForceMeterFloorForDanger()
    {
        DetectionMeter = Mathf.Max(DetectionMeter, alertToDangerThreshold);
    }

    #region Esconder

    public bool TryEnterHide()
    {
        if (CurrentState == PlayerAlertState.Danger)
        {
            OnHideBlocked?.Invoke();
            return false;
        }

        IsHiding = true;
        return true;
    }

    public void ExitHide()
    {
        if (!IsHiding) return;
        IsHiding = false;
        
        if (CurrentState == PlayerAlertState.Safe)
            return;

        if (IsInGap)
        {
            SetState(PlayerAlertState.Danger);
            ForceMeterFloorForDanger();
        }
    }

    #endregion
}
