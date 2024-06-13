using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] float _distance = 5f;
    [SerializeField] string _interactTagName = "Item";
    [SerializeField] GameObject _intaractUI;
    [SerializeField] bool _isInteractable = false;
    CinemachinePOV _POV;
    RaycastHit _hit;
    public bool IsInteractable { get => _isInteractable; }
    public RaycastHit Hit { get => _hit; }

    void Start()
    {
        _POV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _intaractUI.SetActive(false);
    }

    void FixedUpdate()
    {
        PlayerRaycast();
    }

    void PlayerRaycast()
    {
        Ray ray = new Ray(_virtualCamera.transform.position, Camera.main.transform.forward);//メインカメラ引っ張ってきてるのだめ
        if (Physics.Raycast(ray, out _hit, _distance))
        {
            if (_hit.transform.CompareTag(_interactTagName))
            {
                _intaractUI.SetActive(true);
                _isInteractable = true;
            }
            else
            {
                _intaractUI.SetActive(false);
                _isInteractable = false;
            }
        }
        else
        {
            _intaractUI.SetActive(false);
            _isInteractable = false;
        }
        Debug.DrawRay(_virtualCamera.transform.position, Camera.main.transform.forward * _distance, Color.green);
    }
}
