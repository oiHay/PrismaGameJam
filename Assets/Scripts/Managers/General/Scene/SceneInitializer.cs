using System;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
   [SerializeField] private GameState initialState = GameState.Play;

   private void Start()
   {
      GameManager.Instance.ChangeState(initialState);
   }
}
