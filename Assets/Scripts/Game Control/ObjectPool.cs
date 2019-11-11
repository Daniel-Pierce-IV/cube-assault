using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	[SerializeField] private GameObject gameObjectPrefab;

	private List<GameObject> _gameObjects = new List<GameObject>();

	public string GetPrefabTagName()
	{
		if (gameObjectPrefab.CompareTag("Untagged"))
		{
			Debug.LogWarning("Prefab '" + gameObjectPrefab.name + "' used by ObjectPool is untagged!");
		}

		return gameObjectPrefab.tag;
	}

	// Returns the first inactive game object, or
	// creates one if none were found.
	public GameObject GetOrCreate()
	{
		foreach (GameObject gameObject in _gameObjects)
		{
			if (!gameObject.activeInHierarchy)
			{
				return gameObject;
			}
		}

		return CreateObject();
	}

	private GameObject CreateObject()
	{
		GameObject newGameObject = Instantiate(gameObjectPrefab, transform);
		newGameObject.SetActive(false);
		_gameObjects.Add(newGameObject);

		return newGameObject;
	}
}
