using System;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationState : MonoBehaviour
{
    public static InvestigationState Instance { get; private set; }

    //flags podem ser algo tipo "Cuca_mencionou_objeto", "Tutu_falou_sobre_Cuca"
    //são usadas para ativar novas opções de diálogo com os suspeitos que dependeriam de contexto
    private HashSet<string> flags = new HashSet<string>();
    public event Action<string> OnFlagSet;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetFlag(string flag)
    {
        if (flags.Add(flag))
            OnFlagSet?.Invoke(flag);
    }

    public bool HasFlag(string flag) => flags.Contains(flag);
}
