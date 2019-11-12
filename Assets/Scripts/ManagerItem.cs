using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManagerItem : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;

    public Button buyButton;

    public Image icon;

    public string itemDescription;
    public string imageName;
    public int itemID;

    public int baseCost;

    public bool locked = false;

    public List<TapTimer> TapTimerItems { get; set; } = new List<TapTimer>();

    public void Start()
    {
        buyButton.onClick.AddListener(() => BuyItem());
        StartCoroutine(CanAffordUnlockCheck());

        TapTimerItems = TapTimerController.instance.PopulatedItems;
    }

    public void ConstructItem(string descriptionValue, string imageName, bool locked, int baseCost, int itemID)
    {
        SetDescription(descriptionValue);
        SetCost(baseCost);
        icon.sprite = Resources.Load<Sprite>("TapTimer/" + imageName);
        this.locked = locked;
        this.itemID = itemID;
    }

    public void SetDescription(string descriptionValue)
    {
        descriptionText.text = descriptionValue;
    }

    public void SetCost(int value)
    {
        baseCost = value;
        costText.text = "Cost: " + value.ToString();
    }

    public IEnumerator CanAffordUnlockCheck()
    {
        while (locked)
        {
            if (baseCost > CurrencyInventory.instance.currentBalance)
            {
                if (buyButton.interactable)
                    SetBuyButtonActive(false);
            }
            else
            {
                if (!buyButton.interactable)
                    SetBuyButtonActive(true);
            }

            yield return null;
        }

        SetBuyButtonActive(false);
        yield return null;
    }

    public void SetBuyButtonActive(bool active)
    {
        buyButton.interactable = active;
    }

    public void BuyItem()
    {
        if (locked)
        {
            if (CurrencyInventory.instance.currentBalance >= baseCost)
            {
                for (int i = 0; i < TapTimerItems.Count; i++)
                {
                    if (itemID == TapTimerItems[i].itemID)
                    {
                        TapTimerItems[i].ActivateManager(true);
                        SetBuyButtonActive(false);
                        CurrencyInventory.instance.SubtractCurrentBalance(baseCost);
                        locked = false;
                    }
                }
            }
        }
    }
}
