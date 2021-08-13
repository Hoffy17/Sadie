using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed;

    [SerializeField] private bool playerSpeakingFirst;

    [Header("Dialogue - Text Mesh Pro")]
    [SerializeField] private TextMeshProUGUI playerDialogueText;
    [SerializeField] private TextMeshProUGUI nPCDialogueText;

    [Header("Dialogue - Animation Controllers")]
    [SerializeField] private Animator playerSpeechBubbleAnimator;
    [SerializeField] private Animator nPCSpeechBubbleAnimator;

    //[Header("Dialogue - UI Audio Source")]
    //[SerializeField] private AudioSource uIAudioSource;

    [Header("Dialogue - Sentences")]
    [TextArea]
    [SerializeField] private string[] playerDialogueSentences;
    [TextArea]
    [SerializeField] private string[] nPCDialogueSentences;

    private Movement playerMovement;

    private bool dialogueStarted;
    private bool playerDialogueEnded;
    private bool nPCDialogueEnded;

    private int playerIndex;
    private int nPCIndex;

    private float speechBubbleAnimationDelay = 0.6f;

    private void Start()
    {
        playerMovement = FindObjectOfType<Movement>();
        playerDialogueEnded = false;
        nPCDialogueEnded = false;
    }

    public void TriggerStartDialogue()
    {
        StartCoroutine(StartDialogue());
    }

    private void Update()
    {
        if (playerDialogueEnded == true)
            if (Input.GetKeyDown(KeyCode.E))
                TriggerContinueNPCDialogue();

        if (nPCDialogueEnded == true)
            if (Input.GetKeyDown(KeyCode.E))
                TriggerContinuePlayerDialogue();
    }

    public IEnumerator StartDialogue()
    {
        playerMovement.ToggleConversation();

        if (playerSpeakingFirst)
        {
            playerSpeechBubbleAnimator.SetTrigger("Open");

            yield return new WaitForSeconds(speechBubbleAnimationDelay);
            StartCoroutine(TypePlayerDialogue());
        }
        else
        {
            nPCSpeechBubbleAnimator.SetTrigger("Open");

            yield return new WaitForSeconds(speechBubbleAnimationDelay);
            StartCoroutine(TypeNPCDialogue());
        }
    }

    private IEnumerator TypePlayerDialogue()
    {
        foreach (char letter in playerDialogueSentences[playerIndex].ToCharArray())
        {
            playerDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        playerDialogueEnded = true;
    }

    private IEnumerator TypeNPCDialogue()
    {
        foreach (char letter in nPCDialogueSentences[nPCIndex].ToCharArray())
        {
            nPCDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        nPCDialogueEnded = true;
    }

    private IEnumerator ContinuePlayerDialogue()
    {
        nPCDialogueText.text = string.Empty;
        nPCSpeechBubbleAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        playerDialogueText.text = string.Empty;
        playerSpeechBubbleAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        if (dialogueStarted)
            playerIndex++;
        else
            dialogueStarted = true;

        StartCoroutine(TypePlayerDialogue());
    }

    private IEnumerator ContinueNPCDialogue()
    {
        playerDialogueText.text = string.Empty;
        playerSpeechBubbleAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        nPCDialogueText.text = string.Empty;
        nPCSpeechBubbleAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        if (dialogueStarted)
            nPCIndex++;
        else
            dialogueStarted = true;

        StartCoroutine(TypeNPCDialogue());
    }

    private void TriggerContinuePlayerDialogue()
    {
        //uIAudioSource.Play();

        nPCDialogueEnded = false;

        if (playerIndex >= playerDialogueSentences.Length - 1)
        {
            nPCDialogueText.text = string.Empty;
            nPCSpeechBubbleAnimator.SetTrigger("Close");
            playerDialogueEnded = false;
            dialogueStarted = false;
            playerIndex = 0;
            nPCIndex = 0;

            playerMovement.ToggleConversation();
        }
        else
            StartCoroutine(ContinuePlayerDialogue());
    }

    private void TriggerContinueNPCDialogue()
    {
        //uIAudioSource.Play();

        playerDialogueEnded = false;

        if (nPCIndex >= nPCDialogueSentences.Length - 1)
        {
            playerDialogueText.text = string.Empty;
            playerSpeechBubbleAnimator.SetTrigger("Close");
            nPCDialogueEnded = false;
            dialogueStarted = false;
            playerIndex = 0;
            nPCIndex = 0;

            playerMovement.ToggleConversation();
        }
        else
            StartCoroutine(ContinueNPCDialogue());
    }
}
