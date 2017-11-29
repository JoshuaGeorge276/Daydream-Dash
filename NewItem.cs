using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItem : MonoBehaviour {

    public string itemName = "";
    public int cost = 0;
    public string description;
    public int owned;// 0 meaning no and 1 meaning yes

    public string GetName() {
        return itemName;
    }

    public int GetCost() {
        return cost;
    }

    public string GetDescription() {
        return description;
    }

    public bool GetOwned() {
        // USE THIS TO RESET PURCHASED ITEMS 
        //PlayerPrefs.SetInt(itemName, owned);
        if (!PlayerPrefs.HasKey(itemName)) {
            PlayerPrefs.SetInt(itemName, owned);
        } else if (PlayerPrefs.GetInt(itemName) == 1) {
            return true;
        } else {
            return false;
        }
        return false;
    }
}
