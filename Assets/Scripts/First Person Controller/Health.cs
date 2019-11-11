using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
	private bool _isAlive = true;

	public bool IsAlive()
	{
		return _isAlive;
	}

	public void TakeDamage()
	{
		if (IsAlive())
		{
			_isAlive = false;
		}

		GameObject.FindObjectOfType<StateController>().SetGameState(
			StateController.GameState.Stopped);
	}
}
