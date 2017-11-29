using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEagles : MonoBehaviour {

    public GameObject eaglePrefab;

    private Animator[] anims;

    private void Start() {
        int numberOfBirds = Random.Range(1, 3);
        anims = new Animator[numberOfBirds];
        for(int i = 0; i < numberOfBirds; i++) {
            float randomHeight = Random.Range(0, 2);
            GameObject tempEagle = Instantiate(eaglePrefab, new Vector3(transform.position.x, Random.Range(0, 2), transform.position.z), eaglePrefab.transform.rotation) as GameObject;
            anims[i] = tempEagle.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.CompareTag("Player")) {
            StartCoroutine("LaunchBirdAttack");
            GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator LaunchBirdAttack() {
        foreach(Animator anim in anims) {
            anim.SetTrigger("BirdAttack");
            yield return new WaitForSeconds(3f);
        }
    }

    /*
    public void ShuffleArray(int[] arr) {
        // Shuffles around the contents of the array using the Fisher Yates Shuffle.
        for(int i = arr.Length - 1; i > 0; i--) {
            int r = Random.Range(0, i);
            int tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }
    */
}
