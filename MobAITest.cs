using UnityEngine;
using PVR.PSharp;
using UnityEngine.AI;
using static RevoStaticMethodHolder;
using System.Collections;


public class MobAITest : PSharpBehaviour
{
    [Tooltip("This is the max distance the AI will wander from its current position at once!")]
    public float MaxWanderDistance = 5f;
    [Tooltip("This is the minimum distance the AI will wander from its current position at once!")]
    public float MinWanderDistance = 1f;
    private NavMeshAgent agent;
	private Animator anim;
	private PSharpPlayer target;
	private bool walk = false;
    public float rotationSpeed = 5f;
    public float forwardThreshold = 1f;
    private bool hasTarget = false;
    private AudioSource audioSource;
    public int mobHealth = 100;
    private SpawnMob mobSpawner;
    private bool hasDied = false;
    private float deathTimer = 2f;
    //private int currentRetries = 0;
    //private int MaxRetries = 3;
    private bool hasWanderPosition = false;
    private float wanderTimer = 0f;
    private float stopTimer = 0f;
    public float timeTilWander = 10f;
    public float dodgeDistance = 1.0f;
    public float dodgeSpeed = 1.0f;

    private void Start()
	{
        mobSpawner = (SpawnMob)transform.parent.GetComponent(typeof(SpawnMob));
        audioSource = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
	}

    // Start Targeting Player if it has no target
    public override void OnPlayerTriggerEnter(PSharpPlayer player)
    {
        if (!hasTarget)
        {
            audioSource.Play();
            target = player;
            agent.SetDestination(player.GetPosition());
            hasTarget = true;
        }
    }
    // Clear if current target is out of Range
    public override void OnPlayerTriggerExit(PSharpPlayer player)
    {
        if (player == target)
        {
            hasTarget = false ;
            target = null ;
        }
    }
    // Player inside of Mob damage collider
    public void DodgePlayer(Vector3 playerPosition)
    {
        // Calculate the direction from the mob to the player
        Vector3 dodgeDirection = (transform.position - playerPosition).normalized;

        // Move the mob based on dodge direction and dodge speed
        transform.position += dodgeSpeed * Time.deltaTime * dodgeDirection;
    }

    // Clear if current target dies
    public void PlayerKilled(PSharpPlayer player)
    {
        if (player == target)
        {
            hasTarget = false;
            target = null;
        }
    }
    // Reduce Health when player attacks
    public void DealtDamage(int damage)
    {
        mobHealth -= damage;
        if (mobHealth < 0 && !hasDied)
        {
            hasDied = true ;
            anim.SetTrigger("Death");
            mobSpawner.MobDied();
        }
    }
    private void Update()
	{
        // Mob Has Died
        if (hasDied)
        {
            hasTarget = false;
            walk = false;

            deathTimer -= Time.deltaTime;
            if (deathTimer < 0)
            {
                hasDied = false;
                deathTimer = 2f;
                gameObject.SetActive(false);
            }
        }
        else
        {
            // Player in Detection Range
            if (hasTarget)
            {
                Vector3 currentTarget = target.GetPosition();
                Vector3 directionToTarget = (currentTarget - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, directionToTarget);

                // Walk to Target
                if (walk && agent.remainingDistance <= agent.stoppingDistance)
                {
                    walk = false;
                    agent.SetDestination(transform.position);
                    // Debug.Log($"{transform.name} has started walking towards Target");
                }
                // Turn towards target if its not infront 
                else if (agent.remainingDistance <= agent.stoppingDistance && dot < forwardThreshold)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                // Target Moved so Walk to It
                if (!walk && Vector3.Distance(transform.position, currentTarget) > agent.stoppingDistance + 1f)
                {
                    walk = true;
                    anim.SetTrigger("Walk");
                    agent.SetDestination(currentTarget);
                    // Debug.Log($"{transform.name} has started walking towards Target");
                }
                // At Target so Attack
                else if (!walk && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                {
                    anim.SetTrigger("Attack");
                    // Debug.Log($"{transform.name} has started attacking Target");
                }
            }
            // Player out of Detection Range so Idle
            if (!hasTarget && !hasWanderPosition && walk)
            {
                agent.SetDestination(transform.position);
                walk = false;
                anim.SetTrigger("StopWalk");
               // Debug.Log($"Player out of range of {transform.name}");
            }
            // No player in range so start wandering if the timer says so
            else if (!hasTarget && !hasWanderPosition && wanderTimer >= timeTilWander)
            {
               // Debug.Log($"{transform.name} has started wandering");
                StartCoroutine(StartWander());
                hasWanderPosition = true;
                wanderTimer = 0;
                stopTimer = 0;
            }
            // Wandering stop logic and anim control
            if (hasWanderPosition && agent.remainingDistance <= agent.stoppingDistance && stopTimer >= 1)
            {
                hasWanderPosition = false;
                walk = false;
                anim.SetTrigger("StopWalk");
               // Debug.Log($"{transform.name} is done wandering");
            }
            // wandering timer counts when its not wandering
            if (!hasWanderPosition)
            {
                wanderTimer += Time.deltaTime;
            }
            if (hasWanderPosition)
            {
                stopTimer += Time.deltaTime;
            }
        }
    }
    private IEnumerator StartWander()
    {
        anim.SetTrigger("Walk");
        walk = true ;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            yield return null;
           // do nothing
        }
        SetAIDestination(RevoStaticMethods.CalculateRandomForwardPosition(transform, MaxWanderDistance, MinWanderDistance));
    }
    public void SetAIDestination(Vector3 target)
    {
        agent.SetDestination(target);
        /*
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            currentRetries++;
            if (currentRetries <= MaxRetries)
            {
                SetAIDestination(RevoStaticMethods.CalculateRandomPosition(transform, MaxWanderDistance, MinWanderDistance));
            }
            else
            {
                MaxWanderDistance *= 2;
            }
        }
        else
        {
            currentRetries = 0;
            hasWanderPosition = true;
        }
        */
    }
}