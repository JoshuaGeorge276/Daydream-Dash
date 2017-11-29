using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject deathParticles;
	public AudioClip jumpClip;
    public float minSwipeDist, maxSwipeTime; bool couldBeSwipe;
    private Vector2 startPos;
    private float swipeStartTime;


    [SerializeField] private float movePower; // The initial speed to be used for the player when it moves.
    [SerializeField] private float jumpPower; // The force added to the player when it jumps.

	private const float GroundRayLength = 0.6f; // The length of the ray to check if the player is grounded.

    private bool jump; // whether the jump button is currently pressed

    private bool inAir, inTiltMode, inDeathPhase;
    private int layerMask;
    private float startPosition, distance;
    private int score, colourIndex;
    private PlayerColour currentColour;
    private PlayerColour[] playerColours;
    private Rigidbody rb;
    private BasicPlatform currentPlatform;
    private Animator anim;
	private ParticleSystem trail;
	private AudioSource audioSource;
    private Transform hatParent;
	private UIManager uiManager;
	private Color blueColour = new Color(0.36f, 0.60f, 1f);
	private Color greenColour = new Color(0.55f, 0.96f, 0.26f);
	private Color redColour = new Color(0.96f, 0.31f, 0.26f);

    //Touch Controls
    private Vector3 startDrag, endDrag;

    public enum PlayerColour{
        // Represents the colour state that the player can be.
        Blue, Green, Red
    };

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.SetFloat("Speed", movePower);
        anim.SetBool("isRunning", false);
        playerColours = new PlayerColour[]{ PlayerColour.Blue, PlayerColour.Green, PlayerColour.Red };
        colourIndex = 0;
		trail = GetComponentInChildren<ParticleSystem> ();
		currentColour = PlayerColour.Blue;
        hatParent = transform.Find("Head_Section").Find("Hat").transform;
        
		layerMask = LayerMask.GetMask ("Platform");
        startPosition = transform.position.z + 30f; // Magic number used here is the distance the player is from the 0 z position in Global Position.

        StartCoroutine("CheckVerticalSwipes");

        InvokeRepeating("AdjustSpeed", 0, 50);

        /*
        // Google VR
        CardboardMagnetSensor.SetEnabled(true);
        //Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        */
    }

    // Update is called once per frame
    private void Update() {
		if (!GameManager.gameStarted) {
            return;
        }

        /*
        if (Input.GetMouseButtonDown(0) || CrossPlatformInputManager.GetButtonDown("Jump")) {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.D) || CrossPlatformInputManager.GetButtonDown("Swap Colour")) {
            NextColour();
        }

        if(CrossPlatformInputManager.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.S)) {
            anim.SetTrigger("Slide");
        }
        */
        distance = Mathf.RoundToInt(startPosition + transform.position.z);

		if (uiManager) {
			uiManager.UpdateDistanceTravelledText(distance);
		}
       

        /*
        // Google VR
        jump = CardboardMagnetSensor.CheckIfWasClicked();
        */
    }

    void FixedUpdate () {
		if (GameManager.gameStarted) {
            anim.SetBool("isRunning", true);
            //rb.AddForce(new Vector3(move.z, 0, 0) * m_MovePower);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * anim.GetFloat("Speed"));

			if (Physics.Raycast(transform.position, -Vector3.up, GroundRayLength + 0.1f, layerMask) && jump) {
				audioSource.PlayOneShot (jumpClip);
                anim.SetTrigger("Jump");
                // ... add force in upwards.
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                jump = false;

                // Google VR
                //CardboardMagnetSensor.ResetClick();
            }

			//Debug.Log (Physics.Raycast (transform.position, -Vector3.up, GroundRayLength + 0.1f));

            if(!Physics.Raycast(transform.position, -Vector3.up, GroundRayLength)){        
                inAir = true;
				if (transform.position.y < -0.5f && !inTiltMode) { // Condition to check if player is below platform level and cannot recover.
					anim.SetTrigger ("isFalling");
					trail.Stop ();
				}
            } else {
                inAir = false;
            }

        }
    }

    public void NextColour() {
        colourIndex++;
        currentColour = playerColours[colourIndex % playerColours.Length];
		var main = trail.main;
        switch (currentColour) {
		case PlayerColour.Blue:
			SetHatColourToAllRenderers(blueColour);
			main.startColor = blueColour;
			uiManager.UpdateColourIndicator (greenColour, redColour);
                break;
		case PlayerColour.Green:
                SetHatColourToAllRenderers(greenColour);
                main.startColor = greenColour;
			uiManager.UpdateColourIndicator (redColour, blueColour);
                break;
		case PlayerColour.Red:
                SetHatColourToAllRenderers(redColour);
                main.startColor = redColour;
			uiManager.UpdateColourIndicator (blueColour, greenColour);
                break;
        }

        if (!inAir && currentPlatform) {
            CheckColourMatch(currentPlatform);
        }
    }

    public void ChangeColourNoCheck() {
        colourIndex++;
        currentColour = playerColours[colourIndex % playerColours.Length];
        switch (currentColour) {
            case PlayerColour.Blue:
                SetHatColourToAllRenderers(blueColour);
                uiManager.UpdateColourIndicator(greenColour, redColour);
                break;
            case PlayerColour.Green:
                SetHatColourToAllRenderers(greenColour);
                uiManager.UpdateColourIndicator(redColour, blueColour);
                break;
            case PlayerColour.Red:
                SetHatColourToAllRenderers(redColour);
                uiManager.UpdateColourIndicator(blueColour, greenColour);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.GetComponent<BasicPlatform>()) {
            currentPlatform = collision.gameObject.GetComponent<BasicPlatform>();
            CheckColourMatch(currentPlatform);
        }

        if (collision.gameObject.CompareTag("Void")) {
            Death();
        }
    }

    private void CheckColourMatch(BasicPlatform platform) {
        if ((platform.GetColour()) != currentColour.ToString()) {
            Death();
        }
    }

	private Color GetPlatformColour (string colourString){
		// Currently unused but may be used to get the colour of the current basic platform the player is running on.
		Debug.Log (colourString);
		if (colourString.Equals ("Blue")) {
			return blueColour;
		} else if (colourString.Equals ("Green")) {
			return greenColour;
		} else if (colourString.Equals ("Red")) {
			return redColour;
		} else
			return Color.black;
	}

    void Death() {
        if (!inDeathPhase) {
            inDeathPhase = true;
            SoundManager.PlayDeathSound();
            LoseMenu.Show();
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }
    }

    public void CollectedPickup(int value) {
        score += value;
		uiManager.UpdateScoreText (score);
    }

    public float GetDistance() {
        return distance;
    }

    public int GetScore() {
        return score;
    }

    IEnumerator CheckVerticalSwipes() //Coroutine, wich gets Started in "Start()" and runs over the whole game to check for swipes
        {
        
        while (true) { //Loop. Otherwise we wouldnt check continously
            foreach (Touch touch in Input.touches) { //For every touch in the Input.touches - array...
                switch (touch.phase) {
                    case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
                        couldBeSwipe = true;

                        startPos = touch.position;  //Position where the touch started
                        swipeStartTime = Time.time; //The time it started
                        break;
                }
                float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
                float swipeDist = Mathf.Abs(touch.position.y - startPos.y); //Swipedistance

                if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) {
                    // It's a swipe
                    couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.

                    if (Mathf.Sign(touch.position.y - startPos.y) == 1f) { //Swipe-direction, either 1 or -1.
                        jump = true;

                    } else {
                        anim.SetTrigger("Slide");
                    }
                }
            }
            yield return null;
        }
    }
        private void AdjustSpeed() {
		if (!GameManager.gameStarted) {
            return;
        }
        float speedAdditive = 0;
        if(distance <= 1000) {
            speedAdditive = 1;
        } else if(distance <= 1500) {
            speedAdditive = 2;
        }else if(distance <= 2000) {
            speedAdditive = 3;
        }

        anim.SetFloat("Speed", movePower + speedAdditive);
    }

    public void SetTiltMode(bool isInTilt) {
        inTiltMode = isInTilt;
    }

	public void SetUIManager(UIManager ui){
		uiManager = ui;
	}

    private void SetHatColourToAllRenderers(Color color) {
        foreach (MeshRenderer hatPiece in hatParent.GetChild(0).GetComponentsInChildren<MeshRenderer>()) {
            hatPiece.material.color = color;
        }
    }

    public void SavePrefs() {
        if(PlayerPrefs.GetInt("DISTANCE") < distance) {
            PlayerPrefs.SetInt("DISTANCE", Mathf.RoundToInt(distance));
        }

        int totalOrbs = PlayerPrefs.GetInt("ORBS");

        PlayerPrefs.SetInt("ORBS", totalOrbs + score);
    }
}
