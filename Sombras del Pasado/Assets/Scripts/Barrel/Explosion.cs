using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem Particle;

    // Start is called before the first frame update
    void Start()
    {
        Particle = GetComponent<ParticleSystem>();
    }

    void Hit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Explode();
        }
    }

    void Explode()
    {
        Particle.Play();
        Destroy(gameObject, Particle.duration);
    }
}
