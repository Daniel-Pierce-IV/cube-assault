using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private float initialFireRate = 0.5f;
		[SerializeField] private float finalFireRate = 0.5f;

		[SerializeField] private float bulletSpeed = 50f;

		[Tooltip("Number of bullets shot per trigger pull.")]
		[SerializeField] private int initialBulletCount = 1;
		[SerializeField] private int finalBulletCount = 1;

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
				int numBullets = Progression.Instance().CurrentValue(
					initialBulletCount,
					finalBulletCount);

				for (int i = 0; i < numBullets; i++)
				{
					InitializeBullet();
				}

				PlayFiringSound();

				StartCoroutine(Cooldown());
			}
		}

		private void InitializeBullet()
		{
			Bullet bullet = ObjectPoolManager.instance
				.GetObjectPoolByTag(bulletPrefab.tag)
				.GetOrCreate()
				.GetComponent<Bullet>();

			bullet.transform.position = spawnZone.transform.position;
			bullet.transform.rotation = spawnZone.transform.rotation;

			// Locally move the bullet to a random point in the spawn zone
			Vector3 spawnPoint = GetRandomLocalPointInZone();
			bullet.transform.Translate(spawnPoint);

			// Rotate bullet based on angle from center of spawn zone
			Vector3 bulletDir = bullet.transform.position - transform.position;
			float angle = Vector3.Angle(bulletDir, transform.forward) * bulletSpreadIntensity;
			bullet.transform.Rotate(Vector3.up * Mathf.Sign(spawnPoint.x) * angle);

			bullet.SetSpeed(bulletSpeed);
			bullet.Activate();
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

			yield return new WaitForSeconds(
				Progression.Instance().CurrentValue(
					initialFireRate,
					finalFireRate));

			_isOnCooldown = false;
		}

		private void PlayFiringSound()
		{
			GetComponent<AudioSource>().Play();
		}
	}
}
