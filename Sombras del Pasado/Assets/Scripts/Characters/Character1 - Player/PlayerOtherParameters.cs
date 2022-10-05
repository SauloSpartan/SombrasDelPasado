using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOtherParameters : MonoBehaviour
{
    //Audio variables
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;

    //Movement variables
    private float _walkSpeed = 3;
    private float _rotateSpeed = 1080;
    private Vector3 _moveRotation;
    [HideInInspector] public Vector3 MoveDirection;
    private Vector3 _moveVector;
    private CharacterController _controller;

    //Fight variables
    public float Health;
    public float Damage;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Gravity();
    }

    public void MovePlayer()
    {
        //Movement
        float _moveX = Input.GetAxis("Horizontal");
        float _moveZ = Input.GetAxis("Vertical");
        MoveDirection = new Vector3(_moveX, 0, _moveZ);
        MoveDirection *= _walkSpeed;
        _controller.Move(MoveDirection * Time.deltaTime);

        //Rotation
        _moveRotation = new Vector3(_moveX, 0, 0);
        _moveRotation.Normalize();
        transform.Translate(_moveRotation * _walkSpeed * Time.deltaTime, Space.World);
        if (_moveRotation != Vector3.zero) //If character is moving it rotates
        {
            Quaternion toRotation = Quaternion.LookRotation(_moveRotation, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotateSpeed * Time.deltaTime);
        }
    }

    private void Gravity()
    {
        _moveVector = Vector3.zero;

        if (_controller.isGrounded == false) //Check if character is grounded
        {
            _moveVector += Physics.gravity; //Add our gravity Vector
        }

        _controller.Move(_moveVector * Time.deltaTime);
    }

    #region Audio Events
    private void Step_Sound()
    {
        AudioClip clip = StepClip();
        _audioSource.PlayOneShot(clip);
    }

    private void Attack_Sound()
    {
        AudioClip clip = AttackClip();
        _audioSource.PlayOneShot(clip);
    }

    private void Death_Sound()
    {
        AudioClip clip = DeathClip();
        _audioSource.PlayOneShot(clip);
    }

    private AudioClip StepClip()
    {
        return _stepClips[UnityEngine.Random.Range(0, _stepClips.Length)];
    }

    private AudioClip AttackClip()
    {
        return _attackClips[UnityEngine.Random.Range(0, _attackClips.Length)];
    }

    private AudioClip DeathClip()
    {
        return _deathClips[UnityEngine.Random.Range(0, _deathClips.Length)];
    }
    #endregion
}
