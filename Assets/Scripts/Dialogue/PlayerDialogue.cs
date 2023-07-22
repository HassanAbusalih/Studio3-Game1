using TMPro;
using UnityEngine;

/// <summary>
/// This class holds references to all variables concerning dialogue. It is attached to the player, and is used to detect when the player comes within range for dialogue with an NPC.
/// </summary>
public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject dialogueText;
    [SerializeField] GameObject dialogueOptions;
    [SerializeField] GameObject dialogueOptionsPrefab;
    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] TextMeshProUGUI dialogueLine;
    [SerializeField][Range(0, 0.5f)] float fadeDuration = 0.15f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip line;
    [SerializeField] AudioClip start;

    public GameObject DialogueText { get => dialogueText; }
    public TextMeshProUGUI DialogueSpeaker { get => dialogueSpeaker; }
    public TextMeshProUGUI DialogueLine { get => dialogueLine; }
    public GameObject DialogueOptionsPrefab { get => dialogueOptionsPrefab; }
    public GameObject DialogueOptions { get => dialogueOptions; }
    public GameObject DialogueUI { get => dialogueUI; }
    public float FadeDuration { get => fadeDuration; }
    public AudioSource AudioSource { get => audioSource; }
    public AudioClip Line { get => line; }
    public AudioClip Start { get => start; }
}
