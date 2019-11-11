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
}
