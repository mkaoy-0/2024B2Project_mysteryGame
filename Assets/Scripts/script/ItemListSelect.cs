using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemListSelect : MonoBehaviour
{
    // ===================-
    // アイテムリストのアイテムを選択
    // =================---

    //
    public sensorTrigger _sensorTrigger;

    // アイテム一覧を表示させるスクリプト
    public ShowItemList showItemList;

    // フォーカスしているアイテム
    public Transform focusItem;
    // 画像表示
    private Image focusItemImg;
    // テキスト表示
    private TextMeshProUGUI focusItemText;

    // 何番目にフォーカスしているか
    public int currentItem = 0;


    void Start()
    {
        showItemList = this.GetComponent<ShowItemList>();

        if (focusItem != null)
        {
            focusItemImg = focusItem.GetChild(0).GetChild(1).GetComponent<Image>();
            focusItemText = focusItem.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        
    }



    // 左右キーでアイテム操作
    public void SelectItem()
    {
        // 左
        if (_sensorTrigger.PressA)
        {
            if (currentItem > 0) currentItem--;
            else currentItem = 0;
        }
        // 右
        if (_sensorTrigger.PressD)
        {
            if (currentItem < GetItemTrigger.getItemList.Count - 1) currentItem++;
            else currentItem = GetItemTrigger.getItemList.Count - 1;
           
        }
        ChangeFocusItemBG(currentItem);
        FocusItemOnList(currentItem);
    }

    // フォーカスしているアイテムの背景色変更
    public void ChangeFocusItemBG(int index)
    {
        for(int i = 0; i < showItemList.getItemList.childCount; i++)
        {
            // 選択中のアイテムの背景色を変える
            if (i == index)
            {
                showItemList.getItemList.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(180, 180, 180, 130);
            }
            else
            {
                showItemList.getItemList.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(50, 50, 50, 130);
            }
        }       
    }

    // フォーカスしているアイテムの画像とテキストを設定
    public void FocusItemOnList(int index)
    {
        if (!focusItemImg.enabled) focusItemImg.enabled = true;
        int currentFocusItem = GetItemTrigger.getItemList[index];
        focusItemImg.sprite = ItemSpriteAndText.itemSprites[currentFocusItem];
        focusItemText.text = ItemSpriteAndText.itemTexts[currentFocusItem];
    }

}
