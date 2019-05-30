using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField]
    private List<Text> counts;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SubscribeToInventory());
    }

    // Tries to subscribe to inventory delegate until it is found
    IEnumerator SubscribeToInventory()
    {
        while (true)
        {
            BWInventory inv = BWInventory.Instance;
            if(inv != null)
            {
                inv.InventoryUpdateCall += UpdateText;
                break;
            }
            yield return null;
        }
    }

    void UpdateText(Pickup pickup)
    {
        counts[(int)pickup - 1].text = BWInventory.Instance.GetPickupCount(pickup).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
