using UnityEngine;

// The header CreateAssetMenu.. allows us to create instances of this class in the Project
[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
	// set your data here

	// Basic Geometry
	public float groundDistance = -3.34f; // y position to spawn enemies

	// Mario basic starting values
	public int playerMaxSpeed = 15;
	public int playerMaxJumpSpeed = 39;
	public int playerDefaultForce = 100;

	// for Scoring system
	int currentScore;
	int currentPlayerHealth;

	// for Reset values
	Vector3 gombaSpawnPointStart = new Vector3(2.5f, -0.45f, 0); // hardcoded location
																 // .. other reset values 

	// for Consume.cs
	public int consumeTimeStep = 10;
	public int consumeLargestScale = 4;

	// for Break.cs
	public int breakTimeStep = 30;
	public int breakDebrisTorque = 10;
	public int breakDebrisForce = 10;

	// for SpawnDebris.cs
	public int spawnNumberOfDebris = 10;

	// for Rotator.cs
	public int rotatorRotateSpeed = 6;

	// for Game Logic
	public int playerScore;

	// Ground for enemy
	public float groundSurface = -3.34f; // y position to spawn enemies

	//public float groundSurface = -3.92f; // actual ground surface

	// enemyController.cs
	public float maxOffset = 5.0f;
	public float enemyPatroltime = 3.0f;

	// for testing
	public int testValue;

}