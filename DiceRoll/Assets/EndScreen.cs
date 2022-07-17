using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI text;
    private MusicPlayer music;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeInCoro());
        music = FindObjectOfType<MusicPlayer>();
    }

    IEnumerator fadeInCoro()
    {
        float animTime = 0;
        while (animTime < 2.5f)
        {
            text.color = new Color(1,1,1, animTime/2.5f);
            if (music != null)
            {
                music.source.volume -= (Time.deltaTime/2.5f);
            }
            yield return null;
            animTime += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
