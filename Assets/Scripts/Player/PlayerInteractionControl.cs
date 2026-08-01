using System;
using UnityEngine;

public class PlayerInteractionControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pistas"))
        {
            Debug.Log("pista próxima");
        }
    }
}
