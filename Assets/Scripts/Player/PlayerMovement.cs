using UnityEngine;

public class PlayerMovement : EntityMovement
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(-vertical, 0, horizontal);

        rb.velocity = direction.normalized * GetSpeed();
        //rb.MovePosition(transform.position + direction.normalized * GetSpeed() * Time.deltaTime);

        if (direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(new Vector3(-horizontal, 0, -vertical));
    }
}
