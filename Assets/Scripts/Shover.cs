using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shover : MonoBehaviour
{
    public float shoveStrength = 100f;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
            {
                RagdollController ragdoll = hitInfo.collider.GetComponentInParent<RagdollController>();
                if (ragdoll)
                {
                    Vector3 colliderPos = hitInfo.collider.transform.position;
                    Vector3 force = (colliderPos - Camera.main.transform.position).normalized * shoveStrength;

                    ragdoll.ShoveRagdoll(force, hitInfo.point);
                }
            }
        }
    }
}
