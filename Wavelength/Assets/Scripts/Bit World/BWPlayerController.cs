using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWPlayerController : MonoBehaviour
{
    float speed = 3.0f;
    Rigidbody2D body;
    Vector2 moveDirection;

    SpriteRenderer sprite;
    float spriteSize = 0.35f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        InputManager.Instance.MoveCall += SetMoveDirection;
    }

    public void PlayerEnterLevel()
    {
        StartCoroutine(ScalePlayerUp());        
    }

    public void PlayerExitLevel(Vector2 portal)
    {
        ControlsActive(false);
        StartCoroutine(ScalePlayerDown(portal));
    }

    private void ControlsActive(bool state)
    {
        InputManager.Instance.PlayerControlsActive(state);
    }

    IEnumerator ScalePlayerDown(Vector2 portal)
    {
        Vector2 diff = (Vector2)transform.position - portal;
        float animTime = 0.5f;
        float timeElapsed = 0.0f;
        bool notDone = true;
        while (notDone)
        {
            float cos = Mathf.Cos((timeElapsed / animTime) * (Mathf.PI * 0.5f));
            float size = spriteSize * cos;
            float sin = 1.0f - Mathf.Sin((timeElapsed / animTime) * (Mathf.PI * 0.5f));
            Vector2 modDiff = diff * sin;
            if (timeElapsed > animTime)
            {
                size = 0.0f;
                modDiff = Vector2.zero;
                notDone = false;
            }
            sprite.size = new Vector2(size, size);
            transform.position = portal + modDiff;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        FindObjectOfType<BitWorldMaker>().NextLevel();
    }

    IEnumerator ScalePlayerUp()
    {
        float animTime = 0.7f;
        float timeElapsed = 0.0f;
        bool notDone = true;
        while (notDone)
        {
            float size = spriteSize * Mathf.Sin((timeElapsed / animTime) * (Mathf.PI * 0.5f));
            if (timeElapsed > animTime)
            {
                size = spriteSize;
                notDone = false;
            }
            sprite.size = new Vector2(size, size);


            timeElapsed += Time.deltaTime;
            yield return null;
        }
        ControlsActive(true);
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
