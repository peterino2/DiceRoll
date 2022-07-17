using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour, Activateable
{
    // Start is called before the first frame update
    private GridLocationMarker grid;
    public GridLocationMarker target;
    
    void Start()
    {
        grid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        if (grid)
        {
            grid.activateable = this;
        }
    }

    public void Activate(DiceRoll player)
    {
        if (target != null)
        {
            player.TeleportToTarget(target);
        }
    }
}
