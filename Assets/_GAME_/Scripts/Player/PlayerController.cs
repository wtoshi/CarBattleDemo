using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    public override void OnDefeatedEntity()
    {
        base.OnDefeatedEntity();

        TextShower.Instance.ShowText("You Defeated Him!", TextShower.TextShowerType.DefeatedEntity);
    }

    protected override void WobbleCar()
    {
        var playerCarDriver = carDriver as PlayerCarDriver;

        playerCarDriver.joystick.Reset();
        base.WobbleCar();
    }

    #region UI BAR
    protected override void AddUIBar()
    {
        base.AddUIBar();

        if (_UIBar == null)
            return;

        Sprite flagSprite = PlayerDataController.Instance.PlayerData.playerCountry.CountryImg;
		string playerName = PlayerDataController.Instance.PlayerData.playerName;

        _UIBar.Init(flagSprite, playerName);
    }

    #endregion

}
