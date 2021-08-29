using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Player movement
    private CharacterController characterController;
    public float speed;
    //Controls whether the player can move
    private bool inConversation;

    //Controls player character's animations
    [SerializeField] private Animator lexiAnimator;

    //Controls game states
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        //If the player is not in conversation with an NPC, allow the player to move
        if (!inConversation && gameManager.gameState == GameState.game)
        {
            //Get movement input from X and Z axes and calculate movement velocity with the character controller
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.Move(move * Time.deltaTime * speed);

            //If the player is not moving, play idle animation
            if (move == Vector3.zero)
            {
                lexiAnimator.SetTrigger("Idle");
            }
            //Otherwise if the player is moving, play running animation and move the player game object
            else if (move != Vector3.zero)
            {
                lexiAnimator.SetTrigger("Running");
                gameObject.transform.forward = move;
            }
        }
        //If the player is in conversation with an NPC, play idle animation
        else
        {
            lexiAnimator.SetTrigger("Idle");
        }
    }

    //When this function is called, toggle the state of being in conversation with an NPC
    public void ToggleConversation()
    {
        inConversation = !inConversation;
    }
}