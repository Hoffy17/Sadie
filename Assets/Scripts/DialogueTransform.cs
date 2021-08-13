using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTransform : MonoBehaviour
{
    public GameObject speaker;
    public float xDisplacement;
    public float yDisplacement;
    public float zDisplacement;

    void Update()
    {
        float x = speaker.transform.position.x;
        float y = speaker.transform.position.y + yDisplacement;
        float z = speaker.transform.position.z + zDisplacement;

        if (speaker.transform.rotation.y >= 0f && speaker.transform.rotation.y <= 180f)
            x -= xDisplacement;
        else
            x += xDisplacement;

        gameObject.transform.position = new Vector3(x, y, z);
        transform.Rotate(Vector3.zero);
    }
}
