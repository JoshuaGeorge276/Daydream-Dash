using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlatform : Platform {

	[SerializeField]
    private string colour;

	// Use this for initialization
	new void Start () {
        base.Start();
        platformSpawner.SpawnObstacle(transform.position);
    }
	
	public string GetColour() {
        return colour;
    }
}
