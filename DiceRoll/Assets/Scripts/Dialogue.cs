using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI pressSpace;
    private bool canSkip = false;
    private bool inDialogue = false;
    private DiceRoll player;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _typeSpeed = 0.1f;
    [SerializeField] private Camera _camera;
    public string[] dialogueSet;
    public int currentDialogueIndex = 0;

    public AudioClip[] keyStrokeList;

    private void Awake()
    {
       player = FindObjectOfType<DiceRoll>();
       _camera = FindObjectOfType<Camera>();
       _audioSource = _camera.gameObject.GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
       text.gameObject.SetActive(false);
       pressSpace.gameObject.SetActive(false);
    }
     void Update()
     {
         if (!Input.GetKey(KeyCode.Space) && inDialogue)
         {
             canSkip = true;
         }
     }
     
     public void DialogShow(string[] newDialogSet)
     {
         if (!inDialogue)
         {
             dialogueSet = newDialogSet;
             StartCoroutine(DisplayText());
         }
     } 
     
     IEnumerator DisplayText()
     {
         if (inDialogue)
             yield break;
             
         inDialogue = true;
         player.bInDialogue = true; 

         text.gameObject.SetActive(true);
         text.text = null;

         currentDialogueIndex = 0;
         string str = dialogueSet[0]; // _dialogs[currStringSelection];
         int lastaudioId = 0;
         while (currentDialogueIndex < dialogueSet.Length)
         {
             
             str = dialogueSet[currentDialogueIndex];
             pressSpace.gameObject.SetActive(false);
             int i = 0;
            
             text.text = "";
             
             while (i < str.Length)
             {
                 if (Input.GetKey(KeyCode.Space) && i > 1 && canSkip)
                 {
                     text.text = str;
                     break;
                 }

                 //text.text += originalText[i];
                 text.text += str[i];
                 int rand = Random.Range(0, keyStrokeList.Length - 1);
                 if (rand == lastaudioId)
                 {
                     rand = rand + 1 % keyStrokeList.Length;
                 }

                 if (i % 2 == 0)
                 {
                     lastaudioId = rand;
                     _audioSource.clip = keyStrokeList[rand];
                     _audioSource.Play();
                 }
                 yield return new WaitForSeconds(_typeSpeed);
                 ++i;
             }

             pressSpace.gameObject.SetActive(true);
             yield return new WaitForSeconds(0.4f);
             
             var timeout = 2.0f;
             currentDialogueIndex += 1;
             while (true)
             {
                 yield return null;
                 timeout -= Time.deltaTime;
                 if (( (timeout < 0 && currentDialogueIndex < dialogueSet.Length ) || Input.GetKey(KeyCode.Space) ) && canSkip) { break; }
             }

             yield return new WaitForSeconds(0.1f);
             //currStringSelection++;
             //if (currStringSelection >= _dialogs.Length) { currStringSelection = _dialogs.Length - 1; }
         }


         text.gameObject.SetActive(false);
         pressSpace.gameObject.SetActive(false);
         inDialogue = false;
         canSkip = false;
         player.bInDialogue = false;
     }
    
}