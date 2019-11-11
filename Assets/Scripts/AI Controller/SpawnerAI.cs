using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Will be created within the room walls, and will move to the 
 * arena platform. When it arrives within a certain range
 * of the center, it will begin spawning seeker enemies in waves.
 * 
 * It drifts slowly around the arena platform, revolving while aligning
 * one of its corners straight downward, like a spinning top.
 * 
 * Its outer black shell is invulernable to attack; it can only be
 * destroyed by shooting its inner core.
 */

[RequireComponent(typeof(Rigidbody))]
public class SpawnerAI : MonoBehaviour, IDamageable
{
	[Tooltip("How far forward to move from the wall before moving to the spawn area.")]
	[SerializeField] private float awakenDistance = 6f;

	[Tooltip("Length of time to cover the distance above.")]
	[SerializeField] private float awakenDuration = 3f;

	[Tooltip("Speed of the enemy when moving to the spawn area.")]
	[SerializeField] private float movementSpeed = 4f;

	[Tooltip("Minimum time to wait after spawning to spawn again.")]
	[SerializeField] private float minSpawnCooldown = 25f;

	[Tooltip("Maximum time to wait after spawning to spawn again.")]
	[SerializeField] private float maxSpawnCooldown = 35f;

	[SerializeField] private GameObject enemyToSpawnPrefab;
	[SerializeField] private Transform[] spawnPoints = new Transform[3];

	private Rigidbody _rigidbody;
	private Vector3 _destinationPoint;
	private bool _isFullyAwake = false;
	private bool _isInSpawnArea = false;
	private bool _canSpawn = true;

	private StateController _stateController;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_destinationPoint = GameObject.FindGameObjectWithTag("Spawn Area").transform.position;
		_rigidbody.velocity = transform.forward * (awakenDistance / awakenDuration);
		_stateController = GameObject.FindObjectOfType<StateController>();
	}

	void Update()
    {
		if (_stateController.GetGameState() == StateController.GameState.Stopped) return;

		if (_isFullyAwake)
		{
			// Always try to be in the spawn area
			if (!_isInSpawnArea)
			{
				MoveToSpawnArea();
			}

			// NOTE: Spawners can spawn enemies outside of
			// the spawn area, to prevent layers killing them before
			// they get at least one wave out
			if (_canSpawn)
			{
				StartCoroutine(SpawnBehavior());
			}
		}
	}

	IEnumerator SpawnBehavior(int numberOfWaves = 1, float delayBetweenWaves = 0.5f)
	{
		_canSpawn = false;

		for (int i = 0; i < numberOfWaves; i++)
		{
			CreateEnemies();

			if (numberOfWaves > 1)
			{
				yield return new WaitForSeconds(delayBetweenWaves);
			}
		}

		yield return new WaitForSeconds(RandomCooldownTime());

		_canSpawn = true;
	}

	private void CreateEnemies()
	{
		foreach (Transform spawnPoint in spawnPoints)
		{
			GameObject seeker = Instantiate(
				enemyToSpawnPrefab,
				spawnPoint.position,
				spawnPoint.rotation);
		}
	}

	private float RandomCooldownTime()
	{
		return UnityEngine.Random.Range(minSpawnCooldown, maxSpawnCooldown);
	}

	private void MoveToSpawnArea()
	{
		Vector3 direction = _destinationPoint - transform.position;
		_rigidbody.velocity = direction.normalized * movementSpeed;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Spawn Area"))
		{
			_isInSpawnArea = true;
			_rigidbody.velocity = Vector3.zero;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Spawn Area"))
		{
			_isInSpawnArea = false;
		}
	}

	private void AwakeningFinished()
	{
		_isFullyAwake = true;
		GetComponent<Animator>().SetTrigger("hasAwoken");
	}

	public void TakeDamage()
	{
		Destroy(gameObject);
	}
}
