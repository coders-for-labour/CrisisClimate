using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TapTimer : MonoBehaviour
{
    public Button buyButton;
    public Button activateButton;

    public string itemName;
    public string itemDescription;
    public string imageName;
    public int itemID;

    public float baseTimerLength;
    public int valueToAddOnCompletion;
    public int baseCost;
    public int numberOwned;

    public bool locked = false;
    public bool hasManager;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI ownedText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI costText;
    public Text buttonText;

    public Image barForeground;
    public Image icon;

    public Coroutine barCoroutine;
    public Coroutine timerCoroutine;

    public Action action;

    public void Start()
    {
        activateButton.onClick.AddListener(() => ReduceForegroud(baseTimerLength));
        activateButton.onClick.AddListener(() => BeginCountdown(baseTimerLength));
        buyButton.onClick.AddListener(() => UnlockOrBuyItem());
        StartCoroutine(CanAffordUnlockCheck());
    }

    public void ConstructItem(string titleValue, string descriptionValue, string imageName, float timerSeconds, int ownedAmount, bool locked, int baseCost, int valueToAddUponCompletion, int itemID, bool hasManager)
    {
        SetText(titleValue, descriptionValue);
        SetItemsOwned(ownedAmount);
        SetSeconds(timerSeconds);
        SetCost(baseCost);
        SetValueToAdd(valueToAddUponCompletion);
        icon.sprite = Resources.Load<Sprite>("TapTimer/" + imageName);
        this.locked = locked;
        this.itemID = itemID;
        this.hasManager = hasManager;

        buyButton.interactable = false;
    }

    public void SetText(string titleValue, string descriptionValue)
    {
        titleText.text = titleValue;
        descriptionText.text = descriptionValue;
    }

    public void SetItemsOwned(int value)
    {
        numberOwned = value;
        ownedText.text = "Owned: " + numberOwned.ToString();
    }

    public void SetSeconds(float value)
    {
        baseTimerLength = value;
        var time = TimeSpan.FromSeconds(value);
        timerText.text = "Timer:" + time.ToString(@"hh\:mm\:ss\:fff");
    }

    public void SetCost(int value)
    {
        baseCost = value;
        costText.text = "Cost: " + value.ToString();
    }

    public void SetValueToAdd(int value)
    {
        valueToAddOnCompletion = value;
        //costText.text = "Cost: " + value.ToString();
    }

    public void ActivateManager(bool active)
    {
        hasManager = active;
        StartCoroutine(AutoManager());
    }

    public IEnumerator AutoManager()
    {
        while (hasManager)
        {
            while (!activateButton.interactable)
            {
                yield return null;
            }

            ReduceForegroud(baseTimerLength);
            BeginCountdown(baseTimerLength);

            yield return null;
        }

        yield return null;
    }

    public IEnumerator CanAffordUnlockCheck()
    {
        while (true)
        {
            if (baseCost > CurrencyInventory.instance.currentBalance)
            {
                if(buyButton.interactable)
                    SetBUYButtonActive(false);
            }
            else
            {
                if(!buyButton.interactable)
                    SetBUYButtonActive(true);
            }

            yield return null;
        }
    }

    public void UnlockOrBuyItem()
    {
        if (locked)
        {
            if (baseCost <= CurrencyInventory.instance.currentBalance)
            {
                CurrencyInventory.instance.SubtractCurrentBalance(baseCost);
                locked = false;
                SetACTIVATEButtonActive(true);
                buttonText.text = "Buy";

                var amountToSet = numberOwned = 1;
                SetItemsOwned(amountToSet);
            }
        }
        else
        {
            if (baseCost <= CurrencyInventory.instance.currentBalance)
            {
                CurrencyInventory.instance.SubtractCurrentBalance(baseCost);
                numberOwned++;
                var amountToSet = numberOwned;
                //Debug.Log("Awarding " + (valueToAddOnCompletion * itemsOwned).ToString() + " Loves");
                SetItemsOwned(amountToSet);
            }
        }
    }

    public void SetBUYButtonActive(bool active)
    {
        buyButton.interactable = active;
    }

    public void SetACTIVATEButtonActive(bool active)
    {
        activateButton.interactable = active;
    }

    public void ReduceForegroud(float seconds)
    {
        if (locked)
            return;

        if (barCoroutine != null)
        {
            StopCoroutine(barCoroutine);
            barCoroutine = null;
        }

        barCoroutine = StartCoroutine(ReduceForegroud_Co(seconds));
    }

    public IEnumerator ReduceForegroud_Co(float seconds)
    {
        if (action != null)
        {
            action();
        }

        float duration = seconds; 
                            
        float normalizedTime = 0;
        SetACTIVATEButtonActive(false);
        while (normalizedTime <= 1f)
        {
            barForeground.fillAmount = normalizedTime;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        if (valueToAddOnCompletion > 0)
        {
            //Add to total
            CurrencyInventory.instance.AddCurrentBalance(valueToAddOnCompletion * numberOwned);
            Debug.Log("Awarding " + (valueToAddOnCompletion * numberOwned).ToString());
        }

        barForeground.fillAmount = 0f;
        SetACTIVATEButtonActive(true);
        yield return null;
    }

    public void BeginCountdown(float seconds)
    {
        if (locked)
            return;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timerCoroutine = StartCoroutine(BeginCountdown_Co(seconds));
    }

    public IEnumerator BeginCountdown_Co(float seconds)
    {
        TimeSpan time; 

        time = TimeSpan.FromSeconds(seconds);
        string output = "Timer:" + time.ToString(@"hh\:mm\:ss\:fff");
        timerText.text = output;

        float secondsToWait = 0f;

        while (secondsToWait <= 1f)
        {
            secondsToWait += Time.deltaTime / seconds;

            time = TimeSpan.FromSeconds(secondsToWait);
            output = "Timer:" + time.ToString(@"hh\:mm\:ss\:fff");
            timerText.text = output;
            yield return null;
        }

        //Debug.LogError("Done");

        //reset
        time = TimeSpan.FromSeconds(seconds);
        output = "Timer:" + time.ToString(@"hh\:mm\:ss\:fff");
        timerText.text = output;

        yield return null;
    }
}
