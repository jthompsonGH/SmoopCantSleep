using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ParticleSeek : MonoBehaviour
{
    public float force = 15f;
    public float waitTime = .4f;
    GameObject player;
    Transform target;
    ParticleSystem ps;
    CinemachineImpulseSource impulse;
    float moveTimer;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    private void LateUpdate()
    {
        moveTimer += Time.deltaTime;

        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player)
        {
            if (player.transform.GetChild(0).tag == "Hat")
            {
                target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(9).transform;
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(7).transform;
            }
        }

        if (target != null)
        {
            if (moveTimer >= waitTime)
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
                ps.GetParticles(particles);

                for (int i = 0; i < particles.Length; i++)
                {
                    ParticleSystem.Particle p = particles[i];

                    Vector3 particleWorldPosition;

                    if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
                    {
                        particleWorldPosition = transform.TransformPoint(p.position);
                    }
                    else if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Custom)
                    {
                        particleWorldPosition = ps.main.customSimulationSpace.TransformPoint(p.position);
                    }
                    else
                    {
                        particleWorldPosition = p.position;
                    }

                    Vector3 targetDirection = (target.position - p.position).normalized;

                    Vector3 seekForce = targetDirection * force * Time.deltaTime;

                    if (Mathf.Abs(target.position.x - p.position.x) < .1f && Mathf.Abs(target.position.y - p.position.y) < .2f)
                    {
                        p.remainingLifetime = -1f;
                        if (impulse)
                        {
                            impulse.GenerateImpulse();
                        }
                    }
                    else if (Mathf.Abs(target.position.x - p.position.x) < .8f && Mathf.Abs(target.position.y - p.position.y) < 1f)
                    {
                        p.position = Vector3.MoveTowards(p.position, target.position, force * Time.deltaTime);
                    }
                    else
                    {
                        p.velocity += seekForce;
                        p.position = Vector3.MoveTowards(p.position, target.position, (force * Time.deltaTime) / 5f);
                    }

                    particles[i] = p;
                }

                ps.SetParticles(particles, particles.Length);
            }
        }
    }
}
