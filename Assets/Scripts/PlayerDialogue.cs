using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] TextMeshProUGUI[] playerOptions;

    public GameObject DialogueUI { get => dialogueUI; }
    public TextMeshProUGUI DialogueText { get => dialogueText; }
    public TextMeshProUGUI[] PlayerOptionsUI { get => playerOptions; }
    public TextMeshProUGUI DialogueSpeaker { get => dialogueSpeaker; }
}
