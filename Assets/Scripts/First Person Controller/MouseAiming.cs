using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class MouseAiming : MonoBehaviour
	{
		[Tooltip("Degrees the player can look up/down from forward")]
		[SerializeField] private float pitchLimit = 89.9f;
		[SerializeField] private float mouseSensitivity = 400f;
		[SerializeField] private Transform pitchTransform;
		[SerializeField] private Transform yawTransform;

		private Vector2 _mouseDeltas;
		private float _pitchTotal;

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void Update()
		{
			if (GetComponent<Health>().IsAlive())
			{
				UpdateMouseDeltas();
				ClampMousePitchDelta();
				RotatePitchAndYaw();
			}
			else if(Cursor.lockState == CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		private void UpdateMouseDeltas()
		{
			_mouseDeltas = new Vector2(
				Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime,
				Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime);
		}

		private void ClampMousePitchDelta()
		{
			float previousPitchTotal = _pitchTotal;
			_pitchTotal += _mouseDeltas.y;

			if (Mathf.Abs(_pitchTotal) > pitchLimit)
			{
				_pitchTotal = Mathf.Sign(_pitchTotal) * pitchLimit;
				_mouseDeltas.y = _pitchTotal - previousPitchTotal;
			}
		}

		private void RotatePitchAndYaw()
		{
			// Vector.left used to invert pitch control
			pitchTransform.Rotate(Vector3.left, _mouseDeltas.y); 
			yawTransform.Rotate(Vector3.up, _mouseDeltas.x);
		}
	}
}
