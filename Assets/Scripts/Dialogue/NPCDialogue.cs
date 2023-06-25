using System;
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
    public static event Action dialogueStarted;
    public static event Action dialogueEnded;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckState();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EndDialogue();
        }
    }

    private void ClearOptions()
    {
        foreach (var option in optionList)
        {
            Destroy(option);
        }
        optionList.Clear();
    }

    void CheckState()
    {
        switch (dialogueState)
        {
            case DialogueState.InRange:
                dialogueStarted?.Invoke();
                Time.timeScale = 0;
                playerDialogue.DialogueUI.SetActive(true);
                StartDialogue(dialogue.IntroLines);
                break;
            
            case DialogueState.InDialogue:
                if (dialogueText.Count == 0)
                {
                    if (dialogue.DialogueOptions.Length == 0)
                    {
                        ExitingDialogue();
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
                        optionList[optionList.Count - 1].GetComponentInChildren<Button>().onClick.AddListener(ExitingDialogue);
                    }
                    break;
                }
                NextLine();
                break;
            
            case DialogueState.ExitingDialogue:
                if (dialogueText.Count == 0)
                {
                    ExitingDialogue();
                    break;
                }
                NextLine();
                break;
        }
    }

    void ExitingDialogue()
    {
        if (dialogue.EndDialogue.NpcDialogue == null || dialogue.EndDialogue.NpcDialogue.Lines.Length == 0)
        {
            EndDialogue();
        }
        else if (dialogueState != DialogueState.ExitingDialogue)
        {
            StartDialogue(dialogue.EndDialogue.NpcDialogue);
            dialogueState = DialogueState.ExitingDialogue;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        ClearOptions();
        if (dialogueText.Count > 0)
        {
            dialogueSpeaker.Clear();
            dialogueText.Clear();
        }
        playerDialogue.DialogueText.SetActive(false);
        playerDialogue.DialogueUI.SetActive(false);
        playerDialogue.DialogueOptions.SetActive(false);
        dialogueState = DialogueState.InRange;
        dialogueEnded?.Invoke();
        Time.timeScale = 1;
    }

    void SelectOption(DialogueOptions dialogueOptions)
    {
        ClearOptions();
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