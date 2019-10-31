using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Control
{
	public class FirstPersonAiming : MonoBehaviour
	{
		[SerializeField] private Camera camera;
		[SerializeField] private float sensitivity = 200f;

		private float curPitch = 0f;
		private float maxPitch = 90f; // Straight down
		private float minPitch = -90f; // Straight up

		private const float UP_ROTATION_ANGLE = 270f;
		private const float DOWN_ROTATION_ANGLE = 90f;

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
			float yaw = mouseInputX * sensitivity * Time.deltaTime;
			transform.Rotate(0, yaw, 0);
		}

		private void UpdatePitch(float mouseInputY)
		{
			// Invert mouse y for intuitive mouse aiming
			float pitch = -mouseInputY * sensitivity * Time.deltaTime;
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
				camera.transform.Rotate(pitch, 0, 0);
			}
		}

		private void SetCameraEulerPitch(float clampAngle)
		{
			Vector3 eulerRotation = camera.transform.eulerAngles;
			eulerRotation.x = clampAngle;
			camera.transform.eulerAngles = eulerRotation;
		}
	}
}
