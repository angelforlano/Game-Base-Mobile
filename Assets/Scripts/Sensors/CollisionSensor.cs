using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionSensor : MonoBehaviour
{
	public int activeMagnitud;
      public UnityEvent onCollisionMagnitude;

	void OnCollisionEnter(Collision other)
	{
		if (other.relativeVelocity.magnitude > activeMagnitud)
		{
                  onCollisionMagnitude.Invoke();
		}
	}
}