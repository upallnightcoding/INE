using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCntrl : MonoBehaviour
{
    [SerializeField] private UICntrl uiCntrl;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PlayerCntrl playerCntrl;

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

        playerCntrl.StartGamePlay();

        enemyManager.StartEnemyManager();
    }

    public void QuitGamePlay()
    {
        Debug.Log("Quiting Game ...");
        Application.Quit();
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDisplayMainMenu -= DisplayMainMenu;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnDisplayMainMenu += DisplayMainMenu;
    }
}
