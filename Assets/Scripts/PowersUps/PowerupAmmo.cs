using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAmmo : MonoBehaviour
{

	public int ammo;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			var player = other.gameObject.GetComponent<Player>();
			player.AddAmmo(ammo);
			Destroy(gameObject);
		}
	}
}