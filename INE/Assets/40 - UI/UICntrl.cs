using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICntrl : MonoBehaviour
{
    [SerializeField] private Slider[] weaponSlider;
    [SerializeField] private TMP_Text[] weaponSliderText;
    [SerializeField] private Image[] weaponIcon;
    [SerializeField] private Image[] weaponSliderColor;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePlayPanel;

    // Start is called before the first frame update
    void Start()
    {
        EmptySlider(0);
        EmptySlider(1);
        EmptySlider(2);

        MainMenuDisplay();
    }

    public void MainMenuDisplay()
    {
        mainMenuPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void GamePlayDisplay()
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }

    private void SetWeapon(int slot, Sprite sprite, int maxRounds)
    {
        weaponSliderColor[slot].color = Color.red;
        weaponSlider[slot].value = 1.0f;
        weaponSliderText[slot].text = maxRounds.ToString() + "/" + maxRounds.ToString();
        weaponIcon[slot].sprite = sprite;
    }

    private void EmptySlider(int slot)
    {
        weaponSlider[slot].value = 0.0f;
        weaponSliderText[slot].text = "";
        //weaponIcon[slot].color = Color.black;
    }

    private void UpdateWeaponSlider(int slot, int round, int maxRounds)
    {
        weaponSliderColor[slot].color = Color.red;
        weaponSlider[slot].value = (float) round / maxRounds;
        weaponSliderText[slot].text = round.ToString() + "/" + maxRounds.ToString();
    }

    private void UpdateWeaponReload(int slot, float value)
    {
        weaponSliderColor[slot].color = Color.yellow;
        weaponSlider[slot].value = value;
        weaponSliderText[slot].text = "";
    }

    private void OnEnable()
    {
        EventManager.Instance.OnWeaponUpdate += UpdateWeaponSlider;
        EventManager.Instance.OnWeaponReload += UpdateWeaponReload;
        EventManager.Instance.OnSetWeapon += SetWeapon;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnWeaponUpdate -= UpdateWeaponSlider;
        EventManager.Instance.OnWeaponReload -= UpdateWeaponReload;
        EventManager.Instance.OnSetWeapon -= SetWeapon;
    }
}
