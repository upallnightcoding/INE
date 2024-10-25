using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager 
{
    public event Action OnEnemyDestroy = delegate { };
    public void InvokeOnEnemyDestroy() => OnEnemyDestroy.Invoke();

    public event Action<WeaponSO> OnRuneTrigger = delegate { };
    public void InvokeOnRuneTrigger(WeaponSO weapon) => OnRuneTrigger.Invoke(weapon);

    public event Action<int, int, int> OnWeaponUpdate = delegate { };
    public void InvokeOnWeaponUpdate(int slot, int round, int maxRounds) => OnWeaponUpdate.Invoke(slot, round, maxRounds);

    public event Action<int, float> OnWeaponReload = delegate { };
    public void InvokeOnWeaponReload(int slot, float fraction) => OnWeaponReload.Invoke(slot, fraction);

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
