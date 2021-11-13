using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem Particle;
    public GameObject DMG;
    public GameObject Barrel; 

    // Start is called before the first frame update
    void Start()
    {
        Particle = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DMG.SetActive(true);
            Particle.Play();
            Destroy(Barrel, 2f);
            Destroy(DMG, .1f);
        }
    }

}
