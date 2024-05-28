using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// player��Camera�̐��l
/// </summary>
public class PlayerCameraValue : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    CinemachinePOV _cinemachinePOV;
    [Header("X�����x(0.1~10�{)"),SerializeField,Range(0.1f,10f)] float _aimSpeedX = 1f;//���͒l�̏搔
    [Header("Y�����x(0.1~10�{)"), SerializeField,Range(0.1f,10f)] float _aimSpeedY = 1f;//���͒l�̏搔
    [Header("����p(90~120)"), SerializeField, Range(90, 120)] float _viewingAngle = 90;
    [Header("������true�ɂ���ƗL�����@����false�ɂȂ��"),SerializeField]bool _apply = false;

    public float AimSpeedX { get => _aimSpeedX; set => _aimSpeedX = value; }
    public float AimSpeedY { get => _aimSpeedY; set => _aimSpeedY = value; }
    public float ViewingAngle { get => _viewingAngle; set => _viewingAngle = value; }
    public bool Apply { get => _apply; set => _apply = value; }

    void Start()
    {
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
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
        _cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = AimSpeedX;
        _cinemachinePOV.m_VerticalAxis.m_MaxSpeed = AimSpeedY;
        _virtualCamera.m_Lens.FieldOfView = ViewingAngle;
    }




}
