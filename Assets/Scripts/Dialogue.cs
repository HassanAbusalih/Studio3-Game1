using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
