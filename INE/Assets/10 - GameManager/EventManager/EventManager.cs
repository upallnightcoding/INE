using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager 
{
    public event Action OnEnemyDestroy = delegate { };
    public void InokeOnEnemyDestroy() => OnEnemyDestroy.Invoke();

    // Event Manager Singleton
    //------------------------
    public static EventManager Instance
    {
        get
        {
            if (aInstance == null)
            {
                aInstance = new EventManager();
            }

            return (aInstance);
        }
    }

    public static EventManager aInstance = null;
}
