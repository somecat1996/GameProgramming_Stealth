using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

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
            case EnemyState.Patrol:
                if (!navAgent.hasPath)
                    SetNavAgentDestination(controller.GetNewTarget());
                if (Vector3.Distance(transform.position, navAgent.destination) < navAgent.stoppingDistance)
                    navAgent.ResetPath();
                break;
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
            case EnemyState.Inky:
                SetNavAgentDestination(controller.GetNewTarget());
                break;
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

    public void SetNavAgentDestination(Vector3 destination)
    {
        if (NavMesh.SamplePosition(destination, out NavMeshHit hitInfo, wanderDistance + wanderRadius, NavMesh.AllAreas))
        {
            navAgent.destination = hitInfo.position;
        }
    }

    private void RotateAgent()
    {
        Vector3 direction = navAgent.steeringTarget - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    protected override void EnterObstacle()
    {
        base.EnterObstacle();
        navAgent.speed = GetSpeed();
    }

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
