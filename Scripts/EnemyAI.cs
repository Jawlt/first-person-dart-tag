using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float shootingRange = 15f;
    public float shootingInterval = 5f;
    public float randomMovementRange = 30f;
    public Transform movementCentrePoint;
    public float chaseSpeed = 8f;
    public float randomMoveSpeed = 8f;
    public float repathInterval = 2f;
    public float stuckDistanceThreshold = 0.5f;
    public float bulletSpeed = 20f;

    private NavMeshAgent agent;
    private Animator animator;
    private float lastShotTime;
    private bool isIt;
    private Vector3 lastPosition;
    private float repathTimer;
    private float stuckTimer;
    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        agent.updateRotation = true;
        lastPosition = transform.position;
        repathTimer = 0f;
        stuckTimer = 0f;

        if (movementCentrePoint == null)
        {
            movementCentrePoint = this.transform;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing on the enemy object. Please add one.");
        }
    }

    void Update()
    {
        if (isIt)
        {
            HandleItBehavior();
        }
        else
        {
            HandleRandomMovement();
        }

        CheckIfStuck();
    }

    void HandleItBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > 2f)
        {
            ChasePlayer();
        }
        else
        {
            agent.ResetPath();
            animator.SetBool("isRunning", false);
        }

        if (distanceToPlayer <= shootingRange)
        {
            ShootAtPlayer();
        }
    }

    void HandleRandomMovement()
    {
        repathTimer += Time.deltaTime;
        if (agent.remainingDistance <= agent.stoppingDistance || repathTimer >= repathInterval)
        {
            Vector3 point;
            if (RandomPoint(movementCentrePoint.position, randomMovementRange, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.speed = randomMoveSpeed;
                agent.SetDestination(point);
                animator.SetBool("isRunning", true);
            }

            repathTimer = 0f;
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void ChasePlayer()
    {
        if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(hit.position);
            animator.SetBool("isRunning", true);
        }
        else
        {
            Debug.LogWarning("Unable to find a valid path to the player.");
        }
    }
    void ShootAtPlayer()
    {
        if (Time.time >= lastShotTime + shootingInterval)
        {
            if (player == null)
            {
                Debug.LogError("Player reference is missing! Ensure the 'player' Transform is assigned.");
                return;
            }

            Vector3 direction = (player.position - gunPoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(direction));
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.velocity = direction * bulletSpeed;
            }
            else
            {
                Debug.LogError("Rigidbody missing on bullet prefab!");
            }

            lastShotTime = Time.time;
            Destroy(bullet, 2f);

            if (audioSource != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource not assigned to EnemyAI!");
            }
            Debug.Log($"Bullet shot towards player. Direction: {direction}");
        }
    }

    void CheckIfStuck()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved < stuckDistanceThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= repathInterval)
            {
                JumpForward();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    void JumpForward()
    {
        if (rb != null && agent.isOnNavMesh)
        {
            Vector3 jumpDirection = transform.forward + Vector3.up;
            rb.AddForce(jumpDirection * 5f, ForceMode.Impulse);
            Debug.Log("Enemy jumped forward to get unstuck!");
        }
    }

    public void SetIt(bool value)
    {
        isIt = value;
    }
}
