using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour {

    public Material sky;
    public Texture[] dayBreak, midday, evening, sunSet, midnight = new Texture[6];

    private Texture[][] skies = new Texture[5][];
    private int numberOfSkyTransitions;
    private string[] texNames = { "_FrontTex", "_BackTex", "_LeftTex", "_RightTex", "_UpTex", "_DownTex" };
    private float blend;
    // Use this for initialization
    void Start() {
        skies[0] = dayBreak;
        skies[1] = midday;
        skies[2] = evening;
        skies[3] = sunSet;
        skies[4] = midnight;

        numberOfSkyTransitions = 0;

        UpdateSkyMaterialTextures();

        blend = 0;

        InvokeRepeating("Lol", 0, 5);
    }

    // Update is called once per frame
    void Update() {

        blend = Mathf.SmoothStep(0, 1, Time.time);
        sky.SetFloat("_Blend", blend);
        Debug.Log(blend);
    }

    void Lol() {
        blend = 0;
    }

    void UpdateSkyMaterialTextures() {
        int index = numberOfSkyTransitions % 4;
        for (int i = 0; i < 6; i++) {
            sky.SetTexture(texNames[i], skies[index][i]);
            string secondTexNames = texNames[i] + "2";
            sky.SetTexture(secondTexNames[i], skies[index + 1][i]);
        }
        numberOfSkyTransitions++;
    }
}
