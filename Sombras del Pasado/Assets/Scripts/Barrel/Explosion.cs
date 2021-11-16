using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem Particle;
    public GameObject OBJ_Barrel;
    public Animator Explode;
    //Variables para el collider y renderer del barril
    public MeshRenderer Visual;
    public MeshCollider Collider;

    // Start is called before the first frame update
    void Start()
    {
        Particle = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Explode.enabled = true;
            Visual.enabled = false;
            Collider.enabled = false;
            Particle.Play();
            Destroy(OBJ_Barrel, 1.5f);
        }
    }

}
