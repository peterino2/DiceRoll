using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class L_six : MonoBehaviour
{
    public AudioClip newTrack;
    // Start is called before the first frame update
    void Start()
    {
        var x = FindObjectOfType<MusicPlayer>();
        if (newTrack == null)
            return;
        
        if (x != null)
        {
            x.SoftFadeToNewTrack(newTrack);
        }
    }

    // no time left i'm doing it here

    // Update is called once per frame
    void Update()
    {
        
    }
}
