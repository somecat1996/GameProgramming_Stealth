using UnityEngine;

/// <summary>
/// Enemy behavior state
/// </summary>
public enum EnemyState
{
    Patrol,
    Blinky,
    Inky,
    Pinky
}

/// <summary>
/// Enemy control and movement
/// </summary>
public class EnemyController : MonoBehaviour
{
    public AgentController enemy;
    public EnemyState enemyState = EnemyState.Patrol;

    private int waypointIndex = -1;
    // Waypoints
    public Transform[] waypoints;

    public MeshRenderer enemyBody;
    public bool canSeePlayer;

    // Start is called before the first frame update
    void Start()
    {
        canSeePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Patrol:
                break;
            case EnemyState.Blinky:
                EnemyManager.instance.ActivateOtherEnemy(this);
                break;
            case EnemyState.Inky:
                break;
            case EnemyState.Pinky:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Set enemy state
    /// </summary>
    /// <param name="state">Target state</param>
    public void SetState(EnemyState state)
    {
        enemyState = state;
    }

    /// <summary>
    /// Find a new target position for nav agent
    /// </summary>
    /// <returns>Target position</returns>
    public Vector3 GetNewTarget()
    {
        switch (enemyState)
        {
            case EnemyState.Patrol:
                return NewWaypoint();
            case EnemyState.Blinky:
                return PlayerPosition();
            case EnemyState.Inky:
                return PredictPlayerPosition();
            case EnemyState.Pinky:
                return Encircle();
            default:
                return NewWaypoint();
        }
    }

    /// <summary>
    /// Get next waypoint position
    /// </summary>
    /// <returns>Waypoint position</returns>
    public Vector3 NewWaypoint()
    {
        waypointIndex++;

        if (waypointIndex >= waypoints.Length) waypointIndex %= waypoints.Length;

        return waypoints[waypointIndex].position;
    }

    /// <summary>
    /// Get player position
    /// </summary>
    /// <returns>Player position</returns>
    public Vector3 PlayerPosition()
    {
        return EnemyManager.instance.GetPlayerPosition();
    }

    /// <summary>
    /// Get position ahead of player's moving direction
    /// </summary>
    /// <returns>Predict position</returns>
    public Vector3 PredictPlayerPosition()
    {
        return EnemyManager.instance.player.position + EnemyManager.instance.player.right * EnemyManager.instance.lookAheadDistance;
    }

    /// <summary>
    /// Get a position that can encicle the player
    /// </summary>
    /// <returns>Encicle position</returns>
    public Vector3 Encircle()
    {
        if (EnemyManager.instance.GetEnemy(EnemyState.Blinky, out EnemyController blinkyEnemy) && EnemyManager.instance.GetEnemy(EnemyState.Pinky, out EnemyController pinkyEnemy))
        {
            Vector3 midPoint = (blinkyEnemy.transform.position + pinkyEnemy.transform.position) / 2f;
            Vector3 playerPosition = EnemyManager.instance.GetPlayerPosition();
            Vector3 direction = (EnemyManager.instance.GetPlayerPosition() - midPoint).normalized;

            return playerPosition + direction * EnemyManager.instance.lookAheadDistance;
        }
        enemyState = EnemyState.Patrol;
        return NewWaypoint();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            int startIndex = i - 1;
            int endindex = i;
            if (startIndex < 0) startIndex = waypoints.Length - 1;
            if (endindex == waypointIndex)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(waypoints[startIndex].position, waypoints[endindex].position);
            Gizmos.DrawSphere(waypoints[endindex].position, 0.5f);
        }
    }
}
