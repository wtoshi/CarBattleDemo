using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuCurrencyInfo : MonoBehaviour
{
    // Economy
    public TextMeshProUGUI gold;

    private void OnEnable()
    {
        GameEvents.CurrencyUpdated += UpdateCurrency;

        SetCurrency();
    }

    private void OnDisable()
    {
        GameEvents.CurrencyUpdated -= UpdateCurrency;
    }

    public void SetCurrency()
    {
        SetGold();
    }

    public void SetGold()
    {
        gold.text = PlayerDataController.Instance.PlayerData.golds.ToString();
    }

    void UpdateCurrency()
    {
        var currentGoldVal = int.Parse(gold.text);
        var newGoldVal = PlayerDataController.Instance.PlayerData.golds;

        DOTween.To(() => currentGoldVal, x => gold.text = x.ToString(), newGoldVal, 0.5f);
    }

}
