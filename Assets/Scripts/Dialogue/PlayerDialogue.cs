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
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] TextMeshProUGUI[] playerOptionsUI;

    public GameObject DialogueUI { get => dialogueUI; }
    public TextMeshProUGUI DialogueSpeaker { get => dialogueSpeaker; }
    public TextMeshProUGUI DialogueText { get => dialogueText; }
    public TextMeshProUGUI[] PlayerOptionsUI { get => playerOptionsUI; }
}
