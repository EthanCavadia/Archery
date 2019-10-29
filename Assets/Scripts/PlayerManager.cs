using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public GameObject PlayerUiPrefab;
    [SerializeField] public PhotonView myPhotonView;
    [SerializeField] public  Camera camera;
    
    public float health = 1f;
    public static GameObject localPlayerInstance;
        
    private Vector3 originalCameraPosition;
    private CharacterController characterController;
    
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
        if (myPhotonView.IsMine)
        {
            PlayerManager.localPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {       
        myPhotonView = GetComponent<PhotonView>();
        camera = GetComponentInChildren<Camera>();
        
        if (!myPhotonView.IsMine)
        {
            camera.enabled = false;
            return;
        }

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
        if (myPhotonView.IsMine)
        {
            if (health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
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
