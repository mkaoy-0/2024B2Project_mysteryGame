using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBookshelf : MonoBehaviour
{
    // 本棚調べたか判定するスクリプト
    public GetItemTextController getItemTextController;
    // アイコンとかの管理スクリプト
    public GetItemSelect getItemSelect;

    // 背景表示オブジェクト
    public Image BG;

    // 変更後の背景
    public Sprite newMainBg;

    // 変更前のアイコン（非表示にするため）
    public GameObject prevIcons;

    // 変更後のアイコン
    public GameObject newIcons;
    public GameObject[] newIcon;
    // 変更後のアイコンに対応するアイテム番号
    public int[] newIconToItem;

    // 変更した
    public bool isChenged = false;



    void Start()
    {
        getItemTextController = this.GetComponent<GetItemTextController>();
        getItemSelect = this.GetComponent<GetItemSelect>();

        IconSettings();
      //  ChengeBgAndIcons();
    }

    void Update()
    {
        ChengeBgAndIcons();
    }

    // 各アイコン取得
    public void IconSettings()
    {
        int iconNum = newIcons.transform.childCount;
        newIcon = new GameObject[iconNum];
        for (int i = 0; i < iconNum; i++)
        {
            newIcon[i] = newIcons.transform.GetChild(i).gameObject;
        }
    }

    public void ChengeBgAndIcons()
    {
        // 本棚を調べたなら
        if (GetItemTextController.checkedBookshelf && !getItemTextController.DialogueSet.activeSelf && !isChenged)
        {
            // 背景変更
            BG.sprite = newMainBg;
            // アイコン変更
            getItemSelect.icons = newIcons;
            getItemSelect.icon = newIcon;
            getItemSelect.iconToItem = newIconToItem;
          
            // アイコン表示変更
            if (prevIcons.activeSelf) prevIcons.SetActive(false);
            if (!newIcons.activeSelf) newIcons.SetActive(true);

            getItemSelect.UpdateIconAnim(getItemSelect.currentFocus);

            isChenged = true;
        }
    }
}
