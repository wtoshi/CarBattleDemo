using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmojiSettings", menuName = "Settings/EmojiSettings")]
public class EmojiSettings : ScriptableObject
{
    public List<GameEntries.EmojiData> emojiList = new List<GameEntries.EmojiData>();

}

