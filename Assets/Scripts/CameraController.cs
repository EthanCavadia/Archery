using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraController : MonoBehaviourPun
{
    public enum RotationAxis
    {
        MouseX = 1,
        MouseY = 2
    }
    
    public RotationAxis axes = RotationAxis.MouseX;

    private float minimumVert = -45.0f;
    private float maximumVert = 45.0f;

    private float sensVertical = 10.0f;
    private float sensHorizontal = 10.0f;

    private float rotationX = 0;


    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (axes == RotationAxis.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensHorizontal, 0);
        }
        else if (axes == RotationAxis.MouseY)
        {
            rotationX -= Input.GetAxis("Mouse Y") * sensVertical;
            rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
}

