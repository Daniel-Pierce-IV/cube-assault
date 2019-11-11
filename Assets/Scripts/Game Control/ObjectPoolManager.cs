using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
	[SerializeField] private ObjectPool[] _objectPools;

	public static ObjectPoolManager instance;

    void Start()
    {
		instance = this;
    }

	public ObjectPool GetObjectPoolByTag(string tagOfPrefabs)
	{
		foreach (ObjectPool objectPool in _objectPools)
		{
			if (objectPool.GetPrefabTagName() == tagOfPrefabs)
			{
				return objectPool;
			}
		}

		return null;
	}
}
