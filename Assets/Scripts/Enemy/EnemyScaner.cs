using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy scane for player
/// </summary>
public class EnemyScaner : MonoBehaviour
{
    [SerializeField] private EnemyController controller;
    public float scanRange = 10f;
    public float scanAngle = 50f;
    public int scanStepNum = 20;
    public float scanStep = 5f;

    private List<RaycastHit> hitInfos;

    private void Start()
    {
        scanStep = 2 * scanAngle / scanStepNum;
        hitInfos = new List<RaycastHit>();
    }

    private void Update()
    {
        switch (controller.enemyState)
        {
            case EnemyState.Patrol:
                ScanForPlayer();
                break;
            case EnemyState.Blinky:
                if (EnemyManager.instance.playerLost) ScanForPlayer();
                else LookAtPlayer();
                break;
            case EnemyState.Inky:
                ScanForPlayer();
                break;
            case EnemyState.Pinky:
                ScanForPlayer();
                break;
            default:
                ScanForPlayer();
                break;
        }
    }

    /// <summary>
    /// Enemy use multiple RayCast to scan for player
    /// </summary>
    private void ScanForPlayer()
    {
        Vector3 scanDirection;
        bool seePlayer = false;
        hitInfos.Clear();
        for (int i = 0; i < scanStepNum; i++)
        {
            scanDirection = Quaternion.Euler(0, i * scanStep - scanAngle, 0) * transform.forward;
            if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), scanDirection, out RaycastHit hitInfo, scanRange, 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Wall")))
            {
                hitInfos.Add(hitInfo);
                if (hitInfo.transform.tag == "Player")
                {
                    EnemyManager.instance.OnDetectPlayer(controller);
                    seePlayer = true;
                }
            }
        }
        controller.canSeePlayer = seePlayer;
    }

    /// <summary>
    /// Enemy uses one RayCast to check if it can see the player
    /// </summary>
    private void LookAtPlayer()
    {
        Vector3 scanDirection = (EnemyManager.instance.GetPlayerPosition() - transform.position).normalized;
        hitInfos.Clear();
        if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), scanDirection, out RaycastHit hitInfo, scanRange, 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Wall")))
        {
            hitInfos.Add(hitInfo);
            if (hitInfo.transform.tag == "Player")
            {
                EnemyManager.instance.UpdatePlayerDetection(true);
                controller.canSeePlayer = true;
            }
            else
            {
                EnemyManager.instance.UpdatePlayerDetection(false);
                controller.canSeePlayer = false;
            }
        }
        else
        {
            EnemyManager.instance.UpdatePlayerDetection(false);
            controller.canSeePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        switch (controller.enemyState)
        {
            case EnemyState.Patrol:
                DrawScan();
                break;
            case EnemyState.Blinky:
                DrawVision();
                break;
            case EnemyState.Inky:
                DrawScan();
                break;
            case EnemyState.Pinky:
                DrawScan();
                break;
            default:
                DrawScan();
                break;
        }
    }

    /// <summary>
    /// Draw scan RayCast and result
    /// </summary>
    private void DrawScan()
    {
        Gizmos.color = Color.yellow;
        Vector3 scanDirection;
        for (int i = 0; i < scanStepNum; i++)
        {
            scanDirection = Quaternion.Euler(0, i * scanStep - scanAngle, 0) * transform.forward;
            Gizmos.DrawLine(transform.position + new Vector3(0, .5f, 0), transform.position + scanDirection * scanRange + new Vector3(0, .5f, 0));
        }
        if (hitInfos != null)
        {
            foreach (var hitInfo in hitInfos)
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    Gizmos.color = Color.red;
                else Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + new Vector3(0, .5f, 0), hitInfo.point);
            }
        }
    }

    /// <summary>
    /// Draw vision result
    /// </summary>
    private void DrawVision()
    {
        if (hitInfos != null)
        {
            foreach (var hitInfo in hitInfos)
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    Gizmos.color = Color.red;
                else Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + new Vector3(0, .5f, 0), hitInfo.point);
            }
        }
    }
}
