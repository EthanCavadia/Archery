using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public GameObject PlayerUiPrefab;
    [SerializeField] public Camera camera;
    
    public float health = 1f;
    public static GameObject localPlayerInstance;
    
    private CharacterController characterController;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float) stream.ReceiveNext();
            
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            localPlayerInstance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        camera = GetComponentInChildren<Camera>();

        if (!photonView.IsMine)
        {
            camera.enabled = false;
            return;
        }

        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [PunRPC]
    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        CalledOnLevelWasLoaded(scene.buildIndex);
    }

    void OnLevelWasLoaded(int level)
    {
        CalledOnLevelWasLoaded(level);
    }

    void CalledOnLevelWasLoaded(int level)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f,5f,0f);
        }

        GameObject _uiGo = Instantiate(PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
}
