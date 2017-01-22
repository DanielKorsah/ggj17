using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialTurn : MonoBehaviour
{

    List<float> turns = new List<float> { 0.0f, 360 / 8.0f, 2 * 360 / 8.0f, 3 * 360 / 8.0f, 4 * 360 / 8.0f,
        5 * 360 / 8.0f, 6 * 360 / 8.0f, 7 * 360 / 8.0f, };
    float goal;
    public float speed = 180.0f;
    private int goalIndex = 0;

    float turnAmount;

    // Use this for initialization
    void Start()
    {
        GoalIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.eulerAngles.z != goal)
        {
            changeRotation(transform.rotation.eulerAngles.z);
        }
    }

    private void changeRotation(float current)
    {
        turnAmount = goal - transform.rotation.eulerAngles.z * speed * Time.deltaTime;
        if (current < goal && current + turnAmount > goal)
        {
            current = goal;
        }
        else if (current > goal && current + turnAmount < goal)
        {
            current = goal;
        }
        else
        {
            current += turnAmount;
        }
    }

    public int GoalIndex
    {
        set
        {
            goalIndex = value;
            goal = turns[goalIndex];
        }
    }
}
