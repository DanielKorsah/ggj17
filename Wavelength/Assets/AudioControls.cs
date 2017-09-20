using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControls : MonoBehaviour
{



    AudioSource musicLoop;

    void Start()
    {
        AudioSource[] sources;
        sources = GetComponents<AudioSource>();

        musicLoop = sources[0];

    }

    public void MusicMute()
    {
        musicLoop.mute = !musicLoop.mute;
    }
}
