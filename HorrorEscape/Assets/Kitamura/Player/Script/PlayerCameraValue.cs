using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// playerのCameraの数値
/// </summary>
public class PlayerCameraValue : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    PlayerMove _playerMove;
    CinemachinePOV _POV;
    CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
    [Header("X軸感度(0.1~10倍)"), SerializeField, Range(0.1f, 10f)] float _aimSpeedX = 1f;
    [Header("Y軸感度(0.1~10倍)"), SerializeField, Range(0.1f, 10f)] float _aimSpeedY = 1f;
    [Header("視野角(90~120)"), SerializeField, Range(90, 120)] float _viewingAngle = 90;
    [Header("画面揺れ"), SerializeField] bool _screenShaking = true;
    [Header("ここをtrueにすると有効化　すぐfalseになる"), SerializeField] bool _apply = false;

    public float AimSpeedX { get => _aimSpeedX; set => _aimSpeedX = value; }
    public float AimSpeedY { get => _aimSpeedY; set => _aimSpeedY = value; }
    public float ViewingAngle { get => _viewingAngle; set => _viewingAngle = value; }
    public bool Apply { get => _apply; set => _apply = value; }

    void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _POV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _multiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        DataSet();
    }

    void Update()
    {
        if (Apply)
        {
            DataSet();
            Apply = false;
        }
    }

    void DataSet()
    {
        _POV.m_HorizontalAxis.m_MaxSpeed = AimSpeedX;
        _POV.m_VerticalAxis.m_MaxSpeed = AimSpeedY;
        _virtualCamera.m_Lens.FieldOfView = ViewingAngle;
        ScreenShaking();
    }
    void ScreenShaking()
    {
        if (_screenShaking)
        {
            _multiChannelPerlin.m_AmplitudeGain = 0.5f;
            _multiChannelPerlin.m_FrequencyGain = 0.5f;
        }
        else
        {
            _multiChannelPerlin.m_AmplitudeGain = 0;
            _multiChannelPerlin.m_FrequencyGain = 0;
        }
    }
}
