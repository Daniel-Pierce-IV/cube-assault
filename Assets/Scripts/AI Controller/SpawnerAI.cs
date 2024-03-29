﻿using System.Collections;
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
public class SpawnerAI : MonoBehaviour, IDamageable, IPoolable
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

	[SerializeField] private int initialWavesPerSpawn = 1;
	[SerializeField] private int finalWavesPerSpawn = 3;

	[SerializeField] private GameObject enemyToSpawnPrefab;
	[SerializeField] private Transform[] spawnPoints = new Transform[3];

	[SerializeField] private AudioClip deathSoundClip;
	[SerializeField] private float deathSoundVolume = 0.5f;

	[SerializeField] private ParticleSystem deathVFX;

	private Rigidbody _rigidbody;
	private Collider[] _colliders;
	private MeshRenderer[] _meshRenderers;
	private AudioSource _audioSource;
	private Vector3 _destinationPoint;
	private bool _isFullyAwake;
	private bool _isInSpawnArea;
	private float _nextSpawnTimestamp;
	private float _initialVolume;
	private float _deathEndingTimestamp;
	private float _deathDuration;

	private StateController _stateController;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_colliders = GetComponentsInChildren<Collider>();
		_meshRenderers = GetComponentsInChildren<MeshRenderer>();
		_audioSource = GetComponent<AudioSource>();
		_initialVolume = _audioSource.volume;
		_deathDuration = Mathf.Max(deathVFX.main.startLifetime.constant, deathSoundClip.length);
	}

	private void OnEnable()
	{
		_isFullyAwake = false;
		_isInSpawnArea = false;
		_nextSpawnTimestamp = 0f;
		_rigidbody.velocity = transform.forward * (awakenDistance / awakenDuration);
	}

	void Start()
	{
		_destinationPoint = GameObject.FindGameObjectWithTag("Spawn Area").transform.position;
		_stateController = GameObject.FindObjectOfType<StateController>();
	}

	void Update()
    {
		if (_stateController.GetGameState() == StateController.GameState.Stopped) return;

		if (Time.timeSinceLevelLoad < _deathEndingTimestamp)
		{
			// Slowly turn down volume to avoid "popping"
			_audioSource.volume -= Time.deltaTime * _deathDuration;
		}
		else if (_isFullyAwake)
		{
			// Always try to be in the spawn area
			if (!_isInSpawnArea)
			{
				MoveToSpawnArea();
			}

			// NOTE: Spawners can spawn enemies outside of
			// the spawn area, to prevent layers killing them before
			// they get at least one wave out
			if (Time.timeSinceLevelLoad > _nextSpawnTimestamp)
			{
				StartCoroutine(StartSpawning());
			}
		}
	}

	IEnumerator StartSpawning(float delayBetweenWaves = 0.5f)
	{
		// Set next spawn time immediately, so that wave delays dont
		// cause a glitchy enormous spawn scenario
		SetNextSpawnTimestamp();

		int _curWaves = Progression.Instance().CurrentValue(
			initialWavesPerSpawn,
			finalWavesPerSpawn);

		for (int i = 0; i < _curWaves; i++)
		{
			CreateEnemies();

			if (_curWaves > 1)
			{
				yield return new WaitForSeconds(delayBetweenWaves);
			}
		}

	}

	private void CreateEnemies()
	{
		foreach (Transform spawnPoint in spawnPoints)
		{
			GameObject seeker = ObjectPoolManager.instance
				.GetObjectPoolByTag(enemyToSpawnPrefab.tag)
				.GetOrCreate();

			seeker.transform.position = spawnPoint.position;
			seeker.transform.rotation = spawnPoint.rotation;
			seeker.GetComponent<IPoolable>().Activate();
		}
	}
	private void SetNextSpawnTimestamp()
	{
		float cooldownTime = Random.Range(minSpawnCooldown, maxSpawnCooldown);
		_nextSpawnTimestamp = Time.timeSinceLevelLoad + cooldownTime;
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
		Deactivate();
	}

	private void EnableCollidersAndMeshes(bool value)
	{
		foreach (Collider collider in _colliders)
		{
			collider.enabled = value;
		}

		foreach (MeshRenderer meshRenderer in _meshRenderers)
		{
			meshRenderer.enabled = value;
		}
	}

	public void Activate()
	{
		_audioSource.volume = _initialVolume;
		_deathEndingTimestamp = 0f;
		EnableCollidersAndMeshes(true);
		gameObject.SetActive(true);
	}

	public void Deactivate()
	{
		EnableCollidersAndMeshes(false);
		_rigidbody.velocity = Vector3.zero;
		deathVFX.Play();
		AudioSource.PlayClipAtPoint(deathSoundClip, transform.position, deathSoundVolume);
		StartCoroutine(DisableAfterDeath());
	}

	IEnumerator DisableAfterDeath()
	{
		_deathEndingTimestamp = Time.timeSinceLevelLoad + _deathDuration;
		yield return new WaitForSeconds(_deathDuration);
		gameObject.SetActive(false);
	}
}
