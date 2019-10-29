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
    private float characterControllerHeight = 0f;
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
            playerNameText.text = target.myPhotonView.Owner.NickName;
        }
    }

    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void Update()
    {

        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.health;
        }

        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            this.canvasGroup.alpha = targetRender.isVisible ? 1f : 0f;
        }

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = camera.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
}
