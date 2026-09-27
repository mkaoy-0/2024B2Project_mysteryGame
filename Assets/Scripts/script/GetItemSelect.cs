using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemSelect : MonoBehaviour
{
    // ================================
    // 証拠を選択して、テキストを表示させる
    // ================================

    // 
    public sensorTrigger _sensorTrigger;

    // 証拠品選択
    public GameObject icons;
    public GameObject[] icon;
    [Header("アイコンの番号に対応するアイテムの番号を指定")]
    public int[] iconToItem; // アイコンとアイテムを対応させる

    // フォーカスしているアイテム
    public int currentFocus = 0;

    // テキスト表示するスクリプト
    public GetItemTextController getItemTextController;

    // アイテム一覧を表示させるスクリプト
    // 表示中は操作不可
    public ShowItemList showItemList;

    void Start()
    {
        // アイコン取得
        IconSettings();

        // テキスト表示するスクリプト取得
        getItemTextController = this.GetComponent<GetItemTextController>();
        getItemTextController.StartSettings(); // 初期化

        // アイテム一覧を表示させるスクリプト
        showItemList = this.GetComponent<ShowItemList>();
    }

    void Update()
    {

    }


    // アイコン取得
    public void IconSettings()
    {
        int iconNum = icons.transform.childCount;
        icon = new GameObject[iconNum];
        for(int i = 0; i < iconNum; i++)
        {
            icon[i] = icons.transform.GetChild(i).gameObject;
        }
        UpdateIconAnim(currentFocus);
    }

    // 左右キーでアイテムにフォーカス
    public void SelectIcon()
    {
        if (!getItemTextController.DialogueSet.activeSelf)
        {
            if (_sensorTrigger.PressA)
            {
                if (currentFocus > 0)
                {
                    currentFocus--;
                }
                else
                {
                    currentFocus = 0;
                }
                UpdateIconAnim(currentFocus);
            }
            if (_sensorTrigger.PressD)
            {
                if (currentFocus < icon.Length - 1)
                {
                    currentFocus++;
                }
                else
                {
                    currentFocus = icon.Length - 1;
                }
                UpdateIconAnim(currentFocus);
            }
        }

        // 選択したアイコンを決定
        CheckIcon();
    }

    // 選択したアイコンを決定
    public void CheckIcon()
     {   
        // 決定
        if (_sensorTrigger.PressSubmit)
        {
            int itemIndex = iconToItem[currentFocus];
            Debug.Log(itemIndex);
            CheckFocusItem(itemIndex);          
        }


    }
    // フォーカスしているアイコンのアニメーション再生
    public void UpdateIconAnim(int focusedIndex)
    {
        for (int i = 0; i < icon.Length; i++)
        {
            Animator animator = icon[i].transform.GetChild(0).gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsFocusing", i == focusedIndex);
            }
        }
    }

    // フォーカスしているアイテムをクリック
    public void CheckFocusItem(int focusedIndex)
    {
        // 該当のテキストを表示
        getItemTextController.ShowGetItemText(focusedIndex);
    }

}
