using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScaner : MonoBehaviour
{
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
        Vector3 scanDirection;
        hitInfos.Clear();
        for (int i = 0; i < scanStepNum; i++)
        {
            scanDirection = Quaternion.Euler(0, i * scanStep - scanAngle, 0) * transform.forward;
            if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), scanDirection, out RaycastHit hitInfo, scanRange, 1 << LayerMask.NameToLayer("Player")))
            {
                hitInfos.Add(hitInfo);
            }
        }
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
        Gizmos.color = Color.red;
        foreach (var hitInfo in hitInfos)
        {
            Gizmos.DrawLine(transform.position + new Vector3(0, .5f, 0), hitInfo.point);
        }
    }
}
