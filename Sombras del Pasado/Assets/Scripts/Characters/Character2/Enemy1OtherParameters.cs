using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1OtherParameters : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _stepClips;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Step_Sound()
    {
        AudioClip clip = StepClip();
        _audioSource.PlayOneShot(clip);
    }

    private AudioClip StepClip()
    {
        return _stepClips[UnityEngine.Random.Range(0, _stepClips.Length)];
    }
}
