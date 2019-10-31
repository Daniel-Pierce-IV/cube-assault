using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Control
{
	[RequireComponent(typeof(FirstPersonMovement))]
	[RequireComponent(typeof(FirstPersonAiming))]
	public class FirstPersonController : MonoBehaviour
	{
		private FirstPersonMovement firstPersonMovement;
		private FirstPersonAiming firstPersonAiming;

		// Start is called before the first frame update
		void Start()
		{
			firstPersonMovement = GetComponent<FirstPersonMovement>();
			firstPersonAiming = GetComponent<FirstPersonAiming>();

			firstPersonAiming.LockCursor();
		}

		// Update is called once per frame
		void Update()
		{
			// Handle movement before handling the camera to avoid stuttering
			firstPersonMovement.HandleInput(GetMovementInput());
			firstPersonAiming.HandleInput(GetMouseInput());
		}

		private Vector3 GetMovementInput()
		{
			// TODO playtest this version versus normalized version
			return new Vector3(
				Input.GetAxisRaw("Horizontal"),
				0,
				Input.GetAxisRaw("Vertical"));
		}

		private Vector2 GetMouseInput()
		{
			return new Vector2(
				Input.GetAxis("Mouse X"),
				Input.GetAxis("Mouse Y"));
		}
	}
}
