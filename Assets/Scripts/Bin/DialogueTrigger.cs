using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Animator nPCSpeechBubbleAnimator;
    [SerializeField] private DialogueManager dialogueManager;

    private bool triggeredConversation;
    private float speechBubbleAnimationDelay = 0.6f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggeredConversation)
            nPCSpeechBubbleAnimator.SetTrigger("Open");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !triggeredConversation)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(CloseSpeechPrompt());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !triggeredConversation)
            nPCSpeechBubbleAnimator.SetTrigger("Close");
    }

    private IEnumerator CloseSpeechPrompt()
    {
        nPCSpeechBubbleAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(speechBubbleAnimationDelay);
        dialogueManager.TriggerStartDialogue();
        triggeredConversation = true;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggered)
            triggered = false;
    }*/
}
