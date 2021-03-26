using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySensor : MonoBehaviour
{
      public UnityEvent onPlayerEnterEvent;
      public UnityEvent onPlayerExitEvent;

      void OnTriggerEnter(Collider other)
      {
            if (other.gameObject.CompareTag("Enemy"))
            {
                  onPlayerEnterEvent.Invoke();
            }
      }

      void OnTriggerExit(Collider other)
      {
            if (other.gameObject.CompareTag("Enemy"))
            {
                  onPlayerExitEvent.Invoke();
            }
      }
}
