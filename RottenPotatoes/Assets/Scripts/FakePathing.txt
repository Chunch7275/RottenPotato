using UnityEngine;
using UnityEngine.AI;

public class FakePathing : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player;
    public float chaseRange = 5f;
    public float chaseSpeedMultiplier = 1.3f; // Multiplier for chase speed
    public AudioClip walkingSound; // Walking sound clip
    public GameObject playerobj;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Vector3 initialPosition;
    private bool isChasing = false;
    private bool isWalkingSoundPlaying = false; // Track if the walking sound is playing
    private playerstats playerstats;
    private Animator animator;
    private AudioSource audioSource;
    private GameManager gameManager;
    private Rigidbody rb;
    private float baseSpeed;

    private float speedIncreaseInterval = 60f; // Interval to increase speed (in seconds)
    private float elapsedTime = 0f;            // Elapsed time to track total time
    private float maxSpeedMultiplier = 1.35f;  // Max multiplier (135%)

    void Start()
    {
        playerobj = GameObject.FindGameObjectWithTag("Player");
        playerstats = playerobj.GetComponent<playerstats>();
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;

        animator = GetComponent<Animator>();
        baseSpeed = agent.speed;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the enemy!");
        }

        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    void Update()
    {

        /*  Not needed, most of this was for handling speed and "player Chasing"
         *  
         *  Increase enemy speed every minute up to the maximum multiplier after 30 minutes
        IncreaseEnemySpeedOverTime();

        chaseRange = chaseRange * (playerstats.stealthValue / 5);
       
        if (gameManager != null && gameManager.gameOver)
        {
            StopAndFreezeEnemy();
            return;
        }

        if (animator != null)
        {
            animator.speed = 1;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                agent.speed = baseSpeed * chaseSpeedMultiplier; // Increase speed when chasing
                animator.SetBool("isChasing", true);
            }
            agent.SetDestination(player.position);
        }
        else if (isChasing && distanceToPlayer <= chaseRange * 2)
        {
            agent.SetDestination(player.position);
        }
        else if (isChasing && distanceToPlayer > chaseRange * 2)
        {
            isChasing = false;
            agent.speed = baseSpeed; // Reset speed when no longer chasing
            GoToNextWaypoint();
            animator.SetBool("isChasing", false);
        }
        */
        else if (!isChasing && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextWaypoint();
        }

        HandleWalkingSound();
    }
    /*
     * Not Needed
    void IncreaseEnemySpeedOverTime()
    {
        // Accumulate elapsed time each frame
        elapsedTime += Time.deltaTime;

        // Every full minute, increase the base speed incrementally
        if (elapsedTime >= speedIncreaseInterval && agent.speed < baseSpeed * maxSpeedMultiplier)
        {
            // Calculate the new speed based on the elapsed minutes
            float totalMinutes = elapsedTime / speedIncreaseInterval;
            float speedMultiplier = 1 + (0.35f * Mathf.Min(totalMinutes, 30) / 30); // Max multiplier is 1.35 after 30 minutes

            // Set the agent speed to the new increased value
            agent.speed = baseSpeed * speedMultiplier;

            // Log the current multiplier with a debug message each time the speed increases
            Debug.Log("Enemy speed increased! New speed is " + (speedMultiplier * 100).ToString("F1") + "% of base speed.");

            // Reset the timer for the next minute
            elapsedTime -= speedIncreaseInterval;
        }
    }
    */

    // This handled all movement. 

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }
/*
    void HandleWalkingSound()
    {
        if (agent.velocity.magnitude > 0 && !isWalkingSoundPlaying)
        {
            if (walkingSound != null)
            {
                audioSource.clip = walkingSound;
                audioSource.loop = true;
                audioSource.Play();
                isWalkingSoundPlaying = true;
            }
        }
        else if (agent.velocity.magnitude == 0 && isWalkingSoundPlaying)
        {
            audioSource.Stop();
            isWalkingSoundPlaying = false;
        }
    }

    void StopAndFreezeEnemy()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (animator != null)
        {
            animator.speed = 0;
        }

        if (audioSource != null && isWalkingSoundPlaying)
        {
            audioSource.Stop();
            isWalkingSoundPlaying = false;
        }
*/
        Debug.Log("Enemy movement and animations frozen due to game over.");
    }
}
