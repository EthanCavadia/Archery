using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SetCameraPosition : MonoBehaviourPun
{

    private float sensVertical;
    private float maxRotation = 45.0f;
    private float minRotation = -45.0f;
    private float rotation;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        { 
            return;
        }
        Camera.main.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = transform.rotation;
        }
    }
}
