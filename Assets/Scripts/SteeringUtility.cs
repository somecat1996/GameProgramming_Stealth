using UnityEngine;

public static class SteeringUtility
{
    public static Vector3 Seek(Vector3 origin, Vector3 target)
    {
        Vector3 direction = target - origin;

        return direction;
    }

    public static Vector3 Seek(Transform origin, Transform target)
        => Seek(origin.position, target.position);

    public static Vector3 Flee(Vector3 origin, Vector3 target)
        => Seek(target, origin);

    public static Vector3 Flee(Transform origin, Transform target)
        => Seek(target.position, origin.position);

    public static Vector3 Wander(Transform origin, float wanderDistance, float wanderRadius)
    {
        Vector3 wanderPoint = origin.position + origin.forward * wanderDistance;
        Vector2 pointOnCircle = Random.insideUnitCircle.normalized * wanderRadius;

        Vector3 wanderTarget = wanderPoint + new Vector3(pointOnCircle.x, origin.position.y, pointOnCircle.y);
        Vector3 direction = wanderTarget - origin.position;
        
        DrawWanderDebug(origin, wanderRadius, wanderPoint, wanderTarget);

        return wanderTarget;
    }

    private static void DrawWanderDebug(Transform origin, float wanderRadius, Vector3 wanderPoint, Vector3 wanderTarget)
    {
        Debug.DrawLine(origin.position, wanderPoint, Color.red, 1);

        for (int i = 0; i < 360; i += 12)
        {
            Vector3 p1 = Quaternion.Euler(0, i, 0) * Vector3.right;
            Vector3 p2 = Quaternion.Euler(0, i + 12, 0) * Vector3.right;

            Vector3 start = wanderPoint + p1 * wanderRadius;
            Vector3 end = wanderPoint + p2 * wanderRadius;

            Debug.DrawLine(start, end, Color.red, 1);
        }

        Debug.DrawLine(origin.position, wanderTarget, Color.cyan, 1);
    }
}
