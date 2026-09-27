using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListControllerDuringStory : MonoBehaviour
{
    // =============================
    // ストーリー中のEventController
    // アイテム一覧の表示設定
    // ============================

    // 
    public sensorTrigger _sensorTrigger;

    // スクリプト取得
    public ShowItemList showItemList;
    public ItemListSelect itemListSelect;
    // ストーリー制御スクリプト
    public StoryTextController storyTextController;

    void Start()
    {

    }

    void Update()
    {
        // ループイベント、アイテム選択イベント中じゃないとき
        if (!storyTextController.runningEvent_loop && !storyTextController.runningEvent_presentItem)
        {
            // 一覧のヘッダーテキスト変更
            showItemList.header.text = "証拠一覧";

            // 一覧表示
            showItemList.OnButtonClicked();

            // 一覧を表示させるボタンも表示
            if(!showItemList.ShowItemListButton.activeSelf)
            {
                showItemList.ShowItemListButton.SetActive(true);
            }

            if (showItemList.IsActive) // アイテム一覧が表示中のとき
            {
                // アイテムを一個以上入手していたら
                if (GetItemTrigger.getItemList.Count > 0)
                {
                    itemListSelect.SelectItem();
                }
                // センサの値をfalseに戻す
                ResetSensoeBool();
            }
        }
        else // ボタン非表示
        {
            if (showItemList.ShowItemListButton.activeSelf)
            {
                showItemList.ShowItemListButton.SetActive(false);
            }
        }


        if (_sensorTrigger.PressSubmit2) _sensorTrigger.PressSubmit2 = false;

    }

    // センサの値をfalseに戻す
    public void ResetSensoeBool()
    {
        if (_sensorTrigger.PressW) _sensorTrigger.PressW = false;
        if (_sensorTrigger.PressS) _sensorTrigger.PressS = false;
        if (_sensorTrigger.PressA) _sensorTrigger.PressA = false;
        if (_sensorTrigger.PressD) _sensorTrigger.PressD = false;
        if (_sensorTrigger.PressSubmit) _sensorTrigger.PressSubmit = false;
        if (_sensorTrigger.PressSubmit3) _sensorTrigger.PressSubmit3 = false;
        if (_sensorTrigger.PressSubmit4) _sensorTrigger.PressSubmit4 = false;
    }
}
