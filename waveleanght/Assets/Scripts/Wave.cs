using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{


    [SerializeField]
    GameObject dot;

    List<GameObject> dots = new List<GameObject>();

    [SerializeField]
    GameObject up;
    [SerializeField]
    GameObject down;
    [SerializeField]
    GameObject left;
    [SerializeField]
    GameObject right;

    [SerializeField]
    GameObject parent;

    private float height;
    private float yzero;
    public float screenSpeed;

    float old;

    private float amplitude = 1.0f;
    public float frequency = 2.0f;

    // Use this for initialization
    void Start()
    {
        height = (up.transform.position.y - down.transform.position.y) / 2;
        yzero = up.transform.position.y - height;
        old = Time.time;
        screenSpeed = (right.transform.position.x - left.transform.position.x) / 70.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - old > 0.03f)
        {
            dots.Add(Instantiate(dot, new Vector3(right.transform.position.x, yzero + Mathf.Sin(Mathf.PI * Time.time * frequency) * height * amplitude, 0.0f), new Quaternion()));
            dots[dots.Count - 1].transform.parent = parent.transform;
            moveDots();
            old = Time.time;
        }
    }
    

    private void moveDots()
    {
        for (int i = 0; i < dots.Count; ++i)
        {
            dots[i].transform.position -= new Vector3(screenSpeed, 0.0f, 0.0f);
        }
        if (dots[0].transform.position.x < left.transform.position.x)
        {
            Destroy(dots[0]);
            dots.Remove(dots[0]);
        }
    }
    
}
