using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScript : MonoBehaviour {

    public Material[] skyboxes;

    private float rotationPerSecond = 1;

    public void Awake() {
        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Length)];
    }

    // Update is called once per frame
    void Update () {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationPerSecond);
	}
}
