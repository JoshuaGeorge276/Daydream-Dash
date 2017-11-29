using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationManager : MonoBehaviour {
	
	public List<Feature> features;
	public int currFeature;

	private PlayerModel playerModel;
    private AudioSource audioSource;

	void OnEnable(){
		playerModel = GetComponent<PlayerModel> ();
        audioSource = GetComponent<AudioSource>();
		playerModel.FindHat ();
		playerModel.Init ();
		LoadFeatures ();
	}

	void OnDisable(){
        PlayerPrefs.Save(); 
	}

	void LoadFeatures(){
		features = new List<Feature> ();
		features.Add (new ObjectFeature ("Hat", playerModel));
		features.Add (new MaterialFeature ("Hair", new MeshRenderer[]{ playerModel.meshes[0] }));
		features.Add (new MaterialFeature ("Skin", new MeshRenderer[]{ playerModel.meshes [1], playerModel.meshes[2], playerModel.meshes[3]}));
		features.Add (new MaterialFeature ("Body", new MeshRenderer[]{ playerModel.meshes[4]}));

		for (int i = 0; i < features.Count; i++) {
			string key = "FEATURE_" + i;
			if (!PlayerPrefs.HasKey (key)) {
				PlayerPrefs.SetInt (key, features [i].currIndex);
			}
			features [i].currIndex = PlayerPrefs.GetInt (key);
			features [i].UpdateFeature ();
		}
	}

    public void EquipItem() {
        string key = "FEATURE_" + features.IndexOf(features[currFeature]);
        PlayerPrefs.SetInt(key, features[currFeature].currIndex);
    }

    public void BuyItem() {
        NewItem item = features[currFeature].ReturnItem();
        string key = item.GetName();
        PlayerPrefs.SetInt(key, 1);
        int totalOrbs = PlayerPrefs.GetInt("ORBS");
        PlayerPrefs.SetInt("ORBS", totalOrbs - item.GetCost());
    }

	public void SetCurrent(int index){
		if (features == null) {
			return;
		}
		currFeature = index;
	}

	public void NextChoice(){
		if (features == null) {
			return;
		}

		features [currFeature].currIndex++;
		features [currFeature].UpdateFeature ();
	}

	public void PreviousChoice(){
		if (features == null) {
			return;
		}

		features [currFeature].currIndex--;
		features [currFeature].UpdateFeature ();
	}

    public void ToggleGender(int genderNum) {
        playerModel.ToggleGender(genderNum);
        features[1] = new MaterialFeature("Hair", new MeshRenderer[] { playerModel.meshes[0] });
        if(genderNum == 0) {
            PlayerPrefs.SetString("GENDER", "M");
        } else {
            PlayerPrefs.SetString("GENDER", "F");
        }
    }
}

[System.Serializable]
public abstract class Feature{
	public string id;
	public int currIndex;
    public GameObject[] choices;

	public Feature(string id){
		this.id = id;
	}

	public virtual void UpdateFeature (){
        choices = Resources.LoadAll<GameObject>("Shop/" + id);

        if (choices == null) {
            return;
        }

        if (currIndex < 0) {
            currIndex = choices.Length - 1;
        }

        if (currIndex >= choices.Length) {
            currIndex = 0;
        }
    }

    public NewItem ReturnItem() {
        return choices[currIndex].GetComponent<NewItem>();
    }
}

[System.Serializable]
public class MaterialFeature : Feature {
    public Material[] mats;
    public MeshRenderer[] meshRenderer;

    public MaterialFeature(string id, MeshRenderer[] meshRenderer) : base(id) {
        this.meshRenderer = meshRenderer;
        UpdateFeature();
    }

    public override void UpdateFeature() {
        base.UpdateFeature();

        mats = new Material[choices.Length];

        for(int i = 0; i < choices.Length; i++) {
            mats[i] = choices[i].GetComponent<MeshRenderer>().sharedMaterial;
        }

        foreach (MeshRenderer mesh in meshRenderer) {
            mesh.material = mats[currIndex];
        }
    }
}

[System.Serializable]
public class ObjectFeature : Feature {
    public PlayerModel player;

    public ObjectFeature(string id, PlayerModel player) : base(id) {
        this.player = player;
        UpdateFeature();
    }

    public override void UpdateFeature() {
        base.UpdateFeature();
        player.SetObject(choices[currIndex]);
    }
}
