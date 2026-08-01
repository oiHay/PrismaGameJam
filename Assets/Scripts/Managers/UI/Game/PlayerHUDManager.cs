using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementControl playerControl;
    [SerializeField] private Slider staminaBar;

    private void OnEnable() => playerControl.OnStaminaChanged += UpdateStamina;
    private void OnDisable() => playerControl.OnStaminaChanged -= UpdateStamina;

    private void Start()
    {
        staminaBar.value = 1f;
    }

    private void UpdateStamina(float current, float max)
    {
        staminaBar.value = current / max;
    }
}
