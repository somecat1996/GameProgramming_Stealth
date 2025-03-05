using UnityEngine;

/// <summary>
/// Utility functions for nav AI
/// </summary>
public static class SteeringUtility
{
    /// <summary>
    /// Get the direction from origin to traget
    /// </summary>
    /// <param name="origin">Position of origin point</param>
    /// <param name="target">Position of target point</param>
    /// <returns>Direction from origin to target</returns>
    public static Vector3 Seek(Vector3 origin, Vector3 target)
    {
        Vector3 direction = target - origin;

        return direction;
    }

    /// <summary>
    /// Get the direction from origin to traget
    /// </summary>
    /// <param name="origin">Transform of origin point</param>
    /// <param name="target">Transform of target point</param>
    /// <returns>Direction from origin to target</returns>
    public static Vector3 Seek(Transform origin, Transform target)
        => Seek(origin.position, target.position);


    /// <summary>
    /// Get the direction from target to origin
    /// </summary>
    /// <param name="origin">Position of origin point</param>
    /// <param name="target">Position of target point</param>
    /// <returns>Direction from target to origin</returns>
    public static Vector3 Flee(Vector3 origin, Vector3 target)
        => Seek(target, origin);

    /// <summary>
    /// Get the direction from target to origin
    /// </summary>
    /// <param name="origin">Transform of origin point</param>
    /// <param name="target">Transform of target point</param>
    /// <returns>Direction from target to origin</returns>
    public static Vector3 Flee(Transform origin, Transform target)
        => Seek(target.position, origin.position);

    /// <summary>
    /// Get wander target
    /// </summary>
    /// <param name="origin">Transform of wander object</param>
    /// <param name="wanderDistance">Distance of the wander object should wander</param>
    /// <param name="wanderRadius">Radius of wander target deviate</param>
    /// <returns>Wander target</returns>
    public static Vector3 Wander(Transform origin, float wanderDistance, float wanderRadius)
    {
        Vector3 wanderPoint = origin.position + origin.forward * wanderDistance;
        Vector2 pointOnCircle = Random.insideUnitCircle.normalized * wanderRadius;

        Vector3 wanderTarget = wanderPoint + new Vector3(pointOnCircle.x, origin.position.y, pointOnCircle.y);
        Vector3 direction = wanderTarget - origin.position;
        
        DrawWanderDebug(origin, wanderRadius, wanderPoint, wanderTarget);

        return wanderTarget;
    }

    /// <summary>
    /// Draw wander infomation in scene
    /// </summary>
    /// <param name="origin">Transform of wander object</param>
    /// <param name="wanderRadius">Radius of wander target deviate</param>
    /// <param name="wanderPoint">Centre of the wander target</param>
    /// <param name="wanderTarget">Position of the wander target</param>
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
