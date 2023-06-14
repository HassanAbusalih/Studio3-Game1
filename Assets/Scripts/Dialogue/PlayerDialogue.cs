using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class holds references to all UI elements concerning dialogue. It is attached to the player, and is used to detect when the player comes within range for dialogue with an NPC.
/// </summary>
public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject dialogueText;
    [SerializeField] GameObject dialogueOptions;
    [SerializeField] GameObject dialogueOptionsPrefab;
    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] TextMeshProUGUI dialogueLine;

    public GameObject DialogueText { get => dialogueText; }
    public TextMeshProUGUI DialogueSpeaker { get => dialogueSpeaker; }
    public TextMeshProUGUI DialogueLine { get => dialogueLine; }
    public GameObject DialogueOptionsPrefab { get => dialogueOptionsPrefab; }
    public GameObject DialogueOptions { get => dialogueOptions; }
    public GameObject DialogueUI { get => dialogueUI; }
}
