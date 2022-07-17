using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : MonoBehaviour, Activateable
{
    public bool isBroken = false;
    public GridLocationMarker marker;
    
    public void Start()
    {
        if (marker != null)
            marker.occupied = false;
        
        marker = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        if (marker != null)
        {
            marker.occupied = true;
            marker.activateable = this;
        }
        
    }

    public void BreakDown()
    {
        StartCoroutine(Break());
    }

    public float timescale = 1.0f;

    IEnumerator Break()
    {
        if (isBroken)
            yield break;

        isBroken = true;
        marker.occupied = false;
        float animTime = 0.0f;
        while (animTime < 1.0f)
        {
            transform.Rotate(new Vector3(0, 0,Time.deltaTime * timescale * 90.0f));
            yield return null;
            animTime += Time.deltaTime * timescale;
        }
    }

    public void Activate(DiceRoll player)
    {
        BreakDown();
    }
}
