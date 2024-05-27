using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �v���C���[�𓮂����R���|�[�l���g
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    CharacterController _controller;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    CinemachineOrbitalTransposer _cinemachineOrbitalTransposer;//body�̏������ϐ�
    CinemachinePOV _cinemachinePOV;//aim��POV�̂Ƃ��̏������ϐ�

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cinemachineOrbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        CameraForwardLook();//virtualcamera�����Ă������forward�ɂ���
        Move();//gameobject.forward�����ɂ���wasd�ł̈ړ�
    }

    void Move()
    {
        // �L�����N�^�[�̃��[�J����Ԃł̕���
        Vector3 forward = transform.transform.forward;
        Vector3 right = transform.transform.right;

        float verticalSpeed = speed * Input.GetAxis("Vertical");
        float horizontalSpeed = speed * Input.GetAxis("Horizontal");

        // SimpleMove�֐��ňړ�������
        _controller.SimpleMove(forward * verticalSpeed + right * horizontalSpeed);
    }

    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }
}