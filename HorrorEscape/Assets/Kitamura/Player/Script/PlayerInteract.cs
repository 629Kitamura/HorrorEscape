using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] float _distance = 5f;
    CinemachinePOV _cinemachinePOV;
    void Start()
    {
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void PlayerRaycast()
    {
        Vector3 forward = new Vector3(_cinemachinePOV.m_HorizontalAxis.Value, _cinemachinePOV.m_VerticalAxis.Value, 0);
        Ray ray = new Ray(transform.position, forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, _distance))
        {
            
        }
    }
}
