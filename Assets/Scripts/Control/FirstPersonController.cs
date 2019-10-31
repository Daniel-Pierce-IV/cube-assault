using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Control
{
	[RequireComponent(typeof(FirstPersonMovement))]
	public class FirstPersonController : MonoBehaviour
	{
		private FirstPersonMovement firstPersonMovement;

		// Start is called before the first frame update
		void Start()
		{
			firstPersonMovement = GetComponent<FirstPersonMovement>();
		}

		// Update is called once per frame
		void Update()
		{
			firstPersonMovement.HandleInput(GetMovementInput());
		}

		private Vector3 GetMovementInput()
		{
			// TODO playtest this version versus normalized version
			return new Vector3(
				Input.GetAxisRaw("Horizontal"),
				0,
				Input.GetAxisRaw("Vertical"));
		}
	}
}
