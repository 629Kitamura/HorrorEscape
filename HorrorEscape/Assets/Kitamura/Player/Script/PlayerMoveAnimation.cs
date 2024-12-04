using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField] string _walkingFlagName;
    [SerializeField] string _runnigFlagName;
    CharacterController _controller;
    Animator _animator;
    float _speed;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _animator.SetBool(_walkingFlagName, false);
        _animator.SetBool(_runnigFlagName, false);
    }

    void Update()
    {

        if (_controller.velocity.magnitude < 1)
        {
            _animator.SetBool(_walkingFlagName, false);
        }
        else
        {
            _animator.SetBool(_walkingFlagName, true);

        }
        if (Input.GetKey(KeyCode.LeftShift))//ここあんまよくない
        {
            _animator.SetBool(_runnigFlagName, true);
        }
        else
        {
            _animator.SetBool(_runnigFlagName, false);
        }
    }

}
