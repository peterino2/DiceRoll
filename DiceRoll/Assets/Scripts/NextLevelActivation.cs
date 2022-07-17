using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelActivation : MonoBehaviour, Activateable
{
    // Start is called before the first frame update
    public string nextLevelName = "L_one";
    private Dialogue dialogue;
    void Start()
    {
        var grid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        dialogue = FindObjectOfType<Dialogue>();
        if (grid)
        {
            grid.activateable = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SceneTransition()
    {
        var animTime = 0.0f;
        dialogue.DoFadeOut();
        while(animTime < 1.0f)
        {
            yield return null;
            animTime += Time.deltaTime;
        }
        SceneManager.LoadScene(nextLevelName);
    }

    public void Activate(DiceRoll player)
    {
        StartCoroutine(SceneTransition());
    }
}
