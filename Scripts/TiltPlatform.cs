using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltPlatform : Platform {

    private float rollAngle;
    private UIManager uiManager;
    private bool triggeredAnimation;

    // Use this for initialization
    new void Start () {
        base.Start();
        uiManager = FindObjectOfType<UIManager>();
        float randomYHeight = Random.Range(-3, 4);
        transform.position = new Vector3(transform.position.x, randomYHeight, transform.position.z);
        uiManager.TriggerTiltIcon();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float angle = Mathf.Atan2(Input.acceleration.x, Input.acceleration.y) * Mathf.Rad2Deg;
        rollAngle = Mathf.LerpAngle(rollAngle, -angle, 2 * Time.deltaTime);

        transform.localRotation = Quaternion.AngleAxis(rollAngle, new Vector3(1, 0, 0));
    }

    new void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (other.transform.root.CompareTag("Player")) {
            other.transform.root.GetComponent<Player>().SetTiltMode(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.root.CompareTag("Player")) {
            other.transform.root.GetComponent<Player>().SetTiltMode(false);
        }
    }
  
}
