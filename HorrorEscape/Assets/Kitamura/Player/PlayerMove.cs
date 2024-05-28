using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーオブジェクトのpositionとrotationを動かすコンポーネント
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [Header("歩きスピード"),SerializeField] float _speed = 3f;
    [Header("走りスピード"),SerializeField] float _dashSpeed = 5f;
    [Header("走りスタミナ（まだない）")]
    CharacterController _controller;
    CinemachinePOV _cinemachinePOV;//VirtualCameraのAimがPOVのときの情報を持つ変数
    float _verticalSpeed;
    float _horizontalSpeed;
    bool IsGrounded() => _controller.isGrounded;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    void Update()
    {
        CameraForwardLook();
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
                _verticalSpeed = _dashSpeed * Input.GetAxis("Vertical");
                _horizontalSpeed = _dashSpeed * Input.GetAxis("Horizontal");
            }
            else
            {
                _verticalSpeed = _speed * Input.GetAxis("Vertical");
                _horizontalSpeed = _speed * Input.GetAxis("Horizontal");
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
    /// <summary>virtualcameraが見てる方向をforwardにする</summary>
    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }
}