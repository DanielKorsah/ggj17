using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModAudio : MonoBehaviour {

    [SerializeField]
    Sprite defaultIcon;

    Sprite currentIcon;
    bool alreadyPlayed;

	// Use this for initialization
	void Start () {
        currentIcon = GetComponent<Sprite>();
	}
	
	// Update is called once per frame
	void Update () {

        if(currentIcon != defaultIcon && !alreadyPlayed)
        {
            
        }
		
	}
}
