using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl
{
	/*
	 * This class is used to lock physics simulation updates to the
	 * player's framerate, which eliminates the graphical stutter/jitter
	 * inherent to games using physics while also using a moving camera.
	 * 
	 * NOTE: If the player's framerate drops due lots of rigidbodies
	 * being influenced by physcs each update, consider removing this class
	 * and instead using the interpolated transform method of
	 * rigidbody/camera render syncing.
	 */

	public class PhysicsSimulationUpdater : MonoBehaviour
	{
		private void Awake()
		{
			Physics.autoSimulation = false;
		}

		private void LateUpdate()
		{
			Physics.Simulate(Time.deltaTime);
		}
	}
}
