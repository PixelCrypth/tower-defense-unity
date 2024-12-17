using UnityEngine;

public class LiveParticle : MonoBehaviour
{
    public ParticleSystem particleSystemLive; // Particle system for this life
    private bool hasPlayed = false; // Ensures the particle only plays once

    void Start()
    {
        if (particleSystemLive != null)
        {
            particleSystemLive.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Clear particles on start
        }
    }

    /// <summary>
    /// Plays the particle system.
    /// </summary>
    public void PlayParticles()
    {
        if (particleSystemLive != null && !hasPlayed)
        {
            hasPlayed = true; // Prevent particles from playing again
            particleSystemLive.Play();
        }
    }

    /// <summary>
    /// Stops the particle system.
    /// </summary>
    public void StopParticles()
    {
        if (particleSystemLive != null)
        {
            hasPlayed = false; // Reset particle play state
            particleSystemLive.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Properly stop and clear particles
        }
    }
}
