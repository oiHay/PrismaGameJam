using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
   [Header("References")] [SerializeField]
   private PlayerDetectionSystem detectionSystem;
   
   [Header("Move Speed")]
   [SerializeField] private float moveSpeed;
   [SerializeField] private float sprintFactor = 1.5f;
   [SerializeField] private float maxStamina;
   [SerializeField] private float sprintCost;
   [SerializeField] private float chargeRate;
   [SerializeField] private float rechargeTime = 1f;

   private Rigidbody2D _playerRb;
   private float _horizontalInput;
   private bool _isGameActive;
   private bool _isHidden;
   private float _currentStamina;
   private bool _isRunning;
   private Coroutine _rechargeStamina;

   public event Action<float, float> OnStaminaChanged;
   [HideInInspector] public Vector2 moveDir;
   
   private void Awake()
   {
      _playerRb = GetComponent<Rigidbody2D>();
   }

   private void Start()
   {
      _currentStamina = maxStamina;
   }

   public void SetGameState(GameState state)
   { 
      _isGameActive = state == GameState.Play;
   }

   public void SetHidden(bool hidden)
   {
      _isHidden = hidden;

      if (hidden)
      {
         _horizontalInput = 0f;
         moveDir = Vector2.zero;
         _playerRb.linearVelocity = Vector2.zero;
      }
   }

   private void Update()
   {
      if (!_isGameActive || _isHidden) return;

      _horizontalInput = Input.GetAxis("Horizontal");
      moveDir = new Vector2(_horizontalInput, 0).normalized;

      UpdateDetectionMovementState();
   }

   private void FixedUpdate()
   {
      if (!_isGameActive || _isHidden) return;
      HandleMoveInput(_horizontalInput);
   }

   private void UpdateDetectionMovementState()
   {
      if (detectionSystem ==  null) return;

      bool isMoving = Mathf.Abs(_horizontalInput) > 0.01f;
      bool wantsRun = Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0f;

      if (isMoving && wantsRun)
         detectionSystem.Movement = MovementState.Running;
      else if (isMoving)
         detectionSystem.Movement = MovementState.Walking;
      else
         detectionSystem.Movement = MovementState.Idle;
   }

   private void HandleMoveInput(float horizontalInput)
   {
      _isRunning = Input.GetKey(KeyCode.LeftShift);

      if (_isRunning && _currentStamina > 0)
      {
         _playerRb.AddForce(Vector2.right * ((moveSpeed * horizontalInput) * sprintFactor));
         
         _currentStamina -= sprintCost * Time.deltaTime;
         if (_currentStamina <= 0)
            _currentStamina = 0;
         
         OnStaminaChanged?.Invoke(_currentStamina, maxStamina);
         
         if (_rechargeStamina != null) StopCoroutine(_rechargeStamina);
         _rechargeStamina = StartCoroutine(RechargeStamina());
      }
      else
      {
         _playerRb.AddForce(Vector2.right * (moveSpeed * horizontalInput));
      }
   }

   private IEnumerator RechargeStamina()
   {
      yield return new WaitForSeconds(rechargeTime);

      while (_currentStamina < maxStamina)
      {
         _currentStamina += chargeRate / 10f;

         if (_currentStamina > maxStamina)
            _currentStamina = maxStamina;
         
         OnStaminaChanged?.Invoke(_currentStamina, maxStamina);
         
         yield return new WaitForSeconds(.1f);
      }
   }
}
