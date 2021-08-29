using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //The speed at which characters appear in text
    [SerializeField] private float typingSpeed;
    //Delay between triggering dialogue and starting dialogue
    [SerializeField] private float dialogueDelay;

    //Controls which character speaks first
    [SerializeField] private bool playerSpeakingFirst;
    //Control UI events that appear when dialogue is over
    [SerializeField] private bool triggerGetCoffee;
    [SerializeField] private bool triggerGetCatFood;
    [SerializeField] private bool triggerChapterEnd;

    //Text Mesh Pro components for the dialogue
    [Header("Dialogue - Text Mesh Pro")]
    [SerializeField] private TextMeshProUGUI playerDialogueText;
    [SerializeField] private TextMeshProUGUI nPCDialogueText;

    //Animators for the speech bubbles
    [Header("Dialogue - Animation Controllers")]
    [SerializeField] private Animator playerSpeechBubbleAnimator;
    [SerializeField] private Animator nPCSpeechBubbleAnimator;

    [Header("Dialogue - UI Audio Source")]
    [SerializeField] private AudioSource sfxSpeechBubbleOpen;
    [SerializeField] private AudioSource sfxSpeechBubbleClose;

    //The dialogue itself is entered in these fields
    [Header("Dialogue - Sentences")]
    [TextArea]
    [SerializeField] private string[] playerDialogueSentences;
    [TextArea]
    [SerializeField] private string[] nPCDialogueSentences;

    //Other scripts to be called
    private GameManager gameManager;
    private UIManager uIManager;
    private Movement playerMovement;

    //Booleans that follow when dialogue begins and ends
    private bool dialogueStarted;
    private bool playerDialogueEnded;
    private bool nPCDialogueEnded;

    //Integers that count up with each line of dialogue
    private int playerIndex;
    private int nPCIndex;

    //Delay between when speech bubble animates and text appears
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
        //When the player finishes talking, the player can advance NPC dialogue
        if (playerDialogueEnded == true)
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && gameManager.gameState == GameState.game)
                StartCoroutine(CheckContinueNPCDialogue());

        //When the NPC finishes talking, the player can advance player dialogue
        if (nPCDialogueEnded == true)
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && gameManager.gameState == GameState.game)
                StartCoroutine(CheckContinuePlayerDialogue());
    }

    //Function called when dialogue begins
    public IEnumerator StartDialogue()
    {
        //Prevents the player from moving
        playerMovement.ToggleConversation();
        yield return new WaitForSeconds(dialogueDelay);

        //Animates the speech bubbles
        if (playerSpeakingFirst)
        {
            playerSpeechBubbleAnimator.SetTrigger("Open");
            sfxSpeechBubbleOpen.Play();
            yield return new WaitForSeconds(speechBubbleAnimationDelay);

            StartCoroutine(TypePlayerDialogue());
        }
        else
        {
            nPCSpeechBubbleAnimator.SetTrigger("Open");
            sfxSpeechBubbleOpen.Play();
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
        nPCDialogueEnded = false;

        //If the player has no more dialogue, close the NPC's speech bubble
        //and allow the player to move again
        if (playerIndex >= playerDialogueSentences.Length - 1)
        {
            nPCDialogueText.text = string.Empty;
            nPCSpeechBubbleAnimator.SetTrigger("Close");
            sfxSpeechBubbleClose.Play();
            yield return new WaitForSeconds(speechBubbleAnimationDelay);
            dialogueStarted = false;
            playerIndex = 0;
            nPCIndex = 0;

            playerMovement.ToggleConversation();

            if (triggerGetCoffee)
                uIManager.GetComponent<UIManager>().TriggerGetCoffee();
            else if (triggerGetCatFood)
                uIManager.GetComponent<UIManager>().TriggerGetCatFood();
            else if (triggerChapterEnd)
                uIManager.GetComponent<UIManager>().TriggerChapterEnd();
        }
        //If the player has more dialogue, continue player dialogue
        else
            StartCoroutine(ContinuePlayerDialogue());
    }

    //When the player's dialogue ends, check if the NPC has any more dialogue
    private IEnumerator CheckContinueNPCDialogue()
    {
        playerDialogueEnded = false;

        //If the NPC has no more dialogue, close the player's speech bubble
        //and allow the player to move again
        if (nPCIndex >= nPCDialogueSentences.Length - 1)
        {
            playerDialogueText.text = string.Empty;
            playerSpeechBubbleAnimator.SetTrigger("Close");
            sfxSpeechBubbleClose.Play();
            yield return new WaitForSeconds(speechBubbleAnimationDelay);
            dialogueStarted = false;
            playerIndex = 0;
            nPCIndex = 0;

            playerMovement.ToggleConversation();

            if (triggerGetCoffee)
                uIManager.GetComponent<UIManager>().TriggerGetCoffee();
            else if (triggerGetCatFood)
                uIManager.GetComponent<UIManager>().TriggerGetCatFood();
            else if (triggerChapterEnd)
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
        sfxSpeechBubbleClose.Play();
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        playerDialogueText.text = string.Empty;
        playerSpeechBubbleAnimator.SetTrigger("Open");
        sfxSpeechBubbleOpen.Play();
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
        sfxSpeechBubbleClose.Play();
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        nPCDialogueText.text = string.Empty;
        nPCSpeechBubbleAnimator.SetTrigger("Open");
        sfxSpeechBubbleOpen.Play();
        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        if (dialogueStarted)
            nPCIndex++;
        else
            dialogueStarted = true;

        StartCoroutine(TypeNPCDialogue());
    }
}
