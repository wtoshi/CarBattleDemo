using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIPlayerController : Entity
{
    #region AI EMOJI

    public override void GetHit(Entity lastHit, bool hitByTail, Vector3 hitForce)
    {
        base.GetHit(lastHit, hitByTail, hitForce);

        //if (lastHit.entityType == GameEntries.EntityType.Player)
        //{
        //    TextShower.Instance.ShowText("PERFECT HIT!", TextShower.TextShowerType.HitEntity);
        //}

        // Send Emoji SAD

        if (hitByTail)
        {
            var emojiList = GameUtils.GetEmojies();

            var sadEmojies = emojiList.FindAll(x => x.emojiType == GameEntries.EmojiType.GetHit).ToList();

            var emoji = sadEmojies[Random.Range(0, sadEmojies.Count)].emojiPF;

            SendEmoji(emoji);
        }
    }

    public override void Defeated()
    {
        base.Defeated();

        // Send Emoji 
        var emojiList = GameUtils.GetEmojies();

        var sadEmojies = emojiList.FindAll(x => x.emojiType == GameEntries.EmojiType.Defeated).ToList();

        var emoji = sadEmojies[Random.Range(0, sadEmojies.Count)].emojiPF;

        SendEmoji(emoji);

    }

    public override void OnDefeatedEntity()
    {
        base.OnDefeatedEntity();

        // Send Emoji HAHA

        var emojiList = GameUtils.GetEmojies();

        var sadEmojies = emojiList.FindAll(x => x.emojiType == GameEntries.EmojiType.Happy).ToList();

        var emoji = sadEmojies[Random.Range(0, sadEmojies.Count)].emojiPF;

        SendEmoji(emoji);

    }

    #endregion


    #region UI BAR
    protected override void AddUIBar()
    {
        base.AddUIBar();

        if (_UIBar == null)
            return;

        var countryList = GameUtils.GetCountries();
        Sprite flagSprite = countryList.ElementAt(Random.Range(0, countryList.Count)).Value.CountryImg;

        var botNames = GameManager.Instance.GameSettings.botNames.Split(',');
        string botName = botNames[Random.Range(0, botNames.Length)];

        Debug.Log("BotName: "+ botName);
        Debug.Log("Flag: " + flagSprite.name);
        _UIBar.Init(flagSprite, botName);
    }

    #endregion
}
