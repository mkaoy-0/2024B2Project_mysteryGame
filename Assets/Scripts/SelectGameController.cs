using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGameController : MonoBehaviour
{
    // =============================
    // プレイするゲームを決める
    // =============================-


    // 
    public sensorTrigger _sensorTrigger;
    
    // 
    public Transform game_parent;
    private Image game1;
    private Image game2;

    // 
    private int currentGame = 1;


    void Start()
    {
        if (game_parent != null)
        {
            game1 = game_parent.GetChild(0).GetComponent<Image>();
            game2 = game_parent.GetChild(1).GetComponent<Image>();
        }
    }

    void Update()
    {
        DecidePlayGame();
    }

    public void DecidePlayGame()
    {
        SelectGame();
        OnSubmitButtonClicked();
    }

    public void SelectGame()
    {
        if (_sensorTrigger.PressW)
        {
            currentGame = 1;
        }
        _sensorTrigger.PressW = false;

        if (_sensorTrigger.PressS)
        {
            currentGame = 2;
        }
        _sensorTrigger.PressS = false;

        // 選択中のゲームをフォーカス
        ChangeFocusGameBG();
    }

    public void OnSubmitButtonClicked()
    {
        if (_sensorTrigger.PressSubmit)
        {
            if (currentGame == 1)
            {
                Debug.Log("アクションゲーム");
            }
            else
            {
                Debug.Log("ミステリーゲーム");
            }
        }
    }

    public void ChangeFocusGameBG()
    {
        if (currentGame == 1)
        {
            game1.color = new Color32(180, 180, 180, 170);
            game2.color = new Color32(180, 180, 180, 0);
        }
        else
        {
            game1.color = new Color32(180, 180, 180, 0);
            game2.color = new Color32(180, 180, 180, 170);
        }
    }

}
