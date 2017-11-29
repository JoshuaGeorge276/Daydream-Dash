using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    private Player player;

    // For platforms
    public GameObject[] platforms;
    private Vector3 platformSpace = new Vector3(0, -1.5f, 150f);
    
    // For obstacles
    public GameObject[] fullObstacles;
    public GameObject[] halfObstacles;
    public GameObject negativePickup;
    private Vector3 obstacleHeigtOffset = new Vector3(0, 1.5f, 0);
    private Vector3 halfObstacleOffset = new Vector3(0, 0, 10);
    private GameObject chosenObstacle, firstHalf, secondHalf;
    

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    public void SpawnNextPlatform(Vector3 position) {
        int randomIndex;
        if (player.GetDistance() < 300) {
            randomIndex = Random.Range(0, platforms.Length - 1);
        } else {
            randomIndex = Random.Range(0, platforms.Length);
        }

        Instantiate(platforms[randomIndex], new Vector3(0, platformSpace.y, (position.z + platformSpace.z)), Quaternion.identity);
    }

    public void SpawnObstacle(Vector3 position) {
        // If Calling destroy from this method doesn't work then return the objects to the platform script that called this method and let them destroy it on trigger enter.
        if(player.GetDistance() < 300) {
            chosenObstacle = InstantiateObstacle(fullObstacles, position, false);
            Destroy(chosenObstacle, 30);
        } else {
            float fullOrHalfObstacle = Random.Range(0, 2);
            if(fullOrHalfObstacle < 0.5f) {
                chosenObstacle = InstantiateObstacle(fullObstacles, position, true);
                Destroy(chosenObstacle, 30);
            } else {
                firstHalf = InstantiateObstacle(halfObstacles, position - halfObstacleOffset, false);
                secondHalf = InstantiateObstacle(halfObstacles, position + halfObstacleOffset, false);
                Destroy(firstHalf, 30);
                Destroy(secondHalf, 30);
            }
        }
    }

    private GameObject InstantiateObstacle(GameObject[] objectToSpawn, Vector3 position, bool hard) {
        int obstacleIndex = 0;
        if (hard) {
            // If the player has reached a certain distance hard should be true which means that the orb-only obstacles will no longer be part of the pool.
            obstacleIndex = Random.Range(2, objectToSpawn.Length);
        } else {
            obstacleIndex = Random.Range(0, objectToSpawn.Length - 1);
        }
        GameObject temp = Instantiate(objectToSpawn[obstacleIndex], position + obstacleHeigtOffset, Quaternion.identity) as GameObject;

        if (hard) {
            Pickup[] pickupPositions = temp.GetComponentsInChildren<Pickup>();
            int randomPickupIndex = Random.Range(0, pickupPositions.Length);
            Instantiate(negativePickup, pickupPositions[randomPickupIndex].gameObject.transform.position, Quaternion.identity);
            Destroy(pickupPositions[randomPickupIndex].gameObject);
        }
        return temp;
    }
}
