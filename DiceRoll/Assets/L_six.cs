using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_six : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var x  = FindObjectOfType<MusicPlayer>();
        if (x)
        {
            x.source.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
