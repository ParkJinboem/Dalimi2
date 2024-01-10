using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used in https://www.youtube.com/watch?v=jKSz8JJnL4E
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class FireworksParticleSoundSystem : MonoBehaviour
{
    public AudioClip born;
    public AudioClip die;

    private ParticleSystem ps;
    private AudioSource audioSource;

    private int currentNumberOfParticles = 0;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (die != null &&
            ps.particleCount < currentNumberOfParticles)
        {
            audioSource.PlayOneShot(die);
        }
        if (born != null &&
            ps.particleCount > currentNumberOfParticles)
        {
            audioSource.PlayOneShot(born);
        }
        currentNumberOfParticles = ps.particleCount;
    }
}
