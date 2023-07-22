using UnityEngine;

/// <summary>
/// This class contains a string which is the text to display for the dialogue option, and a DialogueData which is the dialogue corresponding to this option.
/// </summary>

[System.Serializable]
public class DialogueOptions
{
    [SerializeField] private string playerLine;
    [SerializeField] private DialogueData npcDialogue;
    public string PlayerLine { get => playerLine; }
    public DialogueData NpcDialogue { get => npcDialogue; }
}
