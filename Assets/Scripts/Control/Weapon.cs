using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private float fireRate = 0.5f;
	private ParticleSystem particleSystem;
	private bool isActive = false;
	private float cooldownTimestamp;

	private void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}

	public void Activate()
	{
		if (CanActivate())
		{
			isActive = true;
			cooldownTimestamp = Time.time + fireRate;
			particleSystem.Play();
		}
	}

	public void Deactivate()
	{
		isActive = false;
		particleSystem.Stop();
	}

	public bool IsActive()
	{
		return isActive;
	}

	public bool CanActivate()
	{
		return Time.time >= cooldownTimestamp;
	}
}
