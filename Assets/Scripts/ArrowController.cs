using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowController : MonoBehaviourPun
{
    private Rigidbody rigidbody;

    private float lifeTime = 2.0f;
    private float timer;
    private bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!hit)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!collision.collider.CompareTag("Arrow"))
        {
            hit = true;
            Stick();
        }

        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 0.5f);
            
            Debug.Log("Hit");
        }
    }

    private void Stick()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
