using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowController : MonoBehaviourPunCallbacks , IPunObservable
{
    [SerializeField] private PhotonView myPhotonView;
    private Rigidbody rigidbody;

    private float lifeTime = 2.0f;
    private float timer;
    private bool hit = false;
    private float arrowDamage = 0.5f;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            rigidbody.position = (Vector3) stream.ReceiveNext();
            rigidbody.velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));

            rigidbody.position += rigidbody.velocity * lag;
        }
    }
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        myPhotonView = GetComponent<PhotonView>();
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
    }
    
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
        if (!myPhotonView.IsMine)
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
            collision.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, arrowDamage);
            
            Debug.Log("Hit");
        }
    }

    private void Stick()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
