using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTransform : MonoBehaviour
{
    //The character that the speech bubble belongs to
    public GameObject speaker;
    //The text box for that speech bubble
    [SerializeField] private GameObject speechBubbleText;
    //The XYZ displacement of the speech bubble, from the speaker
    public float xDisplacement;
    public float yDisplacement;
    public float zDisplacement;

    void Update()
    {
        //Get the XYZ transform of the speaker and add the YZ displacement of the speech bubble 
        float x = speaker.transform.position.x;
        float y = speaker.transform.position.y + yDisplacement;
        float z = speaker.transform.position.z + zDisplacement;

        //If the speaker is facing right, move the speech bubble to the left
        if (speaker.transform.rotation.y >= 0f && speaker.transform.rotation.y <= 180f)
        {
            x -= xDisplacement;
            //Flip the speech bubble sprite
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            //Because it is a child of the speech bubble, the text box's scale must be corrected
            speechBubbleText.transform.localScale = new Vector3(-1, 1, 1);
        }
        //Or if the speaker is facing left, move the speech bubble to the right
        else
        {
            x += xDisplacement;
            //Fix the speech bubble's scale, if it was previously flipped
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            //Fix the text box's scale, if it was previously flipped
            speechBubbleText.transform.localScale = new Vector3(1, 1, 1);
        }

        //Position the speech bubble and set its rotation to zero
        gameObject.transform.position = new Vector3(x, y, z);
        transform.Rotate(Vector3.zero);
    }
}
