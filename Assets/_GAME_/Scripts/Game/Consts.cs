using System.Collections.Generic;
using UnityEngine;

public class Consts : MonoBehaviour
{
    public struct Tags
    {
        public const string Collectable = "Collectable";
        public const string Upgrade = "Upgrade";
        public const string Edible = "Edible";
        public const string BlockJump = "BlockJump";
        public const string Finishline = "Finishline";
        public const string RopeNode = "RopeNode";
        public const string Stair = "Stair";
        public const string Tutorial = "Tutorial";
    }

    public struct FileNames
    {
        public const string LEVELDATA = "level.dat";
        public const string PLAYERDATA = "player.dat";
    }

    public struct AnalyticsEventNames
    {
        public const string LEVEL_START = "Start";
        public const string LEVEL_SUCCESS = "Success";
        public const string LEVEL_FAIL = "Fail";
    }

    public struct PrefKeys
    {
        public const string HAPTIC = "Haptic";
    }

    public struct AnalyticsDataName
    {
        public const string LEVEL = "Level";
    }

    public static readonly Dictionary<int, string> NumberAbbrs = new Dictionary<int, string>
    {
        {1000000000, "B" },
        {1000000, "M" },
        {1000,"K"}
    };
}

