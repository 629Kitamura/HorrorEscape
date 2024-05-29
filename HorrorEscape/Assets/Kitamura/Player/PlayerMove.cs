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
    [Header("歩きスピード"),SerializeField] float _walkSpeed = 3f;
    [Header("走りスピード"),SerializeField] float _runSpeed = 5f;
    [Header("走りスタミナ(second)"), SerializeField] float _Stamina;
    [Header("ジャンプパワー"), SerializeField] float _jampPower;
    CharacterController _controller;
    CinemachinePOV _cinemachinePOV;//VirtualCameraのAimがPOVのときの情報を持つ変数
    float _verticalSpeed;
    float _horizontalSpeed;
    float _speedMag;
    /// <summary>CharacterControllerのスピード</summary>
    public float SpeedMag { get => _speedMag;}

    bool IsGrounded() => _controller.isGrounded;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _cinemachinePOV.m_HorizontalAxis.Value = 0f;
        _cinemachinePOV.m_VerticalAxis.Value = 0f;

    }
    void Update()
    {
        CameraForwardLook();
        Move();//gameobject.forwardを元にしたwasdでの移動
        _speedMag = _controller.velocity.magnitude;//Charactorの速度
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
                _verticalSpeed = _runSpeed * Input.GetAxis("Vertical");
                _horizontalSpeed = _runSpeed * Input.GetAxis("Horizontal");
            }
            else
            {
                _verticalSpeed = _walkSpeed * Input.GetAxis("Vertical");
                _horizontalSpeed = _walkSpeed * Input.GetAxis("Horizontal");
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
    void Jamp()
    {

    }
    /// <summary>virtualcameraが見てる方向をforwardにする</summary>
    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }
}