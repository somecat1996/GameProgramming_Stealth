using System.Collections;
using System.Collections.Generic;
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
    // Parent of waypoints
    public Transform waypointParent;

    public AgentController enemy;

    private int waypointIndex;
    private Transform[] waypoints;
    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetNewWaypoint()
    {
        waypointIndex ++;

        if (waypointIndex >= waypoints.Length) waypointIndex %= waypoints.Length;

        return waypoints[waypointIndex].position;
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
            Gizmos.DrawSphere(waypoints[endindex].position, 1);
        }
    }
}
