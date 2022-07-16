using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, Activateable
{
    public Obstacle activationTarget;

    public bool ChecksFace = true;

    public int FaceNeeded = 1;
    
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

    public void Activate(DiceRoll player)
    {
        if (activationTarget != null)
        {
            if (ChecksFace)
            {
                if (player.checkRotationValue() != FaceNeeded)
                    return;
            }
            
            activationTarget.Activate(player);
        }
    }
}
