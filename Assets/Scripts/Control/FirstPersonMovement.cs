using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Control
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class FirstPersonMovement : MonoBehaviour
	{
		[SerializeField] private float speed = 5f;
		private Rigidbody rigidbody;

		// Start is called before the first frame update
		void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		public void HandleInput(Vector3 movementInput)
		{
			// Using velocity instead of AddForce()
			// for its instant start/stop without acceleration
			rigidbody.velocity = CalculateNewRigidbodyVelocity(movementInput);
		}

		private Vector3 CalculateNewRigidbodyVelocity(Vector3 movementInput)
		{
			Vector3 velocity = movementInput * speed;
			velocity.y = rigidbody.velocity.y;

			return velocity;
		}
	}
}

