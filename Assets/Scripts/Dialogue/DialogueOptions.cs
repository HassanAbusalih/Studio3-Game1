using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOptions
{
    [SerializeField] private string playerLine;
    [SerializeField] private DialogueData npcDialogue;
    public string PlayerLine { get => playerLine; }
    public DialogueData NpcDialogue { get => npcDialogue; }
}
