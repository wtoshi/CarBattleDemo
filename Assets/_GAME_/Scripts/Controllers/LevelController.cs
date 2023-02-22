using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class LevelController : Singleton<LevelController>
{
	#region Levels
	[SerializeField, FoldoutGroup("Levels")]
	protected LevelContent[] allLevels;
	[SerializeField, FoldoutGroup("Levels")]
	protected LevelContent[] levelsToRepeat;
	#endregion

    #region Serialized
    [SerializeField] GameObject playerPF;
	[SerializeField] Transform levelParent;
    #endregion

    #region Local
    PlayerController player;
    LevelContent levelContent;
    LevelFacade levelFacade;
    int levelNo;
    #endregion

    public static bool IsTutorial;

    public PlayerController Player => player;
    public LevelFacade LevelFacade => levelFacade;
    public LevelContent LevelContent => levelContent;

    private readonly List<Object> _destroyOnResetList = new List<Object>();

	protected virtual void OnEnable()
	{
		//EventManager.LevelSuccessEvent.AddListener(OnLevelSuccess);
	}

	protected virtual void OnDisable()
	{
		//EventManager.LevelSuccessEvent.RemoveListener(OnLevelSuccess);
	}

    private void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
	{
        levelContent = GetLevelContent();

		PrepareLevel();
	}

	private LevelContent GetLevelContent()
	{
        levelNo = LevelDataController.Instance.LevelData.currentLevelNo;

		Debug.Log("LevelNo: " + levelNo + " allLevels.Length: " + allLevels.Length + " currentLevelIndex: " + (levelNo - 1).ToString());

		if (levelNo -1 < allLevels.Length)
		{
			return allLevels[levelNo-1];
		}

		int random = Random.Range(0, levelsToRepeat.Length);
		return levelsToRepeat[random];
	}

	private void PrepareLevel()
	{
        levelFacade = InstantiateAsDestroyable<LevelFacade>(levelContent.LevelFacade);
        IsTutorial = levelContent.IsTutorialLevel;

        player = Instantiate(playerPF, levelFacade.PlayerSpawnPosition.position, levelFacade.PlayerSpawnPosition.rotation, levelFacade.transform).GetComponent<PlayerController>();       

        //this is the place where you should add your in-game logic such as instantiating player etc.

        Transform target = player.transform;
        if (target != null)
        {
			CameraManager.Instance.Init(target);
        }

        GameEvents.Instance.OnLevelLoaded();
    }

    public virtual void RestartLevel()
    {
        ResetLevel();
        GameEvents.Instance.OnLevelRestarted();

        LoadLevel();
    }

    protected virtual void ResetLevel()
    {
        foreach (Object obj in _destroyOnResetList)
        {
            switch (obj)
            {
                case GameObject go:
                    Destroy(go);
                    break;
                case Component component:
                    Destroy(component.gameObject);
                    break;
            }
        }
        _destroyOnResetList.Clear();

        Destroy(player.gameObject);
    }

    /// <summary>
    /// This method collects all the Objects(GameObjects and Components) instantiated in the game so that they can be destroyed when ResetLevel is called.
    /// </summary>
    /// <returns>Return the initiated object.</returns>
    protected T InstantiateAsDestroyable<T>(Object obj) where T : Object
    {
        T t = Instantiate(obj, levelParent) as T;
        _destroyOnResetList.Add(t);
        return t;
    }

}