using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pickup : MonoBehaviour {

    [SerializeField]
    private int pickupScore;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.CompareTag("Player")) {
            other.transform.root.GetComponent<Player>().CollectedPickup(pickupScore);
            SoundManager.PlayPickupSound();
            Destroy(gameObject);
        }
    }
}
