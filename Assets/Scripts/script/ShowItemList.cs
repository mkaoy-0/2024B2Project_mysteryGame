using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowItemList : MonoBehaviour
{
    // =================================-
    // 入手したアイテム一覧を表示
    // =================================

    // 
    public sensorTrigger _sensorTrigger;

    // アイテムリストの親オブジェクト
    public GameObject ItemListSet;
    private Image ItemListBgPanel; // リストの後ろに表示させるパネル
    public GameObject ItemList; // メインのアイテム一覧
    public TextMeshProUGUI header; // アイテム一覧のヘッダーテキスト
    public GameObject ShowItemListButton; // アイテム一覧を表示させるボタン

    public Transform getItemList; // 入手したアイテムを足していく親オブジェクト
    [Header("prefabからgetItem_ItemListをアタッチ")]
    public GameObject getItem_ItemList; // 追加するアイテム

    // 一覧を表示するか非表示にするか
    public bool isActive = false;

    void Start()
    {
        // 初期化
        Settings();
        // 入手したアイテムがあれば、シーンロード時に一覧に追加
        if (GetItemTrigger.getItemList.Count > 0)
        {
            AddAllItemToList();
        }
    }

    void Update()
    {
    }

    // 初期設定。オブジェクト取得
    public void Settings()
    {
        if (ItemListSet != null)
        {
            ItemListBgPanel = ItemListSet.transform.GetChild(0).gameObject.GetComponent<Image>(); ;
            ItemList = ItemListSet.transform.GetChild(1).gameObject;
            header = ItemList.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            ShowItemListButton = ItemListSet.transform.GetChild(2).gameObject;

            getItemList = ItemList.transform.GetChild(2);
        }
    }

    // キーを押したらアイテムリスト表示非表示切り替え
    public void OnButtonClicked()
    {
        // とりあえずsubmit2(Iキー)で切り替え
        if (_sensorTrigger.PressSubmit2)
        {
            isActive = !isActive;
        }

        if (isActive)
        {
            ActivateItemList();
        }
        else
        {
            InactivateItemList();
        }
    }
    public void ActivateItemList() // 表示
    {
        if (!ItemListBgPanel.enabled) ItemListBgPanel.enabled = true; // 背景パネル表示
        if (!ItemList.activeSelf) ItemList.SetActive(true); //  アイテム一覧表示
    }
    public void InactivateItemList() // 非表示
    {
        if (ItemListBgPanel.enabled) ItemListBgPanel.enabled = false; // 背景パネル非表示
        if (ItemList.activeSelf) ItemList.SetActive(false); //  アイテム一覧非表示
    }

    // ==========================================-
    // 入手したアイテムを追加
    // GetItemTextControllerでアイテム入手したときの処理のところに書く
    public void AddItemToList(int itemIndex)
    {
        // アイテムプレハブを生成して親オブジェクトに配置
        GameObject newItem = Instantiate(getItem_ItemList, getItemList);

        // 生成したアイテムの画像設定（必要に応じて）
        Image itemImage = newItem.transform.GetChild(0).GetComponent<Image>();
        itemImage.sprite = ItemSpriteAndText.itemSprites[itemIndex];
    }

    // シーン起動時に、リストに入っているアイテムを全て一覧に追加
    public void AddAllItemToList()
    {
        for (int i = 0; i < GetItemTrigger.getItemList.Count; i++)
        {
            // アイテムプレハブを生成して親オブジェクトに配置
            GameObject newItem = Instantiate(getItem_ItemList, getItemList);
            // 画像設定
            Image itemImage = newItem.transform.GetChild(0).GetComponent<Image>();
            itemImage.sprite = ItemSpriteAndText.itemSprites[GetItemTrigger.getItemList[i]];
        }
    }


    // ============================================--
    // 変数受け渡し
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
}
