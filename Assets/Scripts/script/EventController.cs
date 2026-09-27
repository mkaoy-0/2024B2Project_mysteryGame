using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    // =================================
    // イベントを一括管理
    // センサの値をfalseに戻すタイミングで競合がおきないようにするため
    // =================================

    // 
    public sensorTrigger _sensorTrigger;

    // スクリプト取得
    public GetItemSelect getItemSelect;
    public ShowItemList showItemList;
    public ItemListSelect itemListSelect;

    void Start()
    {
        getItemSelect = this.GetComponent<GetItemSelect>();
        showItemList = this.GetComponent<ShowItemList>();
        itemListSelect = this.GetComponent<ItemListSelect>();
    }

    void Update()
    {
        // アイテム一覧が非表示のときのみ
        // アイテム取得操作可能
        if (!showItemList.IsActive)
        {
            getItemSelect.SelectIcon();
        }
        else // アイテム一覧が表示中のとき
        {
            // アイテムを一個以上入手していたら
            if (GetItemTrigger.getItemList.Count > 0)
            {
                itemListSelect.SelectItem();
            }
        }

        // 一覧表示
        showItemList.OnButtonClicked();

        // センサの値をfalseに戻す
        ResetSensoeBool();
    }

    // センサの値をfalseに戻す
    public void ResetSensoeBool()
    {
        if (_sensorTrigger.PressW) _sensorTrigger.PressW = false;
        if (_sensorTrigger.PressS) _sensorTrigger.PressS = false;
        if (_sensorTrigger.PressA) _sensorTrigger.PressA = false;
        if (_sensorTrigger.PressD) _sensorTrigger.PressD = false;
        if (_sensorTrigger.PressSubmit) _sensorTrigger.PressSubmit = false;
        if (_sensorTrigger.PressSubmit2) _sensorTrigger.PressSubmit2 = false;
        if (_sensorTrigger.PressSubmit3) _sensorTrigger.PressSubmit3 = false;
        if (_sensorTrigger.PressSubmit4) _sensorTrigger.PressSubmit4 = false;
    }
}
