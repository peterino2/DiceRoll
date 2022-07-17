using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource source;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoftFadeToNewTrack(AudioClip clip)
    {
        StartCoroutine(SoftFadeToNewTrackCoro(clip));
    }
    
    IEnumerator SoftFadeToNewTrackCoro(AudioClip clip)
    {
        float volume = 1.0f;
        while (volume > 0.1)
        {
            source.volume -= Time.deltaTime / 0.5f;
            volume -= Time.deltaTime / 0.5f;
            yield return null;
        }
        
        source.Stop();
        source.clip = clip;
        source.Play();

        volume = 0.0f;
        while (volume < 1.0f)
        {
            source.volume += Time.deltaTime / 0.5f;
            volume += Time.deltaTime / 0.5f;
            yield return null;
        }
        source.volume = 1.0f;
    }
}
