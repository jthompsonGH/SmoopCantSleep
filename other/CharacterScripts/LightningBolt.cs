using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningBolt : MonoBehaviour
{
    public ParticleSystem buildUp;
    public GameObject bigBlast;
    public BoxCollider2D boxCollider;
    public LayerMask playerMask;
    public CinemachineImpulseSource impulse;
    public UnityEngine.Rendering.Universal.Light2D _Light;
    public SoundObject soundObject;
    public float blastRange;
    public float shineSpeed;
    float explodeTimer;
    bool triggered;
    CullingManager cull;
    GameObject player;

    private void Awake()
    {
        explodeTimer = 0f;

        if (GameObject.Find("CullingManager"))
        {
            cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
        }

        if (cull)
        {
            cull.AddThis(this.gameObject);
        }
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (triggered)
            {
                explodeTimer += Time.deltaTime;

                if (_Light.intensity <= 10f)
                {
                    _Light.intensity += Time.deltaTime * shineSpeed;
                }
            }

            if (explodeTimer >= .4f)
            {
                explodeTimer = 0f;
                triggered = false;
                buildUp.Stop();
                RaycastHit2D boxCast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.extents, 0f, Vector2.down, blastRange, playerMask);

                if (boxCast)
                {
                    switch (boxCast.collider.tag)
                    {
                        case "Player":
                            player.GetComponent<PlayerController>().TakeDamage(3f);
                            break;
                        case "Knight":
                            KnightController thisKnight = boxCast.collider.gameObject.GetComponent<KnightController>();

                            if (thisKnight.isShielded)
                            {
                                thisKnight.ShieldBreak();
                            }
                            break;
                        default:
                            break;
                    }
                }

                Explosion();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                buildUp.Play();
                triggered = true;
            }
        }
    }

    void Explosion()
    {
        _Light.intensity = 0f;
        Instantiate(bigBlast, transform.position, transform.rotation);
        impulse.GenerateImpulse();

        soundObject.PlaySound(0);
    }
}
