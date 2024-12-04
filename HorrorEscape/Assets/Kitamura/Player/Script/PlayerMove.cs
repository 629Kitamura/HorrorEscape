using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static System.Runtime.CompilerServices.RuntimeHelpers;
/// <summary>
/// プレイヤーオブジェクトのpositionとrotationを動かすコンポーネント
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    const float _gravityValue = -9.81f;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] GameObject _playerHead;
    [Header("歩きスピード"), SerializeField] float _walkSpeed = 3f;
    [Header("走りスピード"), SerializeField] float _runSpeed = 5f;
    [Header("走りスタミナ(second)"), SerializeField] float _Stamina;
    [Header("ジャンプパワー"), SerializeField] float _jampPower;
    [Header("以下確認用(基本的には変更しない)")]
    [Tooltip("接地しているか"), SerializeField] bool _isGrounded;
    [Tooltip("移動しているか"), SerializeField] bool _isMoved;
    [Tooltip("走れるか"), SerializeField] bool _isRunning;
    [Tooltip("移動可能か"), SerializeField] bool _isMove = true;
    [Tooltip("視点移動可能か"), SerializeField] bool _isViewPointMove = true;
    CharacterController _controller;
    CinemachinePOV _cinemachinePOV;//VirtualCameraのAimがPOVのときの情報を持つ変数
    Vector3 _playerVelocity;
    float _speedMag;
    /// <summary>CharacterControllerのスピード<br/>
    /// アニメーション遷移で使う</summary>
    public float SpeedMag { get => _speedMag; }
    /// <summary>移動してほしくないときにfalseにする</summary>
    public bool IsMove { get => _isMove; set => _isMove = value; }
    /// <returns>接地しているか？</returns>
    public bool IsGrounded => _controller.isGrounded;//不安定要改善
    /// <returns>移動キー(WASD)を入力しているか？</returns>
    public bool IsMoved => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    /// <summary>LeftSiftキーを押しているか</summary>
    public bool IsRunnig => Input.GetKey(KeyCode.LeftShift);
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    void Update()
    {
        CameraForwardLook();
        InsertValue();
    }
    void FixedUpdate()
    {
        if (IsMove) Move();
    }
    /// <summary> 確認用やその他の変数への代入</summary>
    void InsertValue()
    {
        _isGrounded = IsGrounded;
        _isMoved = IsMoved;
        _speedMag = _controller.velocity.magnitude;//Charactorの速度
        _isRunning = IsRunnig;
    }
    /// <summary>gameobject.forwardを元にしたwasdでの移動</summary>
    void Move()
    {
        //キャラクターのローカル空間での方向
        Vector3 forward = transform.transform.forward;
        Vector3 right = transform.transform.right;
        if (IsGrounded)
        {
            _playerVelocity = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            RunOrWalk(KeyCode.LeftShift);
            Jamping(KeyCode.Space);
            if (_playerVelocity.y < 0) _playerVelocity.y = 0;//接地していたら重力加算を0にする
        }//接地していなかったら移動、ジャンプできない
        _playerVelocity.y += _gravityValue * Time.deltaTime;//重力加算
        _controller.Move(_playerVelocity * Time.deltaTime);// Move関数で移動させる
    }
    /// <summary>_jampPowerと_gravityValueを元にしてジャンプする</summary>
    void Jamping(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode)) _playerVelocity.y += Mathf.Sqrt(_jampPower * -3.0f * _gravityValue); //jamp
    }
    /// <summary> シフトキー押していたら_runSpeed、押していなかったら_walkSpeedを_playerVelocityに乗算</summary>
    void RunOrWalk(KeyCode keyCode)
    {
        if (Input.GetKey(keyCode)) _playerVelocity *= _runSpeed; else _playerVelocity *= _walkSpeed;
    }
    /// <summary>virtualcameraが見てる方向のy軸をGameObjectのforwardにする<br/>頭のオブジェクトはx軸もy軸も合わせる</summary>
    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
        _playerHead.transform.rotation = Quaternion.Euler(_cinemachinePOV.m_VerticalAxis.Value, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }
}