using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShader : MonoBehaviour
{
    [SerializeField] ParticleSystem _particle;
    public void PlayParticle()
    {
        _particle.Play();
    }
}
