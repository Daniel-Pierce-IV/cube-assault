using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Controller
{
	public class FirstPersonWeaponSystem : MonoBehaviour
	{
		[SerializeField] private Weapon primaryWeapon;
		[SerializeField] private Weapon secondaryWeapon;
		[SerializeField] private KeyCode primaryKey = KeyCode.Mouse0;
		[SerializeField] private KeyCode secondaryKey = KeyCode.Mouse1;

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKey(primaryKey) && !secondaryWeapon.IsActive())
			{
				primaryWeapon.Activate();
			}
			else if(Input.GetKeyUp(primaryKey))
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

