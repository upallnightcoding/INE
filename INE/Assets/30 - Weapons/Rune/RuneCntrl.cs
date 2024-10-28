using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCntrl : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon;
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject swirl;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material runeOff;
    [SerializeField] private Material runeOn;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            StartCoroutine(ToggleRune());
        }
    }

    private IEnumerator ToggleRune()
    {
        triggered = true;
        swirl.SetActive(true);
        meshRenderer.material = runeOff;
        EventManager.Instance.InvokeOnRuneTrigger(weapon);

        yield return new WaitForSecondsRealtime(10.0f);

        meshRenderer.material = runeOn;
        swirl.SetActive(false);
        triggered = false;
    }
}
