using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SetCameraPosition : MonoBehaviourPun
{
    [SerializeField] public PhotonView myPhotonView;

    [SerializeField] public  Camera camera;
    private Vector3 originalCameraPosition;
    private CharacterController characterController;
    
    void Start()
    {
        //camera = Camera.main;
        myPhotonView = GetComponent<PhotonView>();
        if (!myPhotonView.IsMine)
        {
            Destroy(camera);
        }
    }

    void Update()
    {
        if (myPhotonView.IsMine)
        {
            camera.transform.position = transform.position;
            camera.transform.rotation = transform.rotation;
        }
    }
}
