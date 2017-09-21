﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDown : MonoBehaviour
{
    RectTransform rt;
    Vector3 downPos = new Vector3(0, 0, 0);
    bool isDown;

    void Start()
    {
        //get the recttransform of the sliding panel 
        rt = GetComponent<RectTransform>();
    }
    void Update()
    {
        Vector3 upPos = new Vector3(0, Screen.height, 0);

        //toggle panel up or down with Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isDown)
            {
                rt.localPosition = downPos;
                isDown = true;
                Debug.Log("Down");
            }
            else
            {
                rt.localPosition = upPos;
                Debug.Log(rt.localPosition = upPos);
                isDown = false;
                Debug.Log("Up");
            }

        }
    }
}
