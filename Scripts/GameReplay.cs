using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReplay : MonoBehaviour {

    private const int bufferFrames = 200;
    private ReplayKeyFrame[] keyFrames = new ReplayKeyFrame[bufferFrames];
    private Rigidbody rigidBody;
    private GameManager gameManager;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        Vector3 startPos = transform.position;
        for(int i = 0; i < keyFrames.Length; i++) {
            keyFrames[i] = new ReplayKeyFrame(0, startPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update() {
		/*
        if (gameManager.recording) {
            Record();
        } else {
            Playback();
        }
        */
    }
    public void Playback() {
        rigidBody.isKinematic = false;
        int frame = Time.frameCount % bufferFrames;
        transform.position = keyFrames[frame].position;
        transform.rotation = keyFrames[frame].rotation;
    }

    private void Record() {
        rigidBody.isKinematic = false;
        int frame = Time.frameCount % bufferFrames;
        float time = Time.time;
        keyFrames[frame] = new ReplayKeyFrame(time, transform.position, transform.rotation);
    }
}

/// <summary>
/// A structure for storing time, position and rotation.
/// </summary>
public struct ReplayKeyFrame {
    public float frameTime;
    public Vector3 position;
    public Quaternion rotation;

    public ReplayKeyFrame(float time, Vector3 pos, Quaternion rot) {
        frameTime = time;
        position = pos;
        rotation = rot;
    }
}
