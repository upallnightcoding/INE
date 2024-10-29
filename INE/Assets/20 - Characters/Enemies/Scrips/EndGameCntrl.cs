using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCntrl : MonoBehaviour
{
    public void EndGame()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnEndGame += EndGame;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEndGame -= EndGame;
    }
}
