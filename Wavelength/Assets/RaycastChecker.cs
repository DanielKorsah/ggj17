using UnityEngine;
using System.Collections;

public class Pathing : MonoBehaviour
{

    private int LayerGround;
    private bool CastRays = true;

    void Update()
    {
        if (CastRays)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                Debug.Log(hit.transform.name);
            }
            else
            {
                Debug.Log("FAIL");
            }
        }
    }
}
