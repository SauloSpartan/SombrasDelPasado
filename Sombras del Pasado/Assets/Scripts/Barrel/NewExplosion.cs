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
    [SerializeField] Material[] _mainMaterials = null;

    // Player variables
    private CameraControl _camera;
    public float Damage;
    private float _baseDamage;

    public Material[] MainMaterials { get { return _mainMaterials; } set { _mainMaterials = value; } }

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
        NoEmitionMaterial();
    }

    private void Start()
    {
        NoEmitionMaterial();
    }

    /// <summary>
    /// Function that changes barrel damage depending on difficulty.
    /// </summary>
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

    /// <summary>
    /// Function disable the emition on the materials
    /// </summary>
    private void NoEmitionMaterial()
    {
        foreach (Material materials in _mainMaterials)
        {
            materials.DisableKeyword("_EMISSION");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Sword" || other.tag == "Enemy1 Sword" || other.tag == "Enemy2 Sword" || other.tag == "Enemy3 Dagger" || other.tag == "Enemy4 Sword")
        {
            StartCoroutine(BarrelWink());
            _capsuleCollider.enabled = false;
            _meshCollider.enabled = false;
        }
    }

    private IEnumerator BarrelWink()
    {
        float timer = 0.55f;
        for (int i = 2; i < 15; i++)
        {
            foreach (Material Materials in _mainMaterials)
            {
                Materials.EnableKeyword("_EMISSION");
            }
            yield return new WaitForSecondsRealtime(timer);

            foreach (Material Materials in _mainMaterials)
            {
                Materials.DisableKeyword("_EMISSION");
            }
            yield return new WaitForSecondsRealtime(timer);

            timer -= timer/i;
        }
        StartCoroutine(_camera.CameraShake(0.5f, 10f));
        _animator.enabled = true;
        _render.enabled = false;
        _audio.enabled = true;
        _particle.Play();
        Destroy(gameObject, 1.5f);
    }
}
