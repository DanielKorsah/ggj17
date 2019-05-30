using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWPlayerController : MonoBehaviour
{
    float speed = 3.0f;
    Rigidbody2D body;
    Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InputManager.Instance.MoveCall += SetMoveDirection;
    }

    private void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    void FixedUpdate()
    {
        body.MovePosition((Vector2)transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    public void GivePickup(Pickup pickup)
    {
        BWInventory.Instance.AddPickup(pickup);
    }
}
