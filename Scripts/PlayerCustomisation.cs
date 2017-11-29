using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomisation : MonoBehaviour {

    public MeshRenderer[] meshes;
    public Transform hatParent;
    public GameObject hairMale, hairFemale;

    private void Awake() {
        UpdateGender();
        UpdateCustomisation();
    }

    private void UpdateGender() {
        if (PlayerPrefs.GetString("GENDER").Equals("M")) {
            if (!hairMale.activeInHierarchy) {
                hairFemale.SetActive(false);
                hairMale.SetActive(true);
                meshes[0] = hairMale.GetComponent<MeshRenderer>();
            }
        } else {
            if (!hairFemale.activeInHierarchy) {
                hairMale.SetActive(false);
                hairFemale.SetActive(true);
                meshes[0] = hairFemale.GetComponent<MeshRenderer>();
            }
        }
    }

    public void UpdateCustomisation() {
        Material[] hairMats = GetMaterialsFromObjects(Resources.LoadAll<GameObject>("Shop/Hair")); // Loading all resources from Hair folder.
        Material[] skinMats = GetMaterialsFromObjects(Resources.LoadAll<GameObject>("Shop/Skin")); // Loading all resources from Skin folder.
        Material[] bodyMats = GetMaterialsFromObjects(Resources.LoadAll<GameObject>("Shop/Body")); // Loading all resources from Body folder.
        GameObject[] hats = Resources.LoadAll<GameObject>("Shop/Hat");  // Loading all resources from Hats folder.
        Material skinMat = skinMats[PlayerPrefs.GetInt("FEATURE_2")];   // Finding correct material for skin as this will be references multiple times.

        Destroy(hatParent.GetChild(0).gameObject); // Destroying old hat.
        if (hatParent.childCount > 1) {             // Making sure that the player only has 1 hat equiped.
            Destroy(hatParent.GetChild(1).gameObject);
        }
        Instantiate(hats[PlayerPrefs.GetInt("FEATURE_0")], hatParent);  // Instantiating the new Hat.

        meshes[0].material = hairMats[PlayerPrefs.GetInt("FEATURE_1")]; // Setting the hair material to the players hair.
        
        // Setting the skin material to the head and both hands.
        meshes[1].material = skinMat;      
        meshes[2].material = skinMat;
        meshes[3].material = skinMat;

        meshes[4].material = bodyMats[PlayerPrefs.GetInt("FEATURE_3")]; // Setting the body material to the players body.
    }

    private Material[] GetMaterialsFromObjects(GameObject[] array) {
        int i = 0;
        Material[] temp = new Material[array.Length];
        foreach(GameObject mat in array) {
            temp[i] = mat.GetComponent<MeshRenderer>().sharedMaterial;
            i++;
        }
        return temp;
    }
}
