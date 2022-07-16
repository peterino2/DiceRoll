using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGrid : MonoBehaviour
{
    public GameObject gridPositionPrefab;
    public int gridWidth;

    public GridLocationMarker[] grids;
    
    // Start is called before the first frame update
    void Start()
    {
        var grids = FindObjectsOfType<GridLocationMarker>();
        // now we build links between each grid.
        // we assume each grid is at set positions of 0, 1, 2, 3, 4 for x and z values
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
