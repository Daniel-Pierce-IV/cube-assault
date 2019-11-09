using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Hit Box"))
		{
			other.GetComponentInParent<IKillable>().TakeDamage();
			Destroy(gameObject);
		}
	}
}
