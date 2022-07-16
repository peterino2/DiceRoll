using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, Activateable
{
    public Obstacle activationTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        var gridLocationMarker = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        if (gridLocationMarker)
        {
            gridLocationMarker.LinkActivateable(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if (activationTarget != null)
        {
            activationTarget.Activate();
        }
    }
}
