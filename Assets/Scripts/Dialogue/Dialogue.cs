using UnityEngine;

/// <summary>
/// This class is a scriptable object that holds everything concerning dialogue with a specific NPC. This includes intro lines that are played when dialogue is initiated, an array of dialogue options (if applicable),
/// and an option to end the dialogue (if needed).
/// </summary>

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Custom/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] DialogueData introLines;
    [SerializeField] DialogueOptions[] dialogueOptions;
    [SerializeField] DialogueOptions endDialogue;

    public DialogueData IntroLines { get => introLines; }
    public DialogueOptions[] DialogueOptions { get => dialogueOptions; }
    public DialogueOptions EndDialogue { get => endDialogue; }
}
