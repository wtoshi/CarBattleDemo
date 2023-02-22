using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : Singleton<GameEvents>
{
    // LEVEL
    public static event Action LevelLoaded;
    public static event Action LevelStarted;
    public static event Action LevelRestarted;
    public static event Action<bool> LevelFinished;

    // CURRENCY
    public static event Action CurrencyUpdated;

    // CAR
    public static event Action PlayerCarSpawned;

    // Entity
    public static event Action<Entity> EntityDefeated;


    #region   LEVEL
    public virtual void OnLevelLoaded()
    {
        LevelLoaded?.Invoke();
    }

    public virtual void OnLevelStarted()
    {
        LevelStarted?.Invoke();
    }
    public virtual void OnLevelRestarted()
    {
        LevelRestarted?.Invoke();
    }

    public virtual void OnLevelFinished(bool successed)
    {
        LevelFinished?.Invoke(successed);
    }

    #endregion

    #region CURRENCY
    public virtual void OnCurrencyUpdated()
    {
        CurrencyUpdated?.Invoke();
    }
    #endregion

    #region CAR
    public virtual void OnPlayerCarSpawned()
    {
        PlayerCarSpawned?.Invoke();
    }
    #endregion


    #region ENTITY
    public virtual void OnEntityDefeated(Entity entity)
    {
        EntityDefeated?.Invoke(entity);
    }
    #endregion
}
