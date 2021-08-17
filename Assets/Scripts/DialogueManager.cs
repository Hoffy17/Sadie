using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed;

    [SerializeField] private bool playerSpeakingFirst;
    [SerializeField] private bool triggerChapterEnd;

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

    private GameManager gameManager;
    private UIManager uIManager;
    private Movement playerMovement;

    private bool dialogueStarted;
    private bool playerDialogueEnded;
    private bool nPCDialogueEnded;

    private int playerIndex;
    private int nPCIndex;

    private float speechBubbleAnimationDelay = 0.6f;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uIManager = FindObjectOfType<UIManager>();
        playerMovement = FindObjectOfType<Movement>();
        playerDialogueEnded = false;
        nPCDialogueEnded = false;
    }

    //Function called from DialogueTrigger.cs
    public void TriggerStartDialogue()
    {
        StartCoroutine(StartDialogue());
    }

    private void Update()
    {
        //When the player finishes talking, press E to advance to NPC dialogue
        if (playerDialogueEnded == true)
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && gameManager.gameState == GameState.game)
                StartCoroutine(CheckContinueNPCDialogue());

        //When the NPC finishes talking, press E to advance to player dialogue
        if (nPCDialogueEnded == true)
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && gameManager.gameState == GameState.game)
                StartCoroutine(CheckContinuePlayerDialogue());
    }

    //Function called when dialogue begins
    public IEnumerator StartDialogue()
    {
        //Prevents the player from moving
        playerMovement.ToggleConversation();

        //Animates the speech bubbles
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

    //Types out the player's dialogue
    private IEnumerator TypePlayerDialogue()
    {
        foreach (char letter in playerDialogueSentences[playerIndex].ToCharArray())
        {
            playerDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        playerDialogueEnded = true;
    }

    //Types out the NPC's dialogue
    private IEnumerator TypeNPCDialogue()
    {
        foreach (char letter in nPCDialogueSentences[nPCIndex].ToCharArray())
        {
            nPCDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        nPCDialogueEnded = true;
    }

    //When the NPC's dialogue ends, check if the player has any more dialogue
    private IEnumerator CheckContinuePlayerDialogue()
    {
        //uIAudioSource.Play();

        nPCDialogueEnded = false;

        //If the player has no more dialogue, close the NPC's speech bubble
        //and allow the player to move again
        if (playerIndex >= playerDialogueSentences.Length - 1)
        {
            nPCDialogueText.text = string.Empty;
            nPCSpeechBubbleAnimator.SetTrigger("Close");
            yield return new WaitForSeconds(speechBubbleAnimationDelay);
            dialogueStarted = false;
            playerIndex = 0;
            nPCIndex = 0;

            playerMovement.ToggleConversation();

            if (triggerChapterEnd)
                uIManager.GetComponent<UIManager>().TriggerChapterEnd();
        }
        //If the player has more dialogue, continue player dialogue
        else
            StartCoroutine(ContinuePlayerDialogue());
    }

    //When the player's dialogue ends, check if the NPC has any more dialogue
    private IEnumerator CheckContinueNPCDialogue()
    {
        //uIAudioSource.Play();

        playerDialogueEnded = false;

        //If the NPC has no more dialogue, close the player's speech bubble
        //and allow the player to move again
        if (nPCIndex >= nPCDialogueSentences.Length - 1)
        {
            playerDialogueText.text = string.Empty;
            playerSpeechBubbleAnimator.SetTrigger("Close");
            yield return new WaitForSeconds(speechBubbleAnimationDelay);
            dialogueStarted = false;
            playerIndex = 0;
            nPCIndex = 0;

            playerMovement.ToggleConversation();

            if (triggerChapterEnd)
                uIManager.GetComponent<UIManager>().TriggerChapterEnd();
        }
        //If the NPC has more dialogue, continue NPC dialogue
        else
            StartCoroutine(ContinueNPCDialogue());
    }

    //Closes the NPC's speech bubble and opens the player's speech bubble
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

    //Closes the player's speech bubble and opens the NPC's speech bubble
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
}
