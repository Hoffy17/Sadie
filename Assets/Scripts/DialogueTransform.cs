using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTransform : MonoBehaviour
{
    public GameObject speaker;
    [SerializeField] private GameObject speechBubbleText;
    public float xDisplacement;
    public float yDisplacement;
    public float zDisplacement;

    void Update()
    {
        float x = speaker.transform.position.x;
        float y = speaker.transform.position.y + yDisplacement;
        float z = speaker.transform.position.z + zDisplacement;

        if (speaker.transform.rotation.y >= 0f && speaker.transform.rotation.y <= 180f)
        {
            x -= xDisplacement;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            speechBubbleText.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            x += xDisplacement;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            speechBubbleText.transform.localScale = new Vector3(1, 1, 1);
        }

        gameObject.transform.position = new Vector3(x, y, z);
        transform.Rotate(Vector3.zero);
    }
}
