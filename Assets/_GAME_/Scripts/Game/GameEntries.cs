using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntries : MonoBehaviour
{
    public enum GameState
    {
        Initial,InMenu, InGame, LevelEnd
    }

    public enum EntityType
    {
        Player, AI
    }


    #region SAVE DATA

    [System.Serializable]
    public class LevelSaveData
    {
        public int currentLevelNo;

        public LevelSaveData()
        {
            currentLevelNo = 1;
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public Country playerCountry;
        public int playerLevel;
        public int golds;
        public List<CarDataEntry> ownedCars;
        public int currentCarID = -1;

        public PlayerData()
        {
            playerLevel = 1;
            playerCountry = new Country();
            playerName = "";
            golds = 0;
            ownedCars = new List<CarDataEntry>();
        }

    }

    [System.Serializable]
    public class CarDataEntry
    {
        public int carID = -1;
        public int currentCarPaintID = -1;
    }
    #endregion

    #region SETTINGS

    public enum EmojiType
    {
        Happy, GetHit, Defeated
    }

    [System.Serializable]
    public class EmojiData
    {
        public Sprite emojiSprite;
        public GameObject emojiPF;
        public EmojiType emojiType;
    }

    [System.Serializable]
    public class CarData
    {
        public int carID;
        public GameObject carPF;
        public List<Material> carPaints;
        public bool isDefaultCar;
    }

    [System.Serializable]
    public class Country
    {
        public string DisplayName;
        public string CountryCode;
        public Sprite CountryImg;
    }

    #endregion
}
