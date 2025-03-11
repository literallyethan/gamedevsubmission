using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class Snowball : MonoBehaviour
{
    public ParticleSystem ps;
    public Animator animator;
    InputAction attack;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleCollisionEvent> hit = new List<ParticleCollisionEvent>();

    void Start()
    {
        attack = InputSystem.actions.FindAction("Attack");
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollEvents = ps.GetCollisionEvents(other, hit);
        //Rigidbody2D rb = other.GetComponent<Rigidbody2D>();          for buttons later

        // Get the current particles
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
        int numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numCollEvents; i++)
        {
            Vector2 collisionPos = hit[i].intersection; // Get collision position

            for (int j = 0; j < numParticlesAlive; j++)
            {
                if (Vector2.Distance(particles[j].position, collisionPos) < 0.1f)
                {
                    particles[j].remainingLifetime = 0; // Remove the particle
                }
            }
        }

        // Apply changes
        ps.SetParticles(particles, numParticlesAlive);
    }

    public void Shoot()
    {
        if (attack.WasPerformedThisFrame())
        {
            animator.SetTrigger("isAttacking");
            Debug.Log("Snowball!!");
            ps.Play();
        }
    }
}