using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    public float walkPointRange = 10f;
    private bool walkPointSet;

    public float timeBetweenAttacks = 1f;
    [SerializeField] private UnityEvent onAttack = new UnityEvent();
    private bool alreadyAttacked;

    public float sightRange = 10f;
    public float attackRange = 2f;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject == null)
                playerObject = GameObject.Find("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh || player == null)
            return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (whatIsPlayer.value == 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            playerInSightRange = distanceToPlayer <= sightRange;
            playerInAttackRange = distanceToPlayer <= attackRange;
        }

        if (!playerInSightRange && !playerInAttackRange)
            patroling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange)
            AttackPlayer();
    }

    private void patroling()
    {
        if (!walkPointSet)
            searchWalkPoint();

        if (!walkPointSet)
            return;

        agent.SetDestination(walkPoint);
        if (!agent.pathPending && agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, 0.1f))
            walkPointSet = false;
    }

    public void searchWalkPoint()
    {
        float range = Mathf.Abs(walkPointRange);
        Vector3 offset = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
        Vector3 candidate = transform.position + offset;

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, range, agent.areaMask))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        if (directionToPlayer.sqrMagnitude > 0f)
            transform.rotation = Quaternion.LookRotation(directionToPlayer);

        if (alreadyAttacked)
            return;

        onAttack?.Invoke();
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), Mathf.Max(0f, timeBetweenAttacks));
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
