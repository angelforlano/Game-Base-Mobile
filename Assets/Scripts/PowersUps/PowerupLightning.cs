using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupLightning : MonoBehaviour
{
      public int speed;
      public int time;

      void OnTriggerEnter(Collider other)
      {
            if (other.gameObject.CompareTag("Player"))
            {
                  var player = other.gameObject.GetComponent<Player>();
                  player.AddSpeed(speed, time);
                  Destroy(gameObject);
            }
      }
}