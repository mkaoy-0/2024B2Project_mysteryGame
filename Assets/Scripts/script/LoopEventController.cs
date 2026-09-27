using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopEventController : MonoBehaviour
{
    // ===============================-
    // ループイベント処理
    // ==============================

    // スクリプト取得
    public sensorTrigger _sensorTrigger;
    private StoryTextController storyTextController;
    public ItemListSelect itemListSelect;

    private int showCommandNum; // イベント発生の行を保存
    private int startLoopNum = 0; // ループの開始地点（最初の行）
    private int currentLoopNum; // 現在の行
    private bool setNum = false;

    // オブジェクト取得
    public GameObject ItemListSet;
    private Image ItemListPanel;
    public GameObject ItemList;
    public GameObject Button; // 提出ボタン


    // 一覧表示中かどうか
    public bool ListIsAvtive = false;

    public bool correct = false;
    public bool wrong = false;

    void Start()
    {
        storyTextController = this.GetComponent<StoryTextController>();

        if (ItemListSet != null)
        {
            ItemListPanel = ItemListSet.transform.GetChild(0).GetComponent<Image>();
        }
    }

    // ループ

    public void Event_Loop()
    {
        // loopNum が 0 の位置を特定 
        SetLoopNum();

        if (!storyTextController.storyWrongChoice.missedTextIsPlaying)
        {
            correct = false;
            wrong = false;
            if (!ListIsAvtive)
            {
                LoopText();
            }

            // ボタン表示
            ActivateButtons();

            // Kキーで一覧表示非表示切り替え
            if (_sensorTrigger.PressSubmit3)
            {
                ListIsAvtive = !ListIsAvtive;
            }
            _sensorTrigger.PressSubmit3 = false;

        }
        ShowPresentItemPanel();
        _sensorTrigger.PressSubmit = false;
    }

    public void SetLoopNum()
    {
        if (!setNum)
        {
            showCommandNum = storyTextController.talkNum;
            startLoopNum = showCommandNum - int.Parse(storyTextController.storyTextData[showCommandNum].loopNum);
            currentLoopNum = startLoopNum;
            setNum = true;
        }
    }

    // テキスト更新
    public void LoopText()
    {
        if (_sensorTrigger.PressD)
        {
            // 進める範囲は showCommand の行まで
            if (currentLoopNum + 1 < storyTextController. storyTextData.Length && storyTextController.storyTextData[currentLoopNum].flag != "showCommand")
            {
                currentLoopNum++;
            }
        }
        _sensorTrigger.PressD = false;

        if (_sensorTrigger.PressA)
        {
            // 戻れる範囲は loopNum == 0 の行まで
            if (currentLoopNum - 1 >= startLoopNum)
            {
                currentLoopNum--;
            }
        }
        _sensorTrigger.PressA = false;
        UpdateText(currentLoopNum); // テキスト更新

    }
    void UpdateText(int index)
    {
        storyTextController.Text.text = storyTextController.storyTextData[index].text;
        storyTextController.Text.maxVisibleCharacters = storyTextController.storyTextData[index].text.Length;

        storyTextController.ChangeCharaImg(index);
    }


    // ボタン表示非表示を切り替え
    public void ActivateButtons()
    {
        // 吹き出し下の「突きつける」ボタン表示
        if (storyTextController.storyTextData[currentLoopNum].flag != "showCommand")
        {
            if (!storyTextController.submitButton.activeSelf) storyTextController.submitButton.SetActive(true);
        }
        else
        {
            if (storyTextController.submitButton.activeSelf) storyTextController.submitButton.SetActive(false);
        }

        // テキスト矢印表示
        // 戻る矢印
        if (currentLoopNum > startLoopNum)
        {
            if (!storyTextController.prevTextButton.activeSelf) storyTextController.prevTextButton.SetActive(true);
        }
        else
        {
            if (storyTextController.prevTextButton.activeSelf) storyTextController.prevTextButton.SetActive(false);
        }
        // 進む矢印
        if (currentLoopNum < showCommandNum && storyTextController.storyTextData[currentLoopNum].flag != "showCommand")
        {
            if (!storyTextController.nextTextButton.activeSelf) storyTextController.nextTextButton.SetActive(true);
        }
        else
        {
            if (storyTextController.nextTextButton.activeSelf) storyTextController.nextTextButton.SetActive(false);
        }
    }

    // 一覧表示
    public void ShowPresentItemPanel()
    {
        if (ListIsAvtive) // 表示
        {

            // ヘッダーテキスト変更
            itemListSelect.showItemList.header.text = "";

            if (!ItemListPanel.enabled) ItemListPanel.enabled = true;
            if (!ItemList.activeSelf) ItemList.SetActive(true);
            if (!Button.activeSelf) Button.SetActive(true);

            SubmitItem();
        }
        else // 非表示
        {
            if (ItemListPanel.enabled) ItemListPanel.enabled = false;
            if (ItemList.activeSelf) ItemList.SetActive(false);
            if (Button.activeSelf) Button.SetActive(false);
        }

        if (_sensorTrigger.PressSubmit)
        {
            if (correct) Correct();
            if (wrong) Missed();
        }
    }

    // アイテム提出
    public void SubmitItem()
    {
        // 左右キーで選択
        itemListSelect.SelectItem();
        // その際に使うキーをfalseに戻しておく
        _sensorTrigger.PressA = false;
        _sensorTrigger.PressD = false;


        // 決定
        if (_sensorTrigger.PressSubmit)
        {
            // 正解なら
            if (storyTextController.storyTextData[currentLoopNum].selectionAnswer　!= null
                && GetItemTrigger.getItemList[itemListSelect.currentItem] == int.Parse(storyTextController.storyTextData[currentLoopNum].selectionAnswer))
            {
               // Correct();
                correct = true;
            }
            else // 不正解なら
            {
               // Missed();
                wrong = true;
            }
        }
    }

    public void Correct()
    {
        // 選択肢非表示
        ListIsAvtive = false;
        // 「突きつける」ボタン非表示
        if (storyTextController.submitButton.activeSelf) storyTextController.submitButton.SetActive(false);
        // 矢印非表示
        if (storyTextController.prevTextButton.activeSelf) storyTextController.prevTextButton.SetActive(false);
        if (storyTextController.nextTextButton.activeSelf) storyTextController.nextTextButton.SetActive(false);

        // 
        if (Button.activeSelf) Button.SetActive(false);

        correct = false;
        storyTextController.talkNum = showCommandNum;
        setNum = false;
        storyTextController.correctAnswer = true; // collectAnswer を true にする
        storyTextController.loopStart = false;
    }

    public void Missed()
    {
        // 選択肢非表示
        ListIsAvtive = false;
        // 「突きつける」ボタン非表示
        if (storyTextController.submitButton.activeSelf) storyTextController.submitButton.SetActive(false);
        // 矢印非表示
        if (storyTextController.prevTextButton.activeSelf) storyTextController.prevTextButton.SetActive(false);
        if (storyTextController.nextTextButton.activeSelf) storyTextController.nextTextButton.SetActive(false);

        storyTextController.storyWrongChoice.missedTextIsPlaying = true;
        storyTextController.storyWrongChoice.ShowMissedText(storyTextController.storyTextData[currentLoopNum].miss);
    }

}
