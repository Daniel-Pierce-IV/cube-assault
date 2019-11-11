using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private float _initialSpeed;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		// Ensure a constant speed, regardless of directional changes
		_rigidbody.velocity = _rigidbody.velocity.normalized * _initialSpeed;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Hit Box"))
		{
			other.GetComponentInParent<IDamageable>().TakeDamage();
			Destroy(gameObject);
		}
	}

	public void SetSpeed(float speed)
	{
		_initialSpeed = speed;
		GetComponent<Rigidbody>().velocity = transform.forward * _initialSpeed;
	}
}
