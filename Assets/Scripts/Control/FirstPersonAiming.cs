using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Control
{
	public class FirstPersonAiming : MonoBehaviour
	{
		[SerializeField] private float lookSensitivity = 200f;

		private Camera childCamera;

		private float curPitch = 0f;
		private float maxPitch = 90f; // Straight down
		private float minPitch = -90f; // Straight up

		private const float UP_ROTATION_ANGLE = 270f;
		private const float DOWN_ROTATION_ANGLE = 90f;

		private void Start()
		{
			childCamera = GetComponentInChildren<Camera>();
		}

		public void LockCursor()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void HandleInput(Vector2 mouseInput)
		{
			UpdateYaw(mouseInput.x);
			UpdatePitch(mouseInput.y);
		}

		private void UpdateYaw(float mouseInputX)
		{
			float yaw = mouseInputX * lookSensitivity * Time.deltaTime;
			transform.Rotate(0, yaw, 0);
		}

		private void UpdatePitch(float mouseInputY)
		{
			// Invert mouse y for intuitive mouse aiming
			float pitch = -mouseInputY * lookSensitivity * Time.deltaTime;
			curPitch += pitch;

			// Custom clamp for Unity rotation angles
			if(curPitch > maxPitch || curPitch < minPitch)
			{
				if(curPitch > maxPitch)
				{
					curPitch = maxPitch;
					SetCameraEulerPitch(DOWN_ROTATION_ANGLE);
				}
				else
				{
					curPitch = minPitch;
					SetCameraEulerPitch(UP_ROTATION_ANGLE);
				}
			}
			else
			{
				childCamera.transform.Rotate(pitch, 0, 0);
			}
		}

		private void SetCameraEulerPitch(float clampAngle)
		{
			Vector3 eulerRotation = childCamera.transform.eulerAngles;
			eulerRotation.x = clampAngle;
			childCamera.transform.eulerAngles = eulerRotation;
		}
	}
}
