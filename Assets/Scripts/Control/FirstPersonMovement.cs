using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Control
{
	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonMovement : MonoBehaviour
	{
		[SerializeField] private float speed = 5f;
		private CharacterController characterController;

		// Start is called before the first frame update
		void Start()
		{
			characterController = GetComponent<CharacterController>();
		}

		public void HandleInput(Vector3 movementInput)
		{
			// Multiplying by the tranform's directions keeps its
			// orientation matched to the camera's perspective
			Vector3 moveForward = transform.forward * movementInput.z * speed;
			Vector3 moveRight = transform.right * movementInput.x * speed;

			characterController.SimpleMove(moveForward + moveRight);
		}
	}
}

