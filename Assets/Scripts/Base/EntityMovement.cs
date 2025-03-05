using UnityEngine;

/// <summary>
/// Base class for moving enetities
/// </summary>
public class EntityMovement : MonoBehaviour
{
    public float speed = 2f;
    public int obstacleDecelerateRate = 2;
    private bool onObstacle;
    // Start is called before the first frame update
    void Start()
    {
        onObstacle = false;
    }

    /// <summary>
    /// Get current speed
    /// </summary>
    /// <returns>Curret speed</returns>
    public float GetSpeed()
    {
        if (onObstacle) return speed / obstacleDecelerateRate;
        else return speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle") EnterObstacle();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle") ExitObstacle();
    }

    /// <summary>
    /// Call on entering obstacle
    /// </summary>
    protected virtual void EnterObstacle()
    {
        onObstacle = true;
    }

    /// <summary>
    /// Call on exiting obstacle
    /// </summary>
    protected virtual void ExitObstacle()
    {
        onObstacle = false;
    }
}
