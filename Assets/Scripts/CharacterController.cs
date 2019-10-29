using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviourPun
{
    public float speed;

    private void Awake()
    {
        if (!photonView.IsMine && GetComponent<CharacterController>() != null)
        {
            Destroy(GetComponent<CharacterController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;
        
        Vector3 movement = new Vector3(h,0,v);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        

    }
}
