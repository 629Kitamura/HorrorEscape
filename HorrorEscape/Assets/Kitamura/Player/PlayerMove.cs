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

    CharacterController _controller;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    CinemachineOrbitalTransposer _cinemachineOrbitalTransposer;//bodyの情報を持つ変数
    CinemachinePOV _cinemachinePOV;//aimがPOVのときの情報を持つ変数

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
        // キャラクターのローカル空間での方向
        Vector3 forward = transform.transform.forward;
        Vector3 right = transform.transform.right;

        float verticalSpeed = speed * Input.GetAxis("Vertical");
        float horizontalSpeed = speed * Input.GetAxis("Horizontal");

        // SimpleMove関数で移動させる
        _controller.SimpleMove(forward * verticalSpeed + right * horizontalSpeed);
    }

    void CameraForwardLook()
    {
        transform.rotation = Quaternion.Euler(0, _cinemachinePOV.m_HorizontalAxis.Value, 0);
    }
}