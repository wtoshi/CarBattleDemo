using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static Dictionary<string, GameEntries.Country> GetCountries()
    {
        Dictionary<string, GameEntries.Country> countries = new Dictionary<string, GameEntries.Country>();

        var countryList = Resources.Load<CountrySettings>("CountryList");

        foreach (var country in countryList.countries)
        {
            countries.Add(country.DisplayName, country);
        }

        return countries;
    }

    public static List<GameEntries.EmojiData> GetEmojies()
    {
        var emojiSettings = Resources.Load<EmojiSettings>("EmojiList");

        return emojiSettings.emojiList;
    }

    public static List<GameEntries.CarData> GetCars()
    {
        var carSettings = Resources.Load<CarSettings>("CarList");

        return carSettings.carList;
    }

    #region FX

    public static EffectSettings GetEffectSettings()
    {
        var effectSettings = Resources.Load<EffectSettings>("EffectSettings");

        return effectSettings;
    }

    public static GameObject GetRandomGetHitFX(bool hitByTail)
    {
        var effectSettings = Resources.Load<EffectSettings>("EffectSettings");

        List<GameObject> getHitFXList;
        if (hitByTail)
        {
            getHitFXList = effectSettings.HitByTailFXs;
        }
        else
        {
            getHitFXList = effectSettings.HitByCarFXs;
        }

        return getHitFXList[Random.Range(0, getHitFXList.Count)];
    }

    #endregion

}
