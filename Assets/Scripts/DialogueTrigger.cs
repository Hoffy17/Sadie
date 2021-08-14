using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private bool triggeredConversation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggeredConversation)
        {
            dialogueManager.TriggerStartDialogue();
            triggeredConversation = true;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggered)
            triggered = false;
    }*/
}
