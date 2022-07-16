using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Activateable
{
    void Activate();
}


[ExecuteInEditMode]
public class GridLocationMarker : MonoBehaviour
{
    public DiceGrid masterGrid;

    public bool occupied;
    public int trueIndex = 0;
    public GridLocationMarker[] links = new GridLocationMarker[4];
    public Vector3 cachedStartPosition;

    public Activateable activateable;

    void Start()
    {
        cachedStartPosition = Vector3.zero;
        masterGrid = FindObjectOfType<DiceGrid>();
        buildLinksOnGrid();
    }

    public void LinkActivateable(Activateable _activateable)
    {
        activateable = _activateable;
    }

    public void OnSteppedOn()
    {
        if (activateable != null)
        {
            activateable.Activate();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Application.IsPlaying(gameObject))
        {
            
        }
        else
        {
            buildLinksOnGrid();
        }
    }

    public static GridLocationMarker GetGridLocationMarkerAtLocation(Vector3 location)
    {
        Ray checkRay = new Ray(location + new Vector3(0, 3, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(checkRay.origin, checkRay.direction, out hit, Mathf.Infinity, 1 << 6))
        {
            var gridMarker = hit.transform.root.gameObject.GetComponent<GridLocationMarker>();
            return gridMarker;
        }

        return null;
    }

    private Vector3[] checkOffset =
    {
        new Vector3(0, 0, -1), // right
        new Vector3(-1, 0, 0), // down
        new Vector3(0, 0, 1), // left
        new Vector3(1, 0, 0), // up
    };
    
    void buildLinksOnGrid()
    {
        var startingPosition = gameObject.transform.position;
        if (cachedStartPosition == startingPosition)
            return;
        
        cachedStartPosition = startingPosition;
        for (int i = 0; i < 4; i += 1)
        {
            Ray checkRay = new Ray(startingPosition + checkOffset[i] + new Vector3(0, 3, 0), Vector3.down * 1000);
            RaycastHit hit;
            
            Debug.DrawRay(checkRay.origin, checkRay.direction*1000, Color.cyan);
            
            if (Physics.Raycast(checkRay.origin, checkRay.direction, out hit, Mathf.Infinity, 1<< 6))
            {
                var gridMarker = hit.transform.root.gameObject.GetComponent<GridLocationMarker>();
                Debug.Log("hit" + hit.transform.root.gameObject.name);
                if (gridMarker != null)
                {
                    if (links.Length > i)
                    {
                        links[i] = gridMarker;
                    }
                }
            }
        }
    }
}
