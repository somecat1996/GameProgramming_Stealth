using UnityEngine;

/// <summary>
/// Player control and move
/// </summary>
public class PlayerMovement : EntityMovement
{
    private Rigidbody rb;
    // Player start positon
    private Vector3 originPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originPosition = transform.position;
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

    // When collide with an enemy, reset the game
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            transform.position = originPosition;
            EnemyManager.instance.AlertClear();
            Wallet.instance.ResetGold();
        }
    }
}
