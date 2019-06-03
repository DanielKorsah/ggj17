using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWCamera : MonoBehaviour
{
    #region singleton
    private static BWCamera instance;
    public static BWCamera Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    Transform player;
    Rigidbody2D pRB;
    float camSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            player = FindObjectOfType<BWPlayerController>()?.transform;
            pRB = player?.GetComponent<Rigidbody2D>();
            yield return null;
        }
    }

    public void ViewLoad(int gridsX, int gridsY)
    {
        transform.parent = null;
        transform.position = new Vector3(gridsX, gridsY - 0.5f, transform.position.z);
    }

    public void GoToPlayer()
    {
        if (player != null)
        {
            StartCoroutine(GoToPlayerRoutine());
        }
    }

    private IEnumerator GoToPlayerRoutine()
    {
        yield return null;
        while (true)
        {
            float dt = Time.fixedDeltaTime;

            Vector2 dir = player.position - transform.position;
            float ratio = 0.05f;
            float dirInfluence = dir.sqrMagnitude + ratio;

            Vector2 playerVel = pRB.velocity;

            Vector2 move = Vector2.zero;
            if (dir != Vector2.zero)
            {
                move += dir.normalized * (dirInfluence < 1.0f ? dirInfluence : 1.0f);
            }
            if (playerVel != Vector2.zero)
            {
                move += playerVel.normalized * (1.0f - (dirInfluence < 1.0f ? dirInfluence : 1.0f));
            }
            if (move != Vector2.zero)
            {
                move = move.normalized * camSpeed * dt;
            }
            if (dirInfluence - ratio <= move.sqrMagnitude)
            {
                transform.parent = player;
                transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
                break;
            }
            else
            {
                transform.position += (Vector3)move;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
