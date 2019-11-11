﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private float fireRate = 0.5f;
		[SerializeField] private float bulletSpeed = 50f;

		[Tooltip("Number of bullets shot per trigger pull.")]
		[SerializeField] private int bulletCount = 1;

		[Range(0f, 1f)]
		[SerializeField] private float bulletSpreadIntensity = 0f;

		[SerializeField] private GameObject bulletPrefab;
		[SerializeField] private GameObject spawnZone;

		private bool _isFiring = false;
		private bool _isOnCooldown = false;

		public void Activate()
		{
			_isFiring = true;
			Fire();
		}

		public void Deactivate()
		{
			_isFiring = false;
		}

		public bool IsActive()
		{
			return _isFiring;
		}

		private void Fire()
		{
			if (!_isOnCooldown)
			{
				for (int i = 0; i < bulletCount; i++)
				{
					CreateBullet();
				}
				StartCoroutine(Cooldown());
			}
		}

		private void CreateBullet()
		{
			GameObject bulletInstance = Instantiate(
				bulletPrefab,
				spawnZone.transform.position,
				spawnZone.transform.rotation);

			Vector3 spawnPoint = GetRandomLocalPointInZone();

			// Move the bullet to a random point in the spawn zone
			bulletInstance.transform.Translate(spawnPoint);

			// Rotate the bullet left/right depending on
			// the distance it is from the center of the spawn zone
			Vector3 bulletDir = bulletInstance.transform.position - transform.position;
			float angle = Vector3.Angle(bulletDir, transform.forward) * bulletSpreadIntensity;
			bulletInstance.transform.Rotate(Vector3.up * Mathf.Sign(spawnPoint.x) * angle);

			bulletInstance.GetComponent<Bullet>().SetSpeed(bulletSpeed);
		}

		private Vector3 GetRandomLocalPointInZone()
		{
			BoxCollider boxCollider = spawnZone.GetComponent<BoxCollider>();
			float areaHalfHeight = boxCollider.size.y / 2;
			float areaHalfWidth = boxCollider.size.x / 2;

			// NOTE: Ignore the z-depth, don't want any depth variation
			return new Vector3(
				Random.Range(-areaHalfWidth, areaHalfWidth),
				Random.Range(-areaHalfHeight, areaHalfHeight),
				0);
		}

		IEnumerator Cooldown()
		{
			_isOnCooldown = true;
			yield return new WaitForSeconds(fireRate);
			_isOnCooldown = false;
		}
	}
}
