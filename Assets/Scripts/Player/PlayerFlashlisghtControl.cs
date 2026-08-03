using UnityEngine;

public class PlayerFlashlisghtControl : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerDetectionSystem detectionSystem;
    [SerializeField] private GameObject lamparina;

    [Header("Audio")] 
    [SerializeField] private AudioClip turnLightOnClip;
    [SerializeField] private AudioClip turnLightOffClip;
    
    [Header("Parâmetros da Luz")]
    [SerializeField] private KeyCode turnLightOn = KeyCode.X;
    [SerializeField] private float offSetX;

    private bool _isLightActive = false;
    private PlayerMovementControl _movementControl;

    private void Start()
    {
        _movementControl = GetComponent<PlayerMovementControl>();
    }

    private void Update()
    {
        TurnOnLight();

        if (_movementControl == null) return;

        lamparina.transform.position = _movementControl.moveDir.x switch
        {
            > 0 => transform.position + new Vector3(offSetX, 0, 0),
            < 0 => transform.position + new Vector3(-offSetX, 0, 0),
            _   => transform.position + new Vector3(offSetX, 0, 0),
        };
    }
    
    private void TurnOnLight()
    {
        if (Input.GetKeyDown(turnLightOn) && !_isLightActive)
        {
            lamparina.SetActive(true);
            _isLightActive = true;
            detectionSystem.FlashlightOn = true;
            AudioManager.Instance.PlaySfx(turnLightOnClip);
        }
        else if (Input.GetKeyDown(turnLightOn) && _isLightActive)
        {
            lamparina.SetActive(false);
            _isLightActive = false;
            detectionSystem.FlashlightOn = false;
            AudioManager.Instance.PlaySfx(turnLightOffClip);
        }
            
    }
}
