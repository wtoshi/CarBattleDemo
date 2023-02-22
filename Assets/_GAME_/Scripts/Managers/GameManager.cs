using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    GameSettings gameSettings;
    GameEntries.GameState gameState = GameEntries.GameState.Initial;

    public GameSettings GameSettings => gameSettings;
    public GameEntries.GameState GameState { get => gameState; set => gameState = value; }

    List<int> userCarNumbers = new List<int>();
    List<Entity> entityList = new List<Entity>();

    GameObject confettiFX;
    

    protected override void Awake()
    {
        gameSettings = Resources.Load<GameSettings>("GameSettings");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    GameEvents.Instance.OnLevelFinished(true);
        //}
    }

    private void OnEnable()
    {
        GameEvents.LevelLoaded += OnLevelLoaded;
        GameEvents.LevelFinished += OnLevelFinished;
        GameEvents.EntityDefeated += OnEntityDeafeated;
    }

    private void OnDisable()
    {
        GameEvents.LevelLoaded -= OnLevelLoaded;
        GameEvents.LevelFinished -= OnLevelFinished;
        GameEvents.EntityDefeated -= OnEntityDeafeated;
    }

    public void StartLevel()
    {
        PrepareForBattle();

        gameState = GameEntries.GameState.InGame;

        parachuteCO = StartCoroutine(doSendParachutes());

        Debug.Log("Level Started");

        GameEvents.Instance.OnLevelStarted();
    }

    void PrepareForBattle()
    {
        if (confettiFX != null)
            Destroy(confettiFX.gameObject);

        userCarNumbers = new List<int>();
        entityList = new List<Entity>();
        entityList.Add(LevelController.Instance.Player);

        AddAICars();

        SetNumbers();
    }


    void SetNumbers()
    {
        int countOfCouldntUseNumber = 0;

        foreach (var entity in entityList)
        {
            var maxCarNumber = 50;
            var carNumber = Random.Range(0, maxCarNumber);

            bool canUse = !userCarNumbers.Contains(carNumber);

            int tryCount = 0;
            int maxTry = 3;


            while (!canUse)
            {
                countOfCouldntUseNumber++;

                carNumber = Random.Range(0, maxCarNumber);
                canUse = !userCarNumbers.Contains(carNumber);

                if (canUse)
                {
                    userCarNumbers.Add(carNumber);

                    break;
                }
                else
                {
                    if (tryCount < maxTry)
                    {
                        tryCount++;
                    }
                    else
                    {
                        carNumber = maxCarNumber + countOfCouldntUseNumber;
                        break;
                    }
                }
            }

            entity.SetCarNumber(carNumber);
        }
    }

    void AddAICars()
    {
        var botCount = LevelController.Instance.LevelContent.BotCount;

        for (int i = 0; i < botCount; i++)
        {
            var AICarPF = Master.Instance.GetResourcesByName<GameObject>("AIPlayer");
            var AICarObj = Instantiate(AICarPF, LevelController.Instance.LevelFacade.transform);

            var AISpawnPosList = LevelController.Instance.LevelFacade.AISpawnPositionList;

            var randomPos = AISpawnPosList[Random.Range(0, AISpawnPosList.Count)];
            AICarObj.transform.position = randomPos.position;

            AISpawnPosList.Remove(randomPos);

            var AIEntity = AICarObj.GetComponent<Entity>();

            entityList.Add(AIEntity);
        }
    }

    void CheckPlayerWon()
    {
        if (gameState != GameEntries.GameState.InGame)
            return;

        bool playerWon = true;
        foreach (var entity in entityList)
        {
            if (entity == LevelController.Instance.Player)  continue;

            if (entity.entityStatus == Entity.EntityStatus.Playing)
            {
                playerWon = false;

                break;
            }

        }

        if (playerWon)
        {
            var confettiHolder = Camera.main.transform.GetChild(0);
            confettiFX = FXController.CreateEffect(GameUtils.GetEffectSettings().successConfettiFx, Vector3.zero, confettiHolder.gameObject, .3f, 0);

            UIManager.Get<MenuUI>().hasRewards = true;

            GameEvents.Instance.OnLevelFinished(true);
        }
            

    }

    Coroutine parachuteCO;
    GameObject lastParachute;
    IEnumerator doSendParachutes()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            if (lastParachute==null)
            {
                lastParachute = Instantiate(gameSettings.parachutePF, LevelController.Instance.LevelFacade.transform);

                lastParachute.transform.position = Vector3.up * 2;
            }


            yield return null;
        }
    }

    void OnEntityDeafeated(Entity entity)
    {
        if (gameState != GameEntries.GameState.InGame)
            return;

        if (entity.entityType == GameEntries.EntityType.Player)
        {
            GameEvents.Instance.OnLevelFinished(false);
        }
        else
        {
            entity.StopGroundedChecking();

            if (entityList.Contains(entity))
            {
                entityList.Remove(entity);
            }
            
            CheckPlayerWon();
        }
    }

    void OnLevelLoaded()
    {
        if (confettiFX != null)
            Destroy(confettiFX.gameObject);

        gameState = GameEntries.GameState.InMenu;
    }

    void OnLevelFinished(bool successed)
    {
        gameState = GameEntries.GameState.LevelEnd;

        if (parachuteCO != null)
            StopCoroutine(parachuteCO);

        if (lastParachute != null)
            Destroy(lastParachute);
    }
}
