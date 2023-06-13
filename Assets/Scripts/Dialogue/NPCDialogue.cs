using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles detecting if a player is nearby, and if they are it displays the dialogue that the NPC has. If there are multiple dialogue options,
/// it allows for selection.
/// </summary>

public class NPCDialogue : MonoBehaviour
{
    DialogueState dialogueState = DialogueState.NotInRange;
    PlayerDialogue playerDialogue;
    Queue<string> dialogueText = new();
    Queue<string> dialogueSpeaker = new();
    [SerializeField] Dialogue dialogue;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckState();
        }
    }

    private void CheckState()
    {
        switch (dialogueState)
        {
            case DialogueState.InRange:
                StartDialogue();
                break;
            case DialogueState.InDialogue:
                if (dialogueText.Count == 0)
                {
                    dialogueState = DialogueState.InDialogueOptions;
                    break;
                }
                NextLine();
                break;
            case DialogueState.InDialogueOptions:
                for (int i = 0; i < playerDialogue.PlayerOptionsUI.Length; i++)
                {
                    playerDialogue.PlayerOptionsUI[i].gameObject.SetActive(true);
                    playerDialogue.PlayerOptionsUI[i].text = dialogue.DialogueOptions[i].PlayerLine;
                }
                break;
        }
    }

    private void StartDialogue()
    {
        playerDialogue.DialogueUI.SetActive(enabled);
        foreach (var line in dialogue.IntroLines.Lines)
        {
            dialogueSpeaker.Enqueue(line.Speaker);
            dialogueText.Enqueue(line.Text);
        }
        NextLine();
        dialogueState = DialogueState.InDialogue;
    }

    private void NextLine()
    {
        playerDialogue.DialogueSpeaker.text = dialogueSpeaker.Dequeue();
        playerDialogue.DialogueText.text = dialogueText.Dequeue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out playerDialogue))
        {
            dialogueState = DialogueState.InRange;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueState = DialogueState.NotInRange;
    }
}

public enum DialogueState
{
    NotInRange,
    InRange,
    InDialogue,
    InDialogueOptions,
    ExitingDialogue
}