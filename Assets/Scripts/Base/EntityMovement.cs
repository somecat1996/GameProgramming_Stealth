using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected virtual void EnterObstacle()
    {
        onObstacle = true;
    }

    protected virtual void ExitObstacle()
    {
        onObstacle = false;
    }
}
