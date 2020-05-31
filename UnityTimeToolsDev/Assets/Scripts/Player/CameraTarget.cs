using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public PlayerController player;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!player.died)
        {
            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;
        }
    }
}
