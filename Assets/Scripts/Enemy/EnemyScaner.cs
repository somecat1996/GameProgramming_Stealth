using System.Collections.Generic;
using UnityEngine;

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
                LookAtPlayer();
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

    private void ScanForPlayer()
    {
        Vector3 scanDirection;
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
                }
            }
        }
    }

    private void LookAtPlayer()
    {

    }

    private void OnDrawGizmos()
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
}
