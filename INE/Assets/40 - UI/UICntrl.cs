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

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text xpText;

    [SerializeField] private GameObject histComponent;
    [SerializeField] private TMP_Text hintText;

    private int whichHint = -1;

    private string[] hints =
    {
        "You'll need to find a weapon, run over a rune to get one.",
        "Once you get a weapon, click '1', '2' or '3' to use as a weapon trigger.",
        "Weapons have different capabilities, use them and see.",
        "With each kill of a skeleton, you gain experience points.",
        "Click and the player will stop at the click position.", 
        "Draw the mouse and the player will follow the cursor."
    };

    private int hours = 0;
    private int minutes = 0;
    private int seconds = 0;

    private int kills = 0;
    private int xp = 0;

    // Start is called before the first frame update
    void Start()
    {
        EmptySlider(0);
        EmptySlider(1);
        EmptySlider(2);

        MainMenuDisplay();
    }

    public void DisplayHint()
    {
        if (whichHint == -1)
        {
            whichHint = 0;
        } else
        {
            whichHint = Random.Range(0, hints.Length);
        }

        hintText.text = hints[whichHint];

        StartCoroutine(DisplayHintComponent());
    }

    private IEnumerator DisplayHintComponent()
    {
        histComponent.SetActive(true);
        yield return new WaitForSecondsRealtime(5.0f);
        histComponent.SetActive(false);
    }

    public void DisplayXpKills(int value)
    {
        killsText.text = (++kills).ToString();
        xp += value;
        xpText.text = xp.ToString();
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

        StartCoroutine(DisplayTimer());
    }

    /**
     * DisplayTimer() - 
     */
    public IEnumerator DisplayTimer()
    {
        hours = 0;
        minutes = 0;
        seconds = 0;

        while(true)
        {
            yield return new WaitForSecondsRealtime(1.0f);

            timeText.text = FormatTimer();
        }
    }

    private string FormatTimer()
    {
        if (seconds == 59)
        {
            seconds = 0;

            if (minutes == 59)
            {
                minutes = 0;
                ++hours;
            }
            else
            {
                minutes++;
            }
        }
        else
        {
            seconds++;
        }

        return (hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2"));
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
        EventManager.Instance.OnDisplayXpKills += DisplayXpKills;
        EventManager.Instance.OnHintDisplay += DisplayHint;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnWeaponUpdate -= UpdateWeaponSlider;
        EventManager.Instance.OnWeaponReload -= UpdateWeaponReload;
        EventManager.Instance.OnSetWeapon -= SetWeapon;
        EventManager.Instance.OnDisplayXpKills -= DisplayXpKills;
        EventManager.Instance.OnHintDisplay -= DisplayHint;
    }
}
