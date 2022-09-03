using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystemLeft;
    [SerializeField] private ParticleSystem _particleSystemRight;

    public void PlayRight() => _particleSystemRight.Play();
    public void PlayLeft() => _particleSystemLeft.Play();
}
