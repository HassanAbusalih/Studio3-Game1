using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles detecting if a player is nearby, and if they are it displays the dialogue that the NPC has. If there are multiple dialogue options,
/// it allows for selection.
/// </summary>

public class NPCDialogue : MonoBehaviour
{
    PlayerDialogue playerDialogue;
    Queue<string> dialogueText = new Queue<string>();
    Queue<string> dialogueSpeaker = new Queue<string>();
    [SerializeField] Dialogue dialogues;
    bool inDialogue;
    bool inDialogueOptions;

    void Start()
    {
        playerDialogue = FindObjectOfType<PlayerDialogue>();
    }

    void Update()
    {
        if (inDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueText.Count > 0)
            {
                playerDialogue.DialogueText.text = dialogueText.Dequeue();
                playerDialogue.DialogueSpeaker.text = dialogueSpeaker.Dequeue();
                if (dialogueText.Count == 0) 
                {
                    inDialogue = false;
                    inDialogueOptions = true;
                }
            }
            else
            {

            }
        }
        else if (inDialogueOptions && Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0;  i < playerDialogue.PlayerOptionsUI.Length; i++)
            {
                playerDialogue.PlayerOptionsUI[i].gameObject.SetActive(true);
                playerDialogue.PlayerOptionsUI[i].text = dialogues.DialogueOptions[i].PlayerLine;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out playerDialogue))
        {
            playerDialogue.DialogueUI.SetActive(enabled);
            inDialogue = true;
            foreach(var line in dialogues.IntroLines.Lines) 
            {
                dialogueSpeaker.Enqueue(line.Speaker);
                dialogueText.Enqueue(line.Text);
            }
            playerDialogue.DialogueText.text = dialogueText.Dequeue();
            playerDialogue.DialogueSpeaker.text = dialogueSpeaker.Dequeue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
