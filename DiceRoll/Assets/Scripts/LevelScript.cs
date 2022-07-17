using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
[Serializable]
public struct LevelDialogue
{
    public string[] row;
}
public class LevelScript : MonoBehaviour
{
    public string[] dialogueOnLevelStart;
    public LevelDialogue[] extraDialoguesByIndex;
    public Dialogue dialogueSystem;
    public DiceRoll player;
    
    // Start is called before the first frame update
    private void Awake()
    {
        player = FindObjectOfType<DiceRoll>();
        dialogueSystem = FindObjectOfType<Dialogue>();
    }

    void Start()
    {
        StartCoroutine(StartlevelDialogue());
    }

    public void TriggerExtraDialogue(int id)
    {
        if (id < extraDialoguesByIndex.Length)
        {
            dialogueSystem.DialogShow(extraDialoguesByIndex[id].row);
        }
    }
    
    IEnumerator StartlevelDialogue()
    {
        yield return new WaitForSeconds(0.3f);
        if (dialogueOnLevelStart.Length > 0)
        {
            dialogueSystem.DialogShow(dialogueOnLevelStart);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
