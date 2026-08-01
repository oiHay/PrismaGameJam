using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EmotionSprite
{
    public Emotion emotion;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "NewSuspect", menuName = "Dialogue/Suspect")]
public class SuspectSO : ScriptableObject
{
    public string suspectName;
    public List<EmotionSprite> portraits;
    public List<DialogueOptionSO> dialogueOptions;

    public Sprite GetSprite(Emotion emotion)
    {
        var match = portraits.Find(p => p.emotion == emotion);
        if (match != null)
            return match.sprite;

        Debug.LogWarning($"{suspectName} não tem sprite pra emoção {emotion}, usando Neutral.");
        return portraits.Find(p => p.emotion == Emotion.Neutral)?.sprite;
    }

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