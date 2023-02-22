using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Entity : MonoBehaviour
{
	public GameEntries.EntityType entityType = GameEntries.EntityType.Player;

	[SerializeField] protected Transform UIBarHolder;
	[SerializeField] protected Transform emojiHolder;
	[SerializeField] protected Transform carHolder;

	protected UIBar _UIBar;
	protected CarDriver carDriver;
	protected CarController currentCar;

	public CarController CurrentCar => currentCar;

    private void Awake()
    {
		FirstInit();
	}

    private void Update()
    {
		if (Input.GetKeyDown( KeyCode.C))
		{
			InitBonus();
		}

	}

	private void OnEnable()
	{
		GameEvents.LevelLoaded += OnLevelLoaded;
		GameEvents.LevelStarted += OnLevelStarted;
		GameEvents.LevelFinished += OnLevelFinished;
	}

	private void OnDisable()
	{
		GameEvents.LevelLoaded -= OnLevelLoaded;
		GameEvents.LevelStarted -= OnLevelStarted;
		GameEvents.LevelFinished -= OnLevelFinished;
	}

    #region INIT

	void FirstInit()
    {
		carDriver = GetComponent<CarDriver>();

        if ( carDriver == null)
			Debug.LogError("Inialization ERROR!");

		SetCar();
	}

	public virtual void SetCar()
    {
		// Destroy currentCar
        if (currentCar!= null)
			Destroy(currentCar.gameObject);

		// IsAI?
		bool isAI = entityType == GameEntries.EntityType.AI;

		var carList = GameUtils.GetCars();

		int currentCarID = -1;
		if (!isAI)
        {
			currentCarID = PlayerDataController.Instance.PlayerData.currentCarID;
		}
        else
        {
			currentCarID = carList[Random.Range(0, carList.Count)].carID;
		}

		var carData = carList[currentCarID];
		var carObj = Instantiate(carData.carPF, carHolder);
		currentCar = carObj.GetComponent<CarController>();

		if (!isAI)
			GameEvents.Instance.OnPlayerCarSpawned();

		carDriver.currentCar = currentCar;

		var ownedCarData = PlayerDataController.Instance.GetOwnedCarData(currentCarID);

		int currentPaintID = isAI ? Random.Range(0, carData.carPaints.Count) : ownedCarData.currentCarPaintID;

		currentCar.Init(carData, currentPaintID);
	}

	#endregion

	#region STATUS
	public enum EntityStatus
    {
		Playing, Defeated
    }

	public EntityStatus entityStatus = EntityStatus.Playing;

	#endregion

	#region COMBAT



	Coroutine groundedCheckCO;
	IEnumerator doGroundedCheck()
    {
		bool grounded = false;

		while (true)
        {
			grounded = carDriver.CheckIsGrounded();

            if (!grounded)
            {
				yield return new WaitForSeconds(.5f);

				grounded = carDriver.CheckIsGrounded();

				if (!grounded)
				{
					ResetCarRot();
				}
			}

			yield return new WaitForSeconds(.5f);
			yield return null;
		}

    }

	public void StopGroundedChecking()
	{
		if (groundedCheckCO != null)
			StopCoroutine(groundedCheckCO);
	}

	public void SetCarNumber(int number)
    {
        currentCar.SetCarNumber(number);
    }

	Entity lastHitEntity;
	public virtual void GetHit(Entity lastHit, bool hitByTail, Vector3 forceDir)
    {
		lastHitEntity = lastHit;

        if (forceDir != Vector3.zero)
        {
			Debug.Log("forceDir: "+ forceDir);

			forceDir *= -1;
			forceDir.y = Random.Range(2,5);

			//currentCar.CarRB.AddForce(forceDir * 75, ForceMode.Impulse);
			//currentCar.CarRB.AddForce(forceDir * 25, ForceMode.Force);
			currentCar.CarRB.AddForce(forceDir * 50, ForceMode.Acceleration);
			//currentCar.CarRB.AddForce(forceDir * 25, ForceMode.VelocityChange);
		}
	}

	public virtual void Defeated()
    {
		entityStatus = EntityStatus.Defeated;

        if (lastHitEntity != null)
        {
			lastHitEntity.OnDefeatedEntity();
		}

		GameEvents.Instance.OnEntityDefeated(this);

		currentCar.DisableMotion(true);

		ResetCarRot(() => {
			WobbleCar();
		});

		Debug.Log("DEFEATED!");
    }

	public virtual void OnDefeatedEntity()
    {
		currentCar.UpgradeTailBall();
    }

	float bonusDuration;
	public virtual void InitBonus()
	{
		currentCar.SetTail(false);

		var bonusSphereObj = Instantiate(Master.Instance.GetResourcesByName<GameObject>("BonusSpherePF"), currentCar.BonusSphereHolder);
		BonusSphere bonusSphere = bonusSphereObj.GetComponent<BonusSphere>();

		bonusSphere.Set(this, 10f);
	}

	public virtual void ResetBonus()
    {
		currentCar.SetTail(true);
	}

	#endregion

	#region UTILS

	protected virtual void WobbleCar()
    {
        
		currentCar.transform.DOLocalMoveY(-.2f, .5f).SetEase( Ease.OutBack);
		currentCar.transform.DOShakeRotation(5, 10, 1, 10, false).SetLoops(-1, LoopType.Yoyo);
	}

	void ResetCarRot(System.Action onComplete = null)
    {
		var toRotate = Quaternion.Euler(0, currentCar.transform.eulerAngles.y, 0);
		currentCar.transform.DORotateQuaternion(toRotate, .5f).SetEase(Ease.Linear).OnComplete(() => {
			onComplete?.Invoke();
		}) ;

	}

	void SetCameraTarget()
    {
		// Set Camera Target For Player
		if (entityType != GameEntries.EntityType.Player)
			return;

		CameraManager.Instance.SetCameraTarget(currentCar.transform);
	}

    #endregion

    #region EMOJI
    public virtual void SendEmoji(GameObject emojiPF)
	{
		var emojiObj = Instantiate(emojiPF, emojiHolder);

		Destroy(emojiObj, 3f);
	}

    #endregion

    #region ANIMATIONS

	void PlayRandomCheer()
    {
		var random = Random.Range(1, 3);
		string animState = "DriverCheering" + random.ToString();
		currentCar.DriverAnimator.Play(animState);
	}

	void PlayIdle()
    {
		currentCar.DriverAnimator.Play("DriverIdle");
	}

    #endregion

    #region UI BAR
    protected virtual void AddUIBar()
	{
		var barPF = Master.Instance.GetResourcesByName<GameObject>("UIBar");

		_UIBar = Instantiate(barPF, UIBarHolder).GetComponent<UIBar>();

	}

	void RemoveUIBar()
	{
		if (_UIBar == null)
			return;

		Destroy(_UIBar.gameObject);
		_UIBar = null;

	}

	#endregion

	#region ACTIONS

	private void OnLevelFinished(bool successed)
	{
		StopGroundedChecking();

		currentCar.DisableMotion(true);
		ResetCarRot();

		if (successed)
			PlayRandomCheer();
	}

	private void OnLevelStarted()
	{
		AddUIBar();

		groundedCheckCO = StartCoroutine(doGroundedCheck());

		SetCameraTarget();

		currentCar.SetTail(true);
		currentCar.DisableMotion(false);

		entityStatus = EntityStatus.Playing;
	}

	private void OnLevelLoaded()
	{
		currentCar.SetTail(false);
		currentCar.DisableMotion(true);
		RemoveUIBar();
	}
	#endregion
}
