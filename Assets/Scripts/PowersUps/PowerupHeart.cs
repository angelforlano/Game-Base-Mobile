using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHeart : MonoBehaviour
{
      public int hp;

      void OnTriggerEnter(Collider other)
      {
            if (other.gameObject.CompareTag("Player"))
            {
                  var player = other.gameObject.GetComponent<Player>();
                  player.GetHp(hp);
                  Destroy(gameObject);
            }
      }
}
