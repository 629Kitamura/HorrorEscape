using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum GroundState
{
    Road,
    Soil,
    Grass
}
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioClip _walkingSoundRoad;
    [SerializeField] AudioClip _walkingSoundSoil;
    [SerializeField] AudioClip _walkingSoundGrass;
    [SerializeField] AudioClip _runningSoundRoad;
    [SerializeField] AudioClip _runningSoundSoil;
    [SerializeField] AudioClip _runningSoundGrass;
    [SerializeField] AudioClip _landingSound;
    [SerializeField] string _roadTagName = "Road";
    [SerializeField] string _soilTagName = "Soil";
    [SerializeField] string _grassTagName = "Grass";
    PlayerMove _playerMove;
    AudioSource _audioSouce;
    bool _isPlaying = false;
    bool _islanding = false;
    AudioClip _previousSound;
    float _delayTime = 0.3f;
    float _time;
    [Header("地面のレイヤーを入力"), SerializeField] int _layer = 9;
    int _layerMask ;
    GroundState _groundState;
    void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _audioSouce = GetComponent<AudioSource>();
        _audioSouce.Stop();
        _layerMask = 1 << _layer;
    }

    void Update()
    {
        ChangeFootsteps();
        AudioSoicePlayAndStop();
        LandingSound();
    }

    void FixedUpdate()
    {
        DownRaycast();
    }
    /// <summary>足元のオブジェクトのタグに応じて足音を変える</summary>
    void ChangeFootsteps()
    {
        AudioClip runningSound;
        AudioClip walkingSound;
        if (_groundState == GroundState.Road)
        {
            runningSound = _runningSoundRoad;
            walkingSound = _walkingSoundRoad;
        }
        else if (_groundState == GroundState.Soil)
        {
            runningSound = _runningSoundSoil;
            walkingSound = _walkingSoundSoil;
        }
        else if (_groundState == GroundState.Grass)
        {
            runningSound = _runningSoundGrass;
            walkingSound = _walkingSoundGrass;
        }
        else
        {
            runningSound = _runningSoundRoad;
            walkingSound = _walkingSoundRoad;
        }
        if (_playerMove.IsRunnig)
            _audioSouce.clip = runningSound;
        else
            _audioSouce.clip = walkingSound;
    }
    void AudioSoicePlayAndStop()
    {
        if (_playerMove.IsMoved && !_isPlaying && !_playerMove.IsRunnig)
        {
            _audioSouce.PlayDelayed(_delayTime);
            _isPlaying = true;
        }//足を踏み出すのに合わせて音を遅らせる
        else if (_playerMove.IsMoved && !_isPlaying && _playerMove.IsRunnig)
        {
            _audioSouce.PlayDelayed(_delayTime);
            _isPlaying = true;
        }
        else if (!_playerMove.IsMoved && _isPlaying || !_playerMove.IsGrounded)
        {
            _audioSouce.Stop();
            _isPlaying = false;
        }
        if (_playerMove.IsMoved && _audioSouce.clip != _previousSound)
        {
            _audioSouce.Play();
            _previousSound = _audioSouce.clip;
        }//歩きと走りを切り替えた時にPlayし直す
    }
    void LandingSound()
    {
        if (_playerMove.IsGrounded == false)
        {
            _time += Time.deltaTime;
            _islanding = false;
        }
        else if (_playerMove.IsGrounded == true && _islanding == false)
        {
            if (_time > 0.5f)
                _audioSouce.PlayOneShot(_landingSound);
            _time = 0;
            _islanding = true;
        }
    }

    void DownRaycast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, _layerMask))
        {
            if (hit.transform.CompareTag(_roadTagName))
            {
                _groundState = GroundState.Road;
            }
            else if (hit.transform.CompareTag(_soilTagName))
            {
                _groundState = GroundState.Soil;
            }
            else if (hit.transform.CompareTag(_grassTagName))
            {
                _groundState = GroundState.Grass;
            }
            else
            {
                _groundState = GroundState.Road;
            }
        }
    }
}
