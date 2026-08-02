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

    private void UpdateDetection(float detection)
    {
        if (IsHiding)
        {
            DetectionMeter = Mathf.MoveTowards(DetectionMeter, 0f, decayRate * detection);
            EvaluateState();
            OnMeterChanged?.Invoke(DetectionMeter);
            return;
        }

        float delta = 0f;
        bool anySource = false;
        bool onlyWalkGap = false;
        
        // Lanterna em lacuna
        bool flashlightContribution = FlashlightOn && IsInGap;
        if (flashlightContribution)
        {
            delta += rateFlashlight * detection;
            anySource = true;
        }
        
        // Andar em lacuna
        if (Movement == MovementState.Walking && IsInGap)
        {
            delta += rateWalk * detection;
            anySource = true;
            onlyWalkGap = !flashlightContribution;
        }
        
        // Corre
        if (Movement == MovementState.Running)
        {
            delta += IsInGap ? rateRunGap * detection : rateRun * detection;
            anySource = true;
        }

        if (anySource)
        {
            DetectionMeter += delta;

            if (onlyWalkGap)
                DetectionMeter = Mathf.Min(DetectionMeter, alertCap);
        }

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
