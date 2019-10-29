using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public GameObject PlayerUiPrefab;
    public float health = 1f;
    public static GameObject localPlayerInstance;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            this.health = (float) stream.ReceiveNext();
        }
    }

    private void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.localPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameObject _uiGo = Instantiate(PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        
    }
    
    [PunRPC]
    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (health <= 0f)
        {
            GameManager.Instance.LeaveRoom();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }

    void CalledOnLevelWasLoaded(int level)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f,5f,0f);
        }

        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
}
