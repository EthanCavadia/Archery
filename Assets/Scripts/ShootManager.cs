using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShootManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform arrowSpawn;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shootForce = 20.0f;

    private bool shotArrow;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(shotArrow);
        }
        else
        {
            this.shotArrow = (bool) stream.ReceiveNext();
        }
    }

    private void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!shotArrow)
            {
                shotArrow = true;

                GameObject go = PhotonNetwork.Instantiate(this.arrowPrefab.name, arrowSpawn.position, Quaternion.identity);
                Rigidbody rb = go.GetComponent<Rigidbody>();

                rb.velocity = camera.transform.forward * shootForce;
            }
            else
            {
                shotArrow = false;
            }
        }
    }
}