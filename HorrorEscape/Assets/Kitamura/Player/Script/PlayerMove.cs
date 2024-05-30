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
    const float _gravityValue = -9.81f;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [Header("歩きスピード"), SerializeField] float _walkSpeed = 3f;
    [Header("走りスピード"), SerializeField] float _runSpeed = 5f;
    [Header("走りスタミナ(second)"), SerializeField] float _Stamina;
    [Header("ジャンプパワー"), SerializeField] float _jampPower;
    [Header("以下確認用")]
    public bool _isGrounded;
    CharacterController _controller;
    CinemachinePOV _cinemachinePOV;//VirtualCameraのAimがPOVのときの情報を持つ変数
    Vector3 _playerVelocity;
    float _speedMag;
    /// <summary>CharacterControllerのスピード</summary>
    public float SpeedMag { get => _speedMag; }

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
        Move();//gameobject.forwardを元にしたwasdでの移動
        CameraForwardLook();
        _speedMag = _controller.velocity.magnitude;//Charactorの速度
        _isGrounded = IsGrounded();
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
                _playerVelocity = right * Input.GetAxis("Horizontal") * _runSpeed + forward * Input.GetAxis("Vertical") * _runSpeed;
            }
            else
            {
                _playerVelocity = right * Input.GetAxis("Horizontal") * _walkSpeed + forward * Input.GetAxis("Vertical") * _walkSpeed;
            }
            if (_playerVelocity.y < 0) _playerVelocity.y = 0;//接地していたら重力加算を0にする
            if (Input.GetKeyDown(KeyCode.Space)) Jamp();
        }
        _playerVelocity.y += _gravityValue * Time.deltaTime;//重力加算
        _controller.Move(_playerVelocity * Time.deltaTime);// Move関数で移動させる
    }

    void Jamp()
    {
        _playerVelocity.y += Mathf.Sqrt(_jampPower * -3.0f * _gravityValue); //jamp
    }
    /// <summary>virtualcameraが見てる方向をforwardにする</summary>
    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }//maincameraのrotationを持ってきてるのと同じ
}