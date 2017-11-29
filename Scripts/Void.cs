using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour {

    private GameObject player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0, -10, 0);
        transform.position = player.transform.position + offset;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
	}
}
