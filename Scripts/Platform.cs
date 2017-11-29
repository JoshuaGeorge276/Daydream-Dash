using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [HideInInspector]
    protected PlatformSpawner platformSpawner;
    private float secondsTillDestroy = 10;

    protected void Start() {
        platformSpawner = FindObjectOfType<PlatformSpawner>();
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            platformSpawner.SpawnNextPlatform(transform.position);
            Destroy(gameObject, secondsTillDestroy);
        }
    }
}
