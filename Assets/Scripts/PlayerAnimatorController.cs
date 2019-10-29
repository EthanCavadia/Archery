using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviourPun
{
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.Log("PlayerAnimator is missing Animator Component", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator)
        {
            return;
        }
        
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v < 0)
        {
            v = 0;
        }
        
        animator.SetFloat("Speed", h * h + v * v);
    }
}
