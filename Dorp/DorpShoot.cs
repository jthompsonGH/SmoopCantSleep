using UnityEngine;

public class DorpShoot : MonoBehaviour
{
    //public GameObject Smoop;
    //public GameObject Head;
    public Animator animator;
    public LayerMask layerMask;
    public float force;
    public float fireRate;
    public float range;
    public GameObject dorpBall;
    public GameObject shootSplash;
    public Transform dorpMouth;
    float nextTimeToFire;
    Transform player;
    Vector2 target;
    Vector2 Direction;
    GameObject Head;
    GameObject Smoop;
    bool attack;

    void Start()
    {
        int parentHealth = GetComponent<DorpController>().maxHealth;
        
        switch (parentHealth)
        {
            case 1:
                fireRate = 6f;
                break;
            case 2:
                fireRate = 4.5f;
                break;
            case 3:
                fireRate = 3f;
                break;
            case 4:
                fireRate = 2.5f;
                break;
            case 5:
                fireRate = 2f;
                break;
            case 6:
                fireRate = 1.5f;
                break;
            default:
                fireRate = 1f;
                break;
        }

        nextTimeToFire = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Head = GameObject.FindGameObjectWithTag("Head");
        Smoop = GameObject.FindGameObjectWithTag("Player");

        if (Head != null || Smoop != null)
        {
            if (Head != null)
            {
                player = GameObject.FindGameObjectWithTag("Head").transform;
                target = new Vector2(player.position.x, player.position.y);
            }
            if (Smoop != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
                target = new Vector2(player.position.x, player.position.y);
            }

            Direction = (target - (Vector2)transform.position);

            RaycastHit2D rayCast = Physics2D.Raycast(this.gameObject.transform.position, Direction, range, layerMask);

            if (rayCast)
            {
                if (rayCast.collider.CompareTag("Player") || rayCast.collider.CompareTag("Head"))
                {
                    GoShoot();
                }
            }
        }

        /*if (Vector2.Distance(transform.position, player.position) <= range)
        {
            GoShoot();
        }*/

        DorpAttack();
        AttackReset();
    }
    void GoShoot()
    {
        if (Time.time >= nextTimeToFire)
        {
            attack = true;
            nextTimeToFire = Time.time + 2f / fireRate;
            Shoot();
        }
    }

    void DorpAttack()
    {
        if (attack)
        {
            animator.SetTrigger("isShoot");
        }
    }
    void AttackReset()
    {
        attack = false;
    }

    private void Shoot()
    {
        GameObject ballInstance = Instantiate(dorpBall, dorpMouth.position, dorpMouth.rotation);
        ballInstance.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * force);
        Instantiate(shootSplash, dorpMouth.transform);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
