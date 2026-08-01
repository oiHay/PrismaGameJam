using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
   [SerializeField] private float moveSpeed;

   private Rigidbody2D _playerRb;
   private float _horizontalInput;
   private bool _isGameActive;

   private void Awake()
   {
      _playerRb = GetComponent<Rigidbody2D>();
   }

   public void SetGameState(GameState state)
   { 
      _isGameActive = state == GameState.Play;
   }

   private void Update()
   {
      if(!_isGameActive) return;

      _horizontalInput = Input.GetAxis("Horizontal");
   }

   private void FixedUpdate()
   {
      if(!_isGameActive) return;
      HandleMoveInput(_horizontalInput);
   }

   private void HandleMoveInput(float horizontalInput)
   {
      _playerRb.AddForce(Vector2.right * (moveSpeed * horizontalInput));
   }
}
