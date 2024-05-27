using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーを動かすコンポーネント
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float dashSpeed = 5f;

    CharacterController _controller;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    CinemachineOrbitalTransposer _cinemachineOrbitalTransposer;//bodyの情報を持つ変数
    CinemachinePOV _cinemachinePOV;//aimがPOVのときの情報を持つ変数
    float _verticalSpeed;
    float _horizontalSpeed;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cinemachineOrbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        CameraForwardLook();//virtualcameraが見てる方向をforwardにする
        Move();//gameobject.forwardを元にしたwasdでの移動
    }

    void Move()
    {
        //キャラクターのローカル空間での方向
        Vector3 forward = transform.transform.forward;
        Vector3 right = transform.transform.right;
        if (IsGrounded())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _verticalSpeed = dashSpeed * Input.GetAxis("Vertical");
                _horizontalSpeed = dashSpeed * Input.GetAxis("Horizontal");
            }
            else
            {
                _verticalSpeed = speed * Input.GetAxis("Vertical");
                _horizontalSpeed = speed * Input.GetAxis("Horizontal");
            }
        }
        else
        {
            _verticalSpeed = 0f;
            _horizontalSpeed = 0f;
        }
        // SimpleMove関数で移動させる
        _controller.SimpleMove(forward * _verticalSpeed + right * _horizontalSpeed);
    }

    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }

    bool IsGrounded() => _controller.isGrounded;
}