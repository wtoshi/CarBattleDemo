using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountrySettings", menuName = "Settings/CountrySettings")]
public class CountrySettings : ScriptableObject
{
    public List<GameEntries.Country> countries = new List<GameEntries.Country>();

}

