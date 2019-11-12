using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CurrencyInventory : MonoBehaviour
{
    public static CurrencyInventory instance;
    public int currentBalance;

    public TextMeshProUGUI currencyText;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        UpdateText();
    }

    public void SetCurrentBalance(int value)
    {
        currentBalance = value;
        UpdateText();
    }

    public void AddCurrentBalance(int value)
    {
        currentBalance += value;
        UpdateText();
    }

    public void SubtractCurrentBalance(int value)
    {
        currentBalance -= value;
        UpdateText();
    }

    public void UpdateText()
    {
        currencyText.text = currentBalance.ToString();
    }
}
