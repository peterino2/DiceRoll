using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    private bool canSkip = false;
    private bool inDialogue = false;
    private DiceRoll player;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _typeSpeed = 0.1f;
    [SerializeField] private Camera _camera;
    private void Start()
    {
       player = FindObjectOfType<DiceRoll>();
       _camera = FindObjectOfType<Camera>();
    }
     void Update()
     {
         if (!Input.GetKey(KeyCode.Space) && inDialogue)
         {
             canSkip = true;
         }
     }
     
     public void DialogShow(DiceRoll player, string dialog, bool endgame)
     {
         this.player = player; 

         if (!inDialogue)
             StartCoroutine(DisplayText(dialog, endgame));
     } 
     
     IEnumerator DisplayText(string dialog, bool endgame)
     {
         if (inDialogue)
             yield break;
             
         inDialogue = true;
         player.bInDialogue = true; 

         int i = 0;
         text.gameObject.SetActive(true);
         text.text = null;

         string str = dialog; // _dialogs[currStringSelection];

         while (i < str.Length)
         {
             if (Input.GetKey(KeyCode.Space) && i > 1 && canSkip)
             {
                 text.text = str;
                 break;
             }

             //text.text += originalText[i];
             text.text += str[i];
             _audioSource.Play();
             yield return new WaitForSeconds(_typeSpeed);
             ++i;
         }

         //currStringSelection++;
         //if (currStringSelection >= _dialogs.Length) { currStringSelection = _dialogs.Length - 1; }
         yield return new WaitForSeconds(0.3f);

         while (true)
         {
             yield return new WaitForSeconds(0.1f);
             if (Input.GetKey(KeyCode.Space) && canSkip) { break; }
         }

         text.gameObject.SetActive(false);
         inDialogue = false;
         canSkip = false;
         player.bInDialogue = false;
     }
    
}