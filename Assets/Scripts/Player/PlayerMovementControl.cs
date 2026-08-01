using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
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

   private void Update()
   {
      if (!_isGameActive) return;

      _horizontalInput = Input.GetAxis("Horizontal");

      moveDir = new Vector2(_horizontalInput, 0).normalized;
   }

   private void FixedUpdate()
   {
      if (!_isGameActive) return;
      HandleMoveInput(_horizontalInput);
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
