using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueOption", menuName = "Dialogue/Option")]
public class DialogueOptionSO : ScriptableObject
{
    public string displayText;
    public string[] requiredFlags;
    public string[] blockedByFlags;
    public string[] flagsToSetOnComplete;

    public DialogueLineSO[] conversation;
}

[Serializable]
public class DialogueLineSO
{
    public string speakerName;
    [TextArea] public string text;
}
