using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDetailsDialog : DialogController
{
    [SerializeField] TMP_Dropdown countryDropDown;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI alertTxt;

    string playerName;

    public override void OnShowDialog()
    {
        base.OnShowDialog();

        FillCountries();
        alertTxt.text = "";
    }

    public void SaveDetails()
    {
        playerName = nameInput.text;


        var selectedCountryOption = countryDropDown.options[countryDropDown.value];

        if (CheckRequirements())
        {
            PlayerDataController.Instance.SetName(playerName);

            var countryCode = GameUtils.GetCountries()[selectedCountryOption.text].CountryCode;
            PlayerDataController.Instance.SetCountry(new GameEntries.Country() { DisplayName = selectedCountryOption.text, CountryImg = selectedCountryOption.image, CountryCode = countryCode });

            customAction?.Invoke();

            Close();
        }
    }

    bool CheckRequirements()
    {
        alertTxt.text = "";
        string alert = "";

        // DROPDOWN CHECK
        if (string.IsNullOrEmpty(countryDropDown.options[countryDropDown.value].text))
        {
            alert = "Please select a country!";
        }

        // NAME CHECK
        if (playerName.Length <= 3)
        {
            alert = "Your name should be longer than 3 characters";
        }

        if (playerName.Length > 15)
        {
            alert = "Too long name..";
        }

        if (string.IsNullOrEmpty(alert))
        {
            return true;
        }
        else
        {
            alertTxt.text = alert;
            return false;
        }
    }

    public void OnValueChanged()
    {
        alertTxt.text = "";
    }

    void FillCountries()
    {
        var countryList = GameUtils.GetCountries();

        countryDropDown.options.Clear();
        foreach (var item in countryList)
        {
            countryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = item.Value.DisplayName , image = item.Value.CountryImg});
        }
    }
}
