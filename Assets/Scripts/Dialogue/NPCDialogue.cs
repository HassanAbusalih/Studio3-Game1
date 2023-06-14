using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles detecting if a player is nearby, and if they are and the player presses a button, it displays the dialogue that the NPC has. 
/// If there are multiple dialogue options, it allows for selection between them, with one option exiting the dialogues.
/// </summary>

public class NPCDialogue : MonoBehaviour
{
    DialogueState dialogueState = DialogueState.NotInRange;
    PlayerDialogue playerDialogue;
    Queue<string> dialogueText = new();
    Queue<string> dialogueSpeaker = new();
    [SerializeField] Dialogue dialogue;
    List<GameObject> optionList = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckState();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            foreach (var option in optionList)
            {
                Destroy(option);
            }
            optionList.Clear();
            ExitDialogue();
        }
    }

    void CheckState()
    {
        switch (dialogueState)
        {
            case DialogueState.InRange:
                playerDialogue.DialogueUI.SetActive(true);
                StartDialogue(dialogue.IntroLines);
                break;
            
            case DialogueState.InDialogue:
                if (dialogueText.Count == 0)
                {
                    if (dialogue.DialogueOptions.Length == 0)
                    {
                        ExitDialogue();
                    }
                    else
                    {
                        dialogueState = DialogueState.InDialogueOptions;
                        playerDialogue.DialogueText.SetActive(false);
                        playerDialogue.DialogueOptions.SetActive(true);
                        foreach (var option in dialogue.DialogueOptions)
                        {
                            optionList.Add(Instantiate(playerDialogue.DialogueOptionsPrefab, playerDialogue.DialogueOptions.transform));
                            optionList[optionList.Count - 1].GetComponent<TextMeshProUGUI>().text = option.PlayerLine;
                            optionList[optionList.Count - 1].GetComponentInChildren<Button>().onClick.AddListener(() => SelectOption(option));
                        }
                        optionList.Add(Instantiate(playerDialogue.DialogueOptionsPrefab, playerDialogue.DialogueOptions.transform));
                        optionList[optionList.Count - 1].GetComponent<TextMeshProUGUI>().text = dialogue.EndDialogue.PlayerLine;
                        optionList[optionList.Count - 1].GetComponentInChildren<Button>().onClick.AddListener(ExitDialogue);
                    }
                    break;
                }
                NextLine();
                break;
            
            case DialogueState.ExitingDialogue:
                if (dialogueText.Count == 0)
                {
                    ExitDialogue();
                    break;
                }
                NextLine();
                break;
        }
    }

    void ExitDialogue()
    {
        playerDialogue.DialogueText.SetActive(false);
        playerDialogue.DialogueUI.SetActive(false);
        playerDialogue.DialogueOptions.SetActive(false);
        dialogueState = DialogueState.InRange;
    }

    void SelectOption(DialogueOptions dialogueOptions)
    {
        foreach(var option in optionList)
        {
            Destroy(option);
        }
        optionList.Clear();
        StartDialogue(dialogueOptions.NpcDialogue);
    }

    void StartDialogue(DialogueData dialogueData)
    {
        playerDialogue.DialogueOptions.SetActive(false);
        playerDialogue.DialogueText.SetActive(true);
        foreach (var line in dialogueData.Lines)
        {
            dialogueSpeaker.Enqueue(line.Speaker);
            dialogueText.Enqueue(line.Text);
        }
        NextLine();
        dialogueState = DialogueState.InDialogue;
    }

    private void NextLine()
    {
        if (dialogueText.TryPeek(out string result))
        {
            playerDialogue.DialogueSpeaker.text = dialogueSpeaker.Dequeue();
            playerDialogue.DialogueLine.text = dialogueText.Dequeue();
        }
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