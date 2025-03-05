using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Player shoot obstacles
/// </summary>
public class PlayerShooter : MonoBehaviour
{
    // Virtual effect obstacle object
    public GameObject virtualObstacle;
    // Real obstacle object
    public GameObject obstacle;
    // Transfrom of Geo parent
    public Transform geo;
    // Position to hide virtual obstacle
    public Vector3 defaultPosition;
    // Offset to place the obstacle
    public Vector3 positionOffset;
    // Gold cost per shoot
    public int shootCost = 1;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(virtualObstacle);
    }

    // Update is called once per frame
    void Update()
    {
        // When button down, show virtual obstacle
        if (Input.GetMouseButtonDown(0))
        {
            virtualObstacle.SetActive(true);
            virtualObstacle.transform.position = GetObstaclePosition();
        }
        // When button pressed, move virtual obstacle
        if (Input.GetMouseButton(0))
        {
            virtualObstacle.transform.position = GetObstaclePosition();
        }
        // When button up, check cost and place the obstacle
        if (Input.GetMouseButtonUp(0))
        {
            virtualObstacle.SetActive(false);
            if (Wallet.instance.SpendGold(shootCost))
            {
                GameObject o = Instantiate(obstacle, geo);
                o.transform.position = GetObstaclePosition();
                virtualObstacle.transform.position = defaultPosition;

                UpdateNavMesh.instance.UpdateNavMeshSurface();
            }
        }
    }

    /// <summary>
    /// Get the obstacle position from mouse position
    /// </summary>
    /// <returns>Obstacle position</returns>
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
        // Hide the virtual obstacle if there is no valid position
        return defaultPosition;
    }
}
