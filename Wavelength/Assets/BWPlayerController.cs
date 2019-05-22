using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWPlayerController : MonoBehaviour
{
    float speed = 1.7f;
    Rigidbody2D body;
    Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        body.MovePosition((Vector2)transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    public void GivePickup(Pickup pickup)
    {
        BWInventory.Instance.AddPickup(pickup);
        // ~~~ UI work
    }
}
