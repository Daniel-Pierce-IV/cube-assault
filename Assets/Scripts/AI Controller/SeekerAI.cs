﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIController
{
	/*
	 * Attempts to collide with the player by rotating its forward axis toward
	 * the player over time, and moving forward at a constant velocity.
	 * 
	 * Will also randomly increase/decrease height
	 */

	[RequireComponent(typeof(Rigidbody))]
	public class SeekerAI : MonoBehaviour, IDamageable, IPoolable
	{
		[SerializeField] private float forwardSpeed = 10f;
		[SerializeField] private float verticalSpeed = 3f;

		[Tooltip("The time in seconds it takes to make a 360 degree rotation")]
		[SerializeField] private float rotationTime = 4f;

		//[Tooltip("Minimum height added to target height, as part of wave movement.")]
		[SerializeField] private float minAscent = 0f;
		//[Tooltip("Maximum height added to target height, as part of wave movement.")]
		[SerializeField] private float maxAscent = 3f;

		//[Tooltip("Minimum time to hover at target height.")]
		[SerializeField] private float minLingerTime = 1f;
		//[Tooltip("Maximum time to hover at target height.")]
		[SerializeField] private float maxLingerTime = 3f;

		[SerializeField] private AudioClip deathSoundClip;
		[SerializeField] private float deathSoundVolume = 0.5f;

		[SerializeField] private ParticleSystem deathVFX;

		private const float FULL_ROTATION = 360f;
		private const string TARGET_TAG = "Player";

		private Transform _target;
		private Rigidbody _rigidbody;
		private Collider[] _colliders;
		private MeshRenderer[] _meshRenderers;
		private AudioSource _audioSource;
		private Vector3 _verticalDirection;
		private float _heightModifier;
		private bool _isLingering;
		private float _initialVolume;
		private float _deathEndingTimestamp;
		private float _deathDuration;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_colliders = GetComponentsInChildren<Collider>();
			_meshRenderers = GetComponentsInChildren<MeshRenderer>();
			_audioSource = GetComponent<AudioSource>();
			_initialVolume = _audioSource.volume;
			_deathDuration = Mathf.Max(deathVFX.main.startLifetime.constant, deathSoundClip.length);
		}

		private void Start()
		{
			_target = GameObject.FindGameObjectWithTag(TARGET_TAG).transform;

			// TODO: Set death duration as longest length between VFX or SFX time
		}

		void Update()
		{
			if (IsDying())
			{
				// Slowly turn down volume to avoid "popping"
				_audioSource.volume -= Time.deltaTime * _deathDuration;
			}
			else
			{
				if (!_isLingering)
				{
					if (TargetHeightReached())
					{
						StartCoroutine(LingerAtCurrentHeight());
						ReverseVerticalDirection();
						UpdateHeightModifier();
					}
				}

				// NOTE: Velocity depends on Rotation, so Rotation must come first
				UpdateRigidbodyRotation();
				UpdateRigidbodyVelocity();
			}
		}

		private bool IsDying()
		{
			
			return Time.timeSinceLevelLoad < _deathEndingTimestamp;
		}

		private void UpdateRigidbodyRotation()
		{
			Quaternion targetRotation = Quaternion.LookRotation(
				GetHeightNullifiedTargetDirection(),
				Vector3.up);

			transform.rotation = Quaternion.RotateTowards(
				transform.rotation,
				targetRotation,
				FULL_ROTATION / rotationTime * Time.deltaTime);
		}

		private void UpdateRigidbodyVelocity()
		{
			Vector3 currentVelocity = Vector3.zero;
			currentVelocity += transform.forward * forwardSpeed;

			if (!_isLingering)
			{
				currentVelocity += verticalSpeed * _verticalDirection;
			}

			_rigidbody.velocity = currentVelocity;
		}

		private void ReverseVerticalDirection()
		{
			// Handled this way, instead of multiplying by -1,
			// to avoid cumulative float errors over time
			if (_verticalDirection == Vector3.up)
			{
				_verticalDirection = Vector3.down;
			}
			else
			{
				_verticalDirection = Vector3.up;
			}
		}

		private bool TargetHeightReached()
		{
			if (_verticalDirection == Vector3.up
				&& transform.position.y >= GetModifiedTargetPosition().y)
			{
				return true;
			}

			if (_verticalDirection == Vector3.down
				&& transform.position.y <= GetModifiedTargetPosition().y)
			{
				return true;
			}

			return false;
		}

		private void UpdateHeightModifier()
		{
			if (Mathf.Approximately(_heightModifier, 0f))
			{
				_heightModifier = UnityEngine.Random.Range(minAscent, maxAscent);
			}
			else
			{
				_heightModifier = 0f;
			}
		}

		private Vector3 GetModifiedTargetPosition()
		{
			return new Vector3(
				_target.position.x,
				_target.position.y + _heightModifier,
				_target.position.z);
		}

		private float GetRandomLingerTime()
		{
			return UnityEngine.Random.Range(minLingerTime, maxLingerTime);
		}

		IEnumerator LingerAtCurrentHeight()
		{
			_isLingering = true;
			yield return new WaitForSeconds(GetRandomLingerTime());
			_isLingering = false;
		}

		// Returns the direction as though the objects compared
		// were at the same elevation
		private Vector3 GetHeightNullifiedTargetDirection()
		{
			Vector3 targetPosition = _target.position;
			targetPosition.y = transform.position.y;

			return targetPosition - transform.position;
		}

		public void TakeDamage()
		{
			Deactivate();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer
				== LayerMask.NameToLayer("Player Collider"))
			{
				collision.gameObject.GetComponent<Health>().TakeDamage();
			}
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
			_isLingering = false;
			_heightModifier = 0f;
			_verticalDirection = Vector3.down;
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
}
