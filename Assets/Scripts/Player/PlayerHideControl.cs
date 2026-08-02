using System;
using UnityEngine;

public class PlayerHideControl : MonoBehaviour
{
   [Header("References")] 
   [SerializeField] private PlayerDetectionSystem detectionSystem;

   [SerializeField] private PlayerMovementControl movementControl;

   [Header("Input")] 
   [SerializeField] private KeyCode hideKey = KeyCode.LeftControl;

   public bool IsHiding => detectionSystem != null && detectionSystem.IsHiding;

   public event Action OnHideEntered;
   public event Action OnHideExited;

   private void Update()
   {
      if (!Input.GetKeyDown(hideKey))return;

      if (!detectionSystem.IsHiding)
         TryHide();
      else
         StopHiding();
   }

   private void TryHide()
   {
      if (detectionSystem.TryEnterHide())
      {
         movementControl.SetHidden(true);
         OnHideEntered?.Invoke();
      }
   }

   private void StopHiding()
   {
      detectionSystem.ExitHide();
      movementControl.SetHidden(false);
      OnHideExited?.Invoke();
   }
}
