using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDown : MonoBehaviour
{
    RectTransform rt;
    Vector3 start;
    Vector3 downvec;
    Vector3 start1080 = new Vector3(0, 1070, 0);
    Vector3 start720 = new Vector3(0, 723, 0);
    bool isDown;

    void Start()
    {

        downvec = new Vector3(0, 0, 0);
        rt = GetComponent<RectTransform>();
        if (Screen.height == 1080)
        {
            start = start1080;
        }
        else if (Screen.height == 720)
        {
            start = start720;
        }
        else
        {
            Debug.Log("You're using a non approved resolution");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isDown)
            {
                rt.localPosition = downvec;
                isDown = true;
            }
            else
            {
                rt.localPosition = start;
                Debug.Log(rt.localPosition = start);
                isDown = false;
            }

        }
    }
}
