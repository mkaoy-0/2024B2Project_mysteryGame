using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GetAllItem : MonoBehaviour
{
    // =============
    // 全アイテム入手

    //
    public sensorTrigger _sensorTrigger;

    // パネル
    public GameObject getAllItemText;

    // 他の操作を制御しているスクリプト
    public GameObject eventController;

    // 
    public GameObject DialogueSet;

    // ストーリー開始フラグ
    public static bool StartStory = false;

    void Start()
    {
        // 全部チェック済みなら、シーンロード時にメッセージ表示
        if (GetItemTrigger.allItemsAreTrue)
        {
            if (!getAllItemText.activeSelf) getAllItemText.SetActive(true);
          //  _sensorTrigger.PressSubmit = false;
        }
    }

    void Update()
    {
        getAllItems();
    }

    public void getAllItems()
    {
        GetItemTrigger.allItemsAreTrue = true;

        for (int i = 0; i < GetItemTrigger.checkedItem.Length; i++)
        {
            if (!GetItemTrigger.checkedItem[i]) // もし1つでもfalseがあれば
            {
                GetItemTrigger.allItemsAreTrue = false; // フラグをfalseにする
                break; // もう確認する必要がないのでループを抜ける
            }
        }

        if (GetItemTrigger.allItemsAreTrue && !DialogueSet.activeSelf)
        {
            // すべて true だった場合の処理

            // 他の操作を受け付けなくする
            if (eventController.activeSelf)
            {
                eventController.SetActive(false);
            }

            // 画面暗転
            ActivatePanel();

        }
    }

    // 画面暗転
    public void ActivatePanel()
    {
        if (!StartStory)
        {
            StartCoroutine(ShowPanel());
        }

        if (getAllItemText.activeSelf && sensorTrigger.currentPage == 1)
        {
            if (_sensorTrigger.PressSubmit)
            {
                getAllItemText.SetActive(false);
                StartStory = true;
            }
            _sensorTrigger.PressSubmit = false;
        }

    }

    IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(0.1f);

        if (!getAllItemText.activeSelf) getAllItemText.SetActive(true);

    }
}
