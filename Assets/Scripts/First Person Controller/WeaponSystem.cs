using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class WeaponSystem : MonoBehaviour
	{
		[SerializeField] private Weapon primaryWeapon;
		[SerializeField] private Weapon secondaryWeapon;
		[SerializeField] private KeyCode primaryKey = KeyCode.Mouse0;
		[SerializeField] private KeyCode secondaryKey = KeyCode.Mouse1;

		// Update is called once per frame
		void Update()
		{
			if (!GetComponent<Health>().IsAlive()) return;

			if (Input.GetKey(primaryKey) && !secondaryWeapon.IsActive())
			{
				primaryWeapon.Activate();
			}
			else if (Input.GetKeyUp(primaryKey))
			{
				primaryWeapon.Deactivate();
			}

			if (Input.GetKey(secondaryKey) && !primaryWeapon.IsActive())
			{
				secondaryWeapon.Activate();
			}
			else if (Input.GetKeyUp(secondaryKey))
			{
				secondaryWeapon.Deactivate();
			}
		}
	}
}
