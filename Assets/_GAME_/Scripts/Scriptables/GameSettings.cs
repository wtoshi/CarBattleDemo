using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    public string botNames;

    public GameObject parachutePF;

    [Header("DESTROYING")]
    public float destroyGap;
    public float circlePartFallingSpeed;
    public float circlePartBlinkDuration;
    public float circlePartBlinkSpeed;
    public Material circlePartDefaultMat;
    public Material circlePartBlinkMat;

}

