using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Sound : MonoBehaviour
{
    private AudioSource Sounds;
    [SerializeField] private AudioClip[] spikeClips;

    // Start is called before the first frame update
    void Start()
    {
        Sounds = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Part1()
    {
        Sounds.PlayOneShot(spikeClips[0]);
    }
    private void Part2()
    {
        Sounds.PlayOneShot(spikeClips[1]);
    }
    private void Part3()
    {
        Sounds.PlayOneShot(spikeClips[2]);
    }

}
