using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCntrl : MonoBehaviour
{
    [SerializeField] private UICntrl uiCntrl;
    [SerializeField] private EnemyManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DisplayMainMenu()
    {
        uiCntrl.MainMenuDisplay();
    }

    public void DisplayGamePlay()
    {
        uiCntrl.GamePlayDisplay();

        enemyManager.StartEnemyManager();
    }
}
