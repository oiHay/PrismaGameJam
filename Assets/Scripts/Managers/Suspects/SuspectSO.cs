using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSuspect", menuName = "Dialogue/Suspect")]
public class SuspectSO : ScriptableObject
{
    public string suspectName;
    public List<DialogueOptionSO> dialogueOptions;

    public List<DialogueOptionSO> GetAvailableOptions()
    {
        var result = new List<DialogueOptionSO>();
        foreach (var opt in dialogueOptions)
        {
            bool hasRequired = opt.requiredFlags.All(f => InvestigationState.Instance.HasFlag(f));
            bool isBlocked = opt.blockedByFlags.Any(f => InvestigationState.Instance.HasFlag(f));
            if (hasRequired && !isBlocked)
                result.Add(opt);
        }
        return result;
    }
}
