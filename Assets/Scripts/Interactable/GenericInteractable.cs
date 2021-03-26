using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteractable : MonoBehaviour
{
	public UnityEvent onInteractEvent;

	public void Interact()
	{
            onInteractEvent.Invoke();
	}
}
