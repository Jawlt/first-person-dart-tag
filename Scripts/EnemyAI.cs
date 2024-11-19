using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float shootingRange = 15f;
    public float shootingInterval = 1f;
    public float movementThreshold = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private float lastShotTime;
    private Vector3 lastPosition;
    public bool isIt;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            ChasePlayer();
            // Only shoot if the enemy has moved the minimum threshold distance
            if (Vector3.Distance(transform.position, lastPosition) >= movementThreshold)
            {
                ShootAtPlayer();
                lastPosition = transform.position; // Update last position after shooting
            }
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
    }

    void ShootAtPlayer()
    {
        if (Time.time >= lastShotTime + shootingInterval)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            Vector3 direction = (player.position - gunPoint.position).normalized;
            bulletRb.velocity = direction * 10f;

            lastShotTime = Time.time;
            Destroy(bullet, 2f);
        }
    }

    public void SetIt(bool value)
    {
        isIt = value;
    }
}
