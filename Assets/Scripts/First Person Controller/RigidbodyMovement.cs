using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyMovement : MonoBehaviour
	{
		[SerializeField] private float speed = 10f;

		private Rigidbody _rigidbody;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			Vector3 direction = transform.forward * Input.GetAxisRaw("Vertical");
			direction += transform.right * Input.GetAxisRaw("Horizontal");

			// Changing velocity has no effect on the rigidbody or
			// gameobject transform until the next FixedUpdate() is called
			_rigidbody.velocity = direction * speed;
		}
	}
}
