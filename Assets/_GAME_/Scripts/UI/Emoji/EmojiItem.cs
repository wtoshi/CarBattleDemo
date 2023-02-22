using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiItem : MonoBehaviour
{
    [SerializeField] Image emojiImg;

    int emojiIndex;
    GameEntries.EmojiData emojiData;

    public void Init(int index, GameEntries.EmojiData data)
    {
        emojiIndex = index;
        emojiData = data;

        emojiImg.sprite = emojiData.emojiSprite;
    }
}
