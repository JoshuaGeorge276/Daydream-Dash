using UnityEngine;
using UnityEngine.UI;

public class PurchaseMenu : SimpleMenu<PurchaseMenu> {

    public Text itemNameText;
    public AudioClip menuAppear, closeMenu, purchaseSound;

    private AudioSource audioSource;
    private CustomisationManager mgr;

	void OnEnable() {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuAppear);
        mgr = FindObjectOfType<CustomisationManager>();
        NewItem currPurchaseItem = mgr.features[mgr.currFeature].ReturnItem();
        itemNameText.text = currPurchaseItem.GetName() + " for " + currPurchaseItem.GetCost() + " orbs?";
    }

    public void ConfirmPurchase() {
        ShopManager sgr = FindObjectOfType<ShopManager>();
        AudioSource.PlayClipAtPoint(purchaseSound, Camera.main.transform.position);
        mgr.BuyItem();
        mgr.EquipItem();
        sgr.UpdateText();
        sgr.UpdateSelectButton();
        Close();
    }

    public override void OnBackPressed() {
        AudioSource.PlayClipAtPoint(closeMenu, Camera.main.transform.position);
        audioSource.PlayOneShot(menuAppear);
        base.OnBackPressed();
    }
}
