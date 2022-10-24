using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem Particle;
    public GameObject OBJ_Barrel;
    public Animator Explode;
    public AudioSource Sound;

    // Variable del collider que origina la explosion
    public CapsuleCollider Trigger;

    // Variables para el collider y renderer del barril
    public MeshRenderer Visual;
    public MeshCollider Collider;

    CameraControl cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        Particle = GetComponent<ParticleSystem>();

        cameraShake = FindObjectOfType<CameraControl>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Sword" || other.gameObject.tag == "Enemy1 Sword"
            || other.gameObject.tag == "Enemy2 Sword" || other.gameObject.tag == "Enemy3 Dagger"
            || other.gameObject.tag == "Enemy4 Sword")
        {
            StartCoroutine(cameraShake.CameraShake(0.5f, 1f));
            Trigger.enabled = false;
            Explode.enabled = true;
            Sound.enabled = true;
            Visual.enabled = false;
            Collider.enabled = false;
            Particle.Play();
            Destroy(OBJ_Barrel, 1.5f);
        }
    }
}
