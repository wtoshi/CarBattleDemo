using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarDriver : CarDriver
{
    public Joystick joystick;
    Entity entity;

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>(true);
        entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        GameEvents.LevelFinished += OnLevelFinished;
    }

    private void OnDisable()
    {
        GameEvents.LevelFinished -= OnLevelFinished;
    }

    public override void DriveCar()
    {

        if (joystick == null || entity == null || entity.entityStatus == Entity.EntityStatus.Defeated || GameManager.Instance.GameState != GameEntries.GameState.InGame)
        {
            Vertical = 0;
            Horizontal = 0;
            return;
        }

        Vertical = joystick.Vertical;
        Horizontal = joystick.Horizontal;

        base.DriveCar();
    }

    void OnLevelFinished(bool successed)
    {
        joystick.Reset();
    }
}
