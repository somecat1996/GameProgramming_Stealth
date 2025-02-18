using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public enum RagdollState {  Animate = 0, Ragdoll = 1 };

    [SerializeField] Animator animator;

    private RagdollState state = RagdollState.Animate;
    private Rigidbody[] bodies;

    // Start is called before the first frame update
    public void Start()
    {
        bodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody body in bodies) body.isKinematic = true;
    }

    public void ShoveRagdoll(Vector3 force, Vector3 point)
    {
        foreach (Rigidbody body in bodies) { body.isKinematic = false; }

        state = RagdollState.Ragdoll;
        animator.enabled = false;

        Rigidbody closest = bodies.OrderBy(rb => Vector3.Distance(rb.position, point)).First();
        closest.AddForceAtPosition(force, point, ForceMode.Impulse);
    }
}
