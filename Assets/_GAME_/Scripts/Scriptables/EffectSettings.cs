using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSettings", menuName = "Settings/EffectSettings")]
public class EffectSettings : ScriptableObject
{
    public GameObject hitWaterFX;
    public GameObject successConfettiFx;
    public GameObject perkLandedFx;
    public GameObject perkPickUPFx;
    public List<GameObject> HitByCarFXs = new List<GameObject>();
    public List<GameObject> HitByTailFXs = new List<GameObject>();
}

