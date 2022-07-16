using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelActivation : MonoBehaviour, Activateable
{
    // Start is called before the first frame update
    public string nextLevelName = "L_one";
    void Start()
    {
        var grid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        if (grid)
        {
            grid.activateable = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(DiceRoll player)
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
