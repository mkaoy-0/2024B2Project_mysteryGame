using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemTrigger : MonoBehaviour
{
    // 全アイテムの入手状況
    public static bool[] getItems = new bool[11];

    // =========================
    // スプライト画像の番号と同じ
    // --------------------------
    // 0 : 壺
    // 1 : ハンカチ
    // 2 : ゴミ箱
    // 3 : DM
    // 4 : 靴
    // 5 : 金
    // 6 : メモ
    // 7 : 遺言書
    // 8 : 遺体
    // 9 : ネックレス
    // 10 : 金庫
    // ==========================

    // アイテムリストに表示させる用に、入手したアイテムをリストにして保管しておく
    public static List<int> getItemList = new List<int>();

    // アイテムをチェックしたか
    public static bool[] checkedItem = new bool[14];

    // 全アイテム入手
    public static bool allItemsAreTrue = false;

    // ============================================================
    // アイテムを入手したかのフラグ
    // 配列の特定のインデックスにアクセスするプロパティ
    public static bool Get_getItems(int index)
    {
        if (index >= 0 && index < getItems.Length)
        {
            return getItems[index];
        }
        else
        {
            return false; // インデックスが範囲外の場合
        }
    }
    // 配列の特定のインデックスに値を設定するメソッド
    public static void Set_getItems(int index, bool value)
    {
        if (index >= 0 && index < getItems.Length)
        {
            getItems[index] = value;
            Debug.Log($"アイテム入手: {index}");
        }
    }

    // ==============================================================----
    // 入手したアイテムをリストに追加
    public static void AddItem(int itemIndex)
    {
        if (!getItemList.Contains(itemIndex))
        {
            getItemList.Add(itemIndex);
            Debug.Log($"アイテム追加: {itemIndex}");
        }
    }

}
