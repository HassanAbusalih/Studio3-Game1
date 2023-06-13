using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Custom/Dialogue Data")]

public class DialogueData : ScriptableObject
{
    [SerializeField] DialogueLine[] lines;
    public DialogueLine[] Lines { get => lines; }
}

[System.Serializable]
public class DialogueLine
{
    [SerializeField] string speaker;
    [SerializeField] string text;
    public string Speaker { get => speaker; }
    public string Text { get => text; }
}
