using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
   [SerializeField] private GameState initialState = GameState.Play;
   [SerializeField] private AudioClip sceneMusic;
   
   private void Start()
   {
      AudioManager.Instance.PlayMusic(sceneMusic);
      GameManager.Instance.ChangeState(initialState);
   }
}
