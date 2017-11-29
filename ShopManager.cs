using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour {

	//public RectTransform btnPrefab;
	public Text itemName, cost, description, playerFundsText;
	public Button[] categoryButtons;
    public GameObject[] selectButtons;
    public Slider genderToggle;
    public AudioClip nextSound, clickSound, rejectSound, equipSound;

	private CustomisationManager mgr;
    private AudioSource audioSource;
    private Animator anim;

	// Use this for initialization
	void Start () {
		mgr = FindObjectOfType<CustomisationManager> ();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        

        if (PlayerPrefs.GetString("GENDER").Equals("M")) {
            genderToggle.value = 0;
        } else {
            genderToggle.value = 1;
        }
        genderToggle.onValueChanged.AddListener(delegate { mgr.ToggleGender(Mathf.RoundToInt(genderToggle.value)); });
        InitialiseButtons();
        UpdateText();
        UpdateSelectButton();
	}

	void InitialiseButtons(){
        transform.Find("Navigation").Find("Prev Button").GetComponent<Button>().onClick.AddListener(() => Previous());
        transform.Find("Navigation").Find("Next Button").GetComponent<Button>().onClick.AddListener(() => Next());

        categoryButtons[0].onClick.AddListener(()=> SetCurrent(0));
		categoryButtons[1].onClick.AddListener(()=> SetCurrent(1));
		categoryButtons[2].onClick.AddListener(()=> SetCurrent(2));
		categoryButtons[3].onClick.AddListener(()=> SetCurrent(3));

        selectButtons[1].GetComponent<Button>().onClick.AddListener(()=> EquipItem());
        selectButtons[2].GetComponent<Button>().onClick.AddListener(() => CannotBuy());
	}

    void SetCurrent(int i) {
        audioSource.PlayOneShot(clickSound);
        mgr.SetCurrent(i);
        UpdateText();
        UpdateSelectButton();
    }

    private void Next() {
        audioSource.PlayOneShot(nextSound);
        anim.SetTrigger("NextItem");
        mgr.NextChoice();
        UpdateText();
        UpdateSelectButton();
    }

    private void Previous() {
        audioSource.PlayOneShot(nextSound);
        anim.SetTrigger("NextItem");
        mgr.PreviousChoice();
        UpdateText();
        UpdateSelectButton();
    }

    public void UpdateText() {
        Feature currFeature = mgr.features[mgr.currFeature];
        NewItem currItem = currFeature.ReturnItem();
        itemName.text = currItem.GetName();
        cost.text = currItem.GetCost().ToString();
        description.text = currItem.GetDescription();

        playerFundsText.text = PlayerPrefs.GetInt("ORBS").ToString();
    }

    public void UpdateSelectButton() {
        NewItem currItem = mgr.features[mgr.currFeature].ReturnItem();
        if (currItem.GetOwned()) {
            cost.text = "Already Owned";
            if (mgr.features[mgr.currFeature].currIndex == PlayerPrefs.GetInt("FEATURE_" + mgr.features.IndexOf(mgr.features[mgr.currFeature]))){
                SetSelectButtons(selectButtons[0]);
            } else {
                SetSelectButtons(selectButtons[1]);
            }
        } else {
            if (PlayerPrefs.GetInt("ORBS") < currItem.GetCost()) {
                SetSelectButtons(selectButtons[2]);
            } else {
                SetSelectButtons(selectButtons[3]);
            }
        }
    }

    void EquipItem() {
        audioSource.PlayOneShot(equipSound);
        mgr.EquipItem();
        UpdateSelectButton();
    }

    void CannotBuy() {
        audioSource.PlayOneShot(rejectSound);
        anim.SetTrigger("CannotBuy");
    }

    void SetSelectButtons(GameObject activeButton) {
        foreach(GameObject button in selectButtons) {
            button.SetActive(false);
        }
        activeButton.SetActive(true);
    }

}