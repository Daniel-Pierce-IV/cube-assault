using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
	[Tooltip("Length of time (in seconds) before there is no more progression.")]
	[SerializeField] private float duration = 300f;

	public float Percent()
	{
		return Mathf.Clamp01(Time.timeSinceLevelLoad / duration);
	}

	// Returns the current value, based on the percentage of progression
	// between the initial and final values
	public int CurrentValue(int initialValue, int finalValue)
	{
		int difference = finalValue - initialValue;
		int toAdd = Mathf.RoundToInt(difference * Percent());
		return initialValue + toAdd;
	}
}
