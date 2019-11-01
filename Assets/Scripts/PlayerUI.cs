using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPun
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Vector3 screenOffset = new Vector3(0f,30f,0f);
    [SerializeField] private Camera camera;
    
    private PlayerManager target;
    private float characterControllerHeight;
    private Transform targetTransform;
    private Renderer targetRender;
    private CanvasGroup canvasGroup;
    private Vector3 targetPosition;
    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            return;
        }
        
        target = _target;
        
        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }
        targetTransform = target.GetComponent<Transform>();
        targetRender = target.GetComponent<Renderer>();

        UnityEngine.CharacterController characterController = _target.GetComponent<UnityEngine.CharacterController>();

        if (characterController != null)
        {
            characterControllerHeight = characterController.height;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.health;
        }
    }

    private void LateUpdate()
    {
        
        if (targetTransform != null)
        {
            canvasGroup.alpha = targetRender.isVisible ? 1f : 0f;
        }

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            transform.position = camera.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
}
