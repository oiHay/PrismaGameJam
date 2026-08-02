using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetectionHUD : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerDetectionSystem detectionSystem;

    [Header("Medidor UI")] 
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient medidorGradient;

    [Header("Hide Icon")] 
    [SerializeField] private Image hideIcon;
    [SerializeField] private float hideEnableAlpha = 1f;
    [SerializeField] private float hideDisableAlpha = 0.3f;

    private void OnEnable()
    {
        if (detectionSystem == null) return;

        detectionSystem.OnMeterChanged += HandleMeterChanged;
        detectionSystem.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (detectionSystem == null) return;
        
        detectionSystem.OnMeterChanged -= HandleMeterChanged;
        detectionSystem.OnStateChanged -= HandleStateChanged;
    }

    private void Start()
    {
        if (detectionSystem == null) return;

        HandleMeterChanged(detectionSystem.DetectionMeter);
        HandleStateChanged(detectionSystem.CurrentState, detectionSystem.CurrentState);
    }

    private void HandleMeterChanged(float value)
    {
        if (fillImage == null) return;

        float normalized = value / 100f;
        fillImage.fillAmount = normalized;
        fillImage.color = medidorGradient.Evaluate(normalized);
    }

    private void HandleStateChanged(PlayerAlertState lastState, PlayerAlertState newState)
    {
        UpdateHideIcon(newState);
    }

    private void UpdateHideIcon(PlayerAlertState state)
    {
        if (hideIcon == null) return;

        bool canHide = state != PlayerAlertState.Danger;
        var color = hideIcon.color;
        color.a = canHide ? hideEnableAlpha : hideDisableAlpha;
        hideIcon.color = color;
    }
}
