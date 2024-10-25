using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICntrl : MonoBehaviour
{
    [SerializeField] private Slider[] weaponSlider;
    [SerializeField] private TMP_Text[] weaponSliderText;
    [SerializeField] private Image[] weaponColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateWeaponSlider(int slot, int round, int maxRounds)
    {
        weaponSlider[slot].value = (float) round / maxRounds;
        weaponSliderText[slot].text = round.ToString() + "/" + maxRounds.ToString();
    }

    private void UpdateWeaponReload(int slot, float value)
    {
        weaponSlider[slot].value = value;
        weaponSliderText[slot].text = "";
    }

    private void OnEnable()
    {
        EventManager.Instance.OnWeaponUpdate += UpdateWeaponSlider;
        EventManager.Instance.OnWeaponReload += UpdateWeaponReload;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnWeaponUpdate -= UpdateWeaponSlider;
        EventManager.Instance.OnWeaponReload += UpdateWeaponReload;
    }
}
