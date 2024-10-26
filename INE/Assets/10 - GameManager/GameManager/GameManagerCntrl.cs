using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCntrl : MonoBehaviour
{
    [SerializeField] private UICntrl uiCntrl;

    // Start is called before the first frame update
    void Start()
    {
        //DisplayMainMenu();
    }

    public void DisplayMainMenu()
    {
        uiCntrl.MainMenuDisplay();
    }

    public void DisplayGamePlay()
    {
        uiCntrl.GamePlayDisplay();
    }
}
