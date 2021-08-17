using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
    public float cameraHeight;
    public float zDisplacement;

    void Update()
    {
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float y = cameraHeight;
        float z = player.transform.position.z + zDisplacement;
        z = Mathf.Clamp(z, zMin, zMax);
        gameObject.transform.position = new Vector3(x, y, z);
    }
}
