using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, Activateable
{
    private GridLocationMarker grid;
    
    public string[] dialogueSet;
    public bool dialogueShown = false;
    public bool bTriggerOnce = true;
    public bool stopMusic = false;

    // Start is called before the first frame update
    void Start()
    {
        grid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        if (grid)
        {
            grid.activateable = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(DiceRoll _player)
    {
        if (dialogueShown && bTriggerOnce)
            return;
        FindObjectOfType<Dialogue>().DialogShow(dialogueSet);
        dialogueShown = true;
        if (stopMusic)
        {
            var music = FindObjectOfType<MusicPlayer>();
            if (music != null)
            {
                music.source.Stop();
            }
        }
    }
}
