using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitterAudio : MonoBehaviour {

    [SerializeField]
    List<AudioClip> tones;
    Transmitter t;
    int state;
    int lastState;
    AudioSource source;
    
	// Use this for initialization
	void Start () {
        t = GetComponent<Transmitter>();
        source = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
        state = t.i;
        if(state!=lastState)
        {
            source.clip = tones[state];  
            Debug.Log("State: " + state);
            Debug.Log("tone: " + tones[state]);
            source.Play();  //lets just call this cancer a load level sound the first time it plays
            lastState = t.i;
        }
              
    }

    
}
