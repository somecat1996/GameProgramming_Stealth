using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class AgentController : MonoBehaviour
{
    [SerializeField] private EnemyController controller;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Animator animator;

    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private float wanderDistance = 7f;
    [SerializeField] private float wanderRadius = 2f;

    private readonly int jogParam = Animator.StringToHash("Jog");

    private void Start()
    {
        Assert.IsNotNull(navAgent);
        Assert.IsNotNull(animator);

        animator.SetBool(jogParam, true);
    }

    private void Update()
    {
        if (!navAgent.hasPath)
            SetNavAgentDestination(controller.GetNewWaypoint());

        RotateAgent();

        if (Vector3.Distance(transform.position, navAgent.destination) < navAgent.stoppingDistance)
            navAgent.ResetPath();
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
}
