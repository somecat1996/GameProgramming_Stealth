using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShooter : MonoBehaviour
{
    public GameObject virtualObstacle;
    public GameObject obstacle;
    public Transform geo;
    public Vector3 defaultPosition;
    public Vector3 positionOffset;
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(virtualObstacle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            virtualObstacle.SetActive(true);
            virtualObstacle.transform.position = GetObstaclePosition();
        }

        if (Input.GetMouseButton(0))
        {
            virtualObstacle.transform.position = GetObstaclePosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            virtualObstacle.SetActive(false);
            GameObject o = Instantiate(obstacle, geo);
            o.transform.position = GetObstaclePosition();
            virtualObstacle.transform.position = defaultPosition;
        }
    }

    private Vector3 GetObstaclePosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, 100f, 1 << LayerMask.NameToLayer("Ground")))
        {
            Vector3 playerShootDirection = hitInfo.point - transform.position - Vector3.up;
            if (Physics.Raycast(transform.position + Vector3.up, playerShootDirection, out RaycastHit hitInfo2, 100f, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Wall")))
            {
                return new Vector3(hitInfo2.point.x, positionOffset.y, hitInfo2.point.z);
            }
        }
        return defaultPosition;
    }
}
