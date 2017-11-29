using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerModel : MonoBehaviour {

	[HideInInspector]
	public MeshRenderer[] meshes;
    public GameObject hairMale, hairFemale;

	private Transform hatParent;

    private string gender;

    // Update is called once per frame
    void Update () {
		transform.Rotate (Vector3.up * 50 * Time.deltaTime, Space.Self);
	}

	public void Init(){
        meshes = new MeshRenderer[5];

        gender = PlayerPrefs.GetString("GENDER");
        if (gender.Equals("M")) {
            ToggleGender(0);
        } else {
            ToggleGender(1);
        }
		meshes [1] = transform.Find ("Head_Section").Find ("Head").GetComponent<MeshRenderer> ();
		meshes [2] = transform.Find ("Body_Section").Find ("Hands").Find ("Left_Hand").GetComponent<MeshRenderer> ();
		meshes [3] = transform.Find ("Body_Section").Find ("Hands").Find ("Right_Hand").GetComponent<MeshRenderer> ();
		meshes [4] = transform.Find ("Body_Section").Find ("Body").GetComponent<MeshRenderer> ();
	}

	public void FindHat(){
		hatParent = transform.Find ("Head_Section").Find ("Hat").transform;
	}

	public void SetObject(GameObject newObject){
		Destroy (hatParent.GetChild (0).gameObject);
        if(hatParent.childCount > 1) {
            Destroy(hatParent.GetChild(1).gameObject);
        }
		Instantiate (newObject, hatParent);
	}

    public void ToggleGender(int genderNum) {
        if (genderNum == 1) {
            hairMale.SetActive(false);
            hairFemale.SetActive(true);
            gender = "F";
            meshes[0] = transform.Find("Head_Section").Find("Hair_Default_" + gender).GetComponent<MeshRenderer>();
        } else {
            hairFemale.SetActive(false);
            hairMale.SetActive(true);
            gender = "M";
            meshes[0] = transform.Find("Head_Section").Find("Hair_Default_" + gender).GetComponent<MeshRenderer>();
        }
    }
}
