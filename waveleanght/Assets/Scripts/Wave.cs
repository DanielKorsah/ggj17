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

    private float amplitude = 0.5f;
    public float frequency = 2.0f;

    private float offset = 0.0f;
    private float lastPies;
    private float lastFrq;

    // Use this for initialization
    void Start()
    {
        height = (up.transform.position.y - down.transform.position.y) / 2;
        yzero = up.transform.position.y - height;
        old = Time.fixedTime;
        screenSpeed = (right.transform.position.x - left.transform.position.x) / 80.0f;

        lastFrq = frequency;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Time.time - old > 0.01f)
        {
            if (lastFrq != frequency)
            {
                offset = lastPies - Mathf.PI * Time.fixedTime * frequency;
            }

            dots.Add(Instantiate(dot, new Vector3(right.transform.position.x, yzero + Mathf.Sin(Mathf.PI * Time.fixedTime * frequency + offset) * height * amplitude, 0.0f), new Quaternion()));
            dots[dots.Count - 1].transform.SetParent(parent.transform);
            moveDots();
            old = Time.fixedTime;

            lastFrq = frequency;
            lastPies = Mathf.PI * Time.fixedTime * frequency + offset;
            while (lastPies > Mathf.PI * 2)
                lastPies -= Mathf.PI * 2;
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
