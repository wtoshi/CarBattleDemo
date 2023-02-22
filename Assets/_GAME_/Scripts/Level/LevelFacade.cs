using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds all the references required for a level to be instantiated
/// </summary>
public class LevelFacade : MonoBehaviour
{
	[SerializeField] Transform playerSpawnPosition;
	[SerializeField] List<Transform> aISpawnPositionList = new List<Transform>();
	//[SerializeField] ArenaController arenaController;

	public Transform PlayerSpawnPosition => playerSpawnPosition;
	public List<Transform> AISpawnPositionList => aISpawnPositionList;
	//public ArenaController ArenaController => arenaController;

}