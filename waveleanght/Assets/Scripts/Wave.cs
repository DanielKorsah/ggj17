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
    public int frqc = 0;

    private float offset = 0.0f;
    private float lastPies;
    private float lastFrq;

    private float ir = 2f;
    private float v = 3.5f;
    private float uv = 5.2f;

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

            float sinPos = 0;

            switch (frqc)
            {
                case 0:
                    sinPos = 0;
                    frequency = 0;
                    break;
                case 1:
                    sinPos = Mathf.Sin(Mathf.PI * Time.fixedTime * ir + offset);
                    frequency = ir;
                    break;
                case 2:
                    sinPos = Mathf.Sin(Mathf.PI * Time.fixedTime * v + offset);
                    frequency = v;
                    break;
                case 3:
                    sinPos = Mathf.Sin(Mathf.PI * Time.fixedTime * uv + offset);
                    frequency = uv;
                    break;
                case 4:
                    sinPos = Mathf.Sin(Mathf.PI * Time.fixedTime * ir + offset) + Mathf.Sin(Mathf.PI * Time.fixedTime * v + offset);
                    frequency = ir + v;
                    break;
                case 5:
                    sinPos = Mathf.Sin(Mathf.PI * Time.fixedTime * ir + offset) + Mathf.Sin(Mathf.PI * Time.fixedTime * uv + offset);
                    frequency = ir + uv;
                    break;
                case 6:
                    sinPos = Mathf.Sin(Mathf.PI * Time.fixedTime * v + offset) + Mathf.Sin(Mathf.PI * Time.fixedTime * uv + offset);
                    frequency = v + uv;
                    break;
                case 7:
                    sinPos = (Mathf.Sin(Mathf.PI * Time.fixedTime * ir + offset) + Mathf.Sin(Mathf.PI * Time.fixedTime * v + offset) + Mathf.Sin(Mathf.PI * Time.fixedTime * uv + offset)) * 0.6f;
                    frequency = ir + v +uv;
                    break;
            }
            Debug.Log(frequency);

            dots.Add(Instantiate(dot, new Vector3(right.transform.position.x, yzero + sinPos * height * amplitude, 0.0f), new Quaternion()));
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
