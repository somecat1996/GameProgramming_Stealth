using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

/// <summary>
/// Enemy movement
/// </summary>
public class AgentController : EntityMovement
{
    [SerializeField] private EnemyController controller;
    [SerializeField] private NavMeshAgent navAgent;

    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private float wanderDistance = 7f;
    [SerializeField] private float wanderRadius = 2f;

    //private readonly int jogParam = Animator.StringToHash("Jog");

    private void Start()
    {
        Assert.IsNotNull(navAgent);
    }

    private void Update()
    {
        switch (controller.enemyState)
        {
            // Move according waypoints
            case EnemyState.Patrol:
                if (!navAgent.hasPath)
                    SetNavAgentDestination(controller.GetNewTarget());
                if (Vector3.Distance(transform.position, navAgent.destination) < navAgent.stoppingDistance)
                    navAgent.ResetPath();
                break;
            // Move towards the player
            case EnemyState.Blinky:
                if (!EnemyManager.instance.playerLost)
                    SetNavAgentDestination(controller.GetNewTarget());
                else
                {
                    if (Vector3.Distance(transform.position, navAgent.destination) < navAgent.stoppingDistance)
                        navAgent.ResetPath();
                    if (!navAgent.hasPath)
                        EnemyManager.instance.AlertClear();
                }
                break;
            // Move ahead the player
            case EnemyState.Inky:
                SetNavAgentDestination(controller.GetNewTarget());
                break;
            // Move to encircle the player
            case EnemyState.Pinky:
                SetNavAgentDestination(controller.GetNewTarget());
                break;
            default:
                if (!navAgent.hasPath)
                    SetNavAgentDestination(controller.GetNewTarget());
                if (Vector3.Distance(transform.position, navAgent.destination) < navAgent.stoppingDistance)
                    navAgent.ResetPath();
                break;
        }
        RotateAgent();
    }

    /// <summary>
    /// Set target position for Nav Agent
    /// </summary>
    /// <param name="destination">Target position</param>
    public void SetNavAgentDestination(Vector3 destination)
    {
        if (NavMesh.SamplePosition(destination, out NavMeshHit hitInfo, wanderDistance + wanderRadius, NavMesh.AllAreas))
        {
            navAgent.destination = hitInfo.position;
        }
    }

    /// <summary>
    /// Rotate enemy towards its moving direction
    /// </summary>
    private void RotateAgent()
    {
        Vector3 direction = navAgent.steeringTarget - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// When enemy enter an obstacle, set new speed
    /// </summary>
    protected override void EnterObstacle()
    {
        base.EnterObstacle();
        navAgent.speed = GetSpeed();
    }

    /// <summary>
    /// When enemy exit an obstacle, reset its speed
    /// </summary>
    protected override void ExitObstacle()
    {
        base.ExitObstacle();
        navAgent.speed = GetSpeed();
    }

    private void OnDrawGizmos()
    {
        if (navAgent.hasPath)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(navAgent.destination + new Vector3(0, 1, 0), Vector3.one);
        }
    }
}
