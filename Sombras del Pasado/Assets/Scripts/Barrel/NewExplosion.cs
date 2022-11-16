using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewExplosion : MonoBehaviour
{
    // Explosion variables
    private CapsuleCollider _capsuleCollider;
    private ParticleSystem _particle;
    private Animator _animator;
    private AudioSource _audio;

    // Barrel variables
    private MeshRenderer _render;
    private MeshCollider _meshCollider;

    // Player variables
    private CameraControl _camera;
    public float Damage;
    private float _baseDamage;

    void Awake()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        _animator = GetComponentInChildren<Animator>();
        _audio = GetComponentInChildren<AudioSource>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _render = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();

        _camera = FindObjectOfType<CameraControl>();

        _animator.enabled = false;
        _audio.enabled = false;
        _baseDamage = 15;
        Difficulty();
    }

    private void Difficulty()
    {
        if (MainMenu.Difficulty == 1) // Easy
        {
            Damage = _baseDamage / 2;
        }
        else if (MainMenu.Difficulty == 2) // Normal
        {
            Damage = _baseDamage;
        }
        else if (MainMenu.Difficulty == 3) // Hard
        {
            Damage = _baseDamage * 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Sword" || other.tag == "Enemy1 Sword" || other.tag == "Enemy2 Sword" || other.tag == "Enemy3 Dagger" || other.tag == "Enemy4 Sword")
        {
            StartCoroutine(_camera.CameraShake(0.5f, 10f));
            _capsuleCollider.enabled = false;
            _animator.enabled = true;
            _audio.enabled = true;
            _render.enabled = false;
            _meshCollider.enabled = false;
            _particle.Play();
            Destroy(gameObject, 1.5f);
        }
    }
}
