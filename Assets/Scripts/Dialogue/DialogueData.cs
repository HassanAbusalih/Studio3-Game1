using UnityEngine;

/// <summary>
/// This class contains an array of DialogueLines, which themselves contain two strings: a line, and its speaker. 
/// </summary>

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
