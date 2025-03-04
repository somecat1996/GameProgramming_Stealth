using UnityEngine;

public enum EnemyState
{
    Patrol,
    Blinky,
    Inky,
    Pinky
}

public class EnemyController : MonoBehaviour
{
    public AgentController enemy;
    public EnemyState enemyState = EnemyState.Patrol;

    private int waypointIndex = 0;
    // Waypoints
    public Transform[] waypoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(EnemyState state)
    {
        enemyState = state;
    }

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

    public Vector3 NewWaypoint()
    {
        waypointIndex++;

        if (waypointIndex >= waypoints.Length) waypointIndex %= waypoints.Length;

        return waypoints[waypointIndex].position;
    }

    public Vector3 PlayerPosition()
    {
        return EnemyManager.instance.GetPlayerPosition();
    }

    public Vector3 PredictPlayerPosition()
    {
        return EnemyManager.instance.player.position + EnemyManager.instance.player.right * EnemyManager.instance.lookAheadDistance;
    }

    public Vector3 Encircle()
    {
        return Vector3.zero;
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
