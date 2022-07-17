using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashPickup : MonoBehaviour, Activateable
{
    // Start is called before the first frame update
    private GridLocationMarker grid;
    public string abilityToGrant;
    void Start()
    {
        grid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
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
        player.Abilities.Add(abilityToGrant);
        grid.activateable = null;
        Destroy(gameObject);
    }
}
