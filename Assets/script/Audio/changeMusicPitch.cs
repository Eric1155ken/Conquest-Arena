using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMusicPitch : MonoBehaviour
{
    public float pitch = 0.5f;

    void Start(){
        AudioSource source = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource> ();
        source.pitch = pitch;
        source.Play();
    }
}
