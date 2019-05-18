using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioClip backgroundClip;
    public AudioSource backgroundClipSource;

    // Start is called before the first frame update
    void Start()
    {
        backgroundClipSource.clip = backgroundClip;
        backgroundClipSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
