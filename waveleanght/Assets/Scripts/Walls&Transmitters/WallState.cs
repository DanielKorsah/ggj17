using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallState : MonoBehaviour {

    [SerializeField]
    private string wallType;

    [SerializeField]
    public Vector2 gridLocation;
    

    public List<string> freqs = new List<string>();

    private void Start()
    {
        
    }
    
	
	// Update is called once per frame
	public void StateUpdate(string transmitterState)
    {
        freqs.Add(transmitterState);
        if (freqs.Contains(wallType))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }



    public void StateCull(string transmitterNotState)
    {
        foreach (string t in freqs)
            if (t.Equals(transmitterNotState))
            {
                freqs.Remove(t);
                break;
            }
    }
}
