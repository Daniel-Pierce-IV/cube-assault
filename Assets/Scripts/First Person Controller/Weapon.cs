using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private float fireRate = 0.5f;

		private ParticleSystem _particleSystem;
		private List<ParticleCollisionEvent> _collisionEvents;
		private bool _isActive = false;
		private float _cooldownTimestamp;

		private void Start()
		{
			_particleSystem = GetComponent<ParticleSystem>();
			_collisionEvents = new List<ParticleCollisionEvent>();
		}

		public void Activate()
		{
			if (CanActivate())
			{
				_isActive = true;
				_cooldownTimestamp = Time.time + fireRate;
				_particleSystem.Play();
			}
		}

		public void Deactivate()
		{
			_isActive = false;
			_particleSystem.Stop();
		}

		public bool IsActive()
		{
			return _isActive;
		}

		public bool CanActivate()
		{
			return Time.time >= _cooldownTimestamp;
		}

		private void OnParticleCollision(GameObject other)
		{
			_particleSystem.GetCollisionEvents(other, _collisionEvents);

			foreach (ParticleCollisionEvent collisionEvent in _collisionEvents)
			{
				GameObject hitObject = collisionEvent.colliderComponent.gameObject;
				if (LayerMask.LayerToName(hitObject.layer) == "Hit Box")
				{
					Destroy(other);
				}
			}
		}
	}
}
