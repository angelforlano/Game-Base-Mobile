using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSensor : MonoBehaviour
{
	public UnityEvent onPlayerEnterEvent;
      public UnityEvent onPlayerExitEvent;

	void OnTriggerEnter(Collider other)
	{
            if (other.gameObject.CompareTag("Player"))
            {
                  onPlayerEnterEvent.Invoke();
            }
	}

      void OnTriggerExit(Collider other)
      {
            if (other.gameObject.CompareTag("Player"))
            {
                  onPlayerExitEvent.Invoke();
            }
      }
}