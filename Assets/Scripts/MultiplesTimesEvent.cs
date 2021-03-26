using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiplesTimesEvent : MonoBehaviour
{
    public bool justOnce = true;
    public int timesToEvent = 3;

    public UnityEvent onTimesEvent;

    int times;

    public void AddTime()
    {
        if (times >= timesToEvent && justOnce) return;
        
        times++;

        if (times >= timesToEvent)
        {
            onTimesEvent.Invoke();
        }
    }
}
