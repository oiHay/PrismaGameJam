using Unity.Cinemachine;
using UnityEngine;

public class InteractionHUDTracker : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerInteractionControl interactionControl;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform hudIcon;

    [Header("Offset")] 
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        
        if (hudIcon != null)
            hudIcon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.AddListener(AtualizarPosIcon);
        
        if (interactionControl != null)
            interactionControl.OnPistaRangeChanged += HandleClueRangeChanged;
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(AtualizarPosIcon);
        
        if (interactionControl != null)
            interactionControl.OnPistaRangeChanged -= HandleClueRangeChanged;
    }
    
    private void HandleClueRangeChanged(bool inRange)
    {
        if (hudIcon != null)
            hudIcon.gameObject.SetActive(inRange);
    }

    private void AtualizarPosIcon(CinemachineBrain brain)
    {
        if (playerTransform == null || hudIcon == null || _mainCamera == null) return;

        Vector3 targetPos = playerTransform.position + offset;
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(targetPos);

        hudIcon.position = screenPos;
    }
}
