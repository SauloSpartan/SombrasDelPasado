using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem _particle;
    [SerializeField] private GameObject OBJ_Barrel;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audio;

    // Variable del collider que origina la explosion
    private CapsuleCollider _capsuleCollider;

    // Variables para el collider y renderer del barril
    private MeshRenderer _render;
    private MeshCollider _meshCollider;

    private CameraControl _camera;

    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _render = GetComponentInParent<MeshRenderer>();
        _meshCollider = GetComponentInParent<MeshCollider>();

        _camera = FindObjectOfType<CameraControl>();

        _animator.enabled = false;
        _audio.enabled = false;
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
            Destroy(OBJ_Barrel, 1.5f);
        }
    }
}
