using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private float enemySpawnDelay = 5f;

	private GameObject[] _spawnPoints;
	private bool _canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
		_spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
    }

    // Update is called once per frame
    void Update()
    {
		if (_canSpawn)
		{
			StartCoroutine(SpawnEnemy());
		}
    }

	IEnumerator SpawnEnemy()
	{
		_canSpawn = false;

		GameObject spawnPoint = GetRandomSpawnPoint();
		Instantiate(
			enemyPrefab,
			spawnPoint.transform.position,
			spawnPoint.transform.rotation);

		yield return new WaitForSeconds(enemySpawnDelay);

		_canSpawn = true;
	}

	private GameObject GetRandomSpawnPoint()
	{
		int index = Random.Range(0, _spawnPoints.Length);
		return _spawnPoints[index];
	}
}
