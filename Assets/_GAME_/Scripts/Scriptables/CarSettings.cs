using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarSettings", menuName = "Settings/CarSettings")]
public class CarSettings : ScriptableObject
{
    public List<GameEntries.CarData> carList = new List<GameEntries.CarData>();

}