using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpriteAndText : MonoBehaviour
{
    // ======================
    // アイテムの画像と、一覧画面で表示するテキストを格納
    // =======================

    // 画像
    public static Sprite[] itemSprites = Resources.LoadAll<Sprite>("itemImg");
    // テキスト
    public static string[] itemTexts = new string[]
    {
        "書斎にあった壺。何かが拭き取られた痕跡がある。",
        "ルーシーの部屋にあったハンカチ。血が付着している。",
        "ルーシーの部屋のゴミ箱には、政略結婚に関する資料が捨ててあった。",
        "遺体のそばに書いてあった文字。少し違和感があるような…。",
        "ディランの部屋にあった靴。底面に血が付着している。",
        "ディランの部屋にあった棚。中には大量のお金が入っていた。",
        "ジャックの部屋の床に落ちていたメモ。何かの番号が書いてある。",
        "ジャックの部屋の暖炉に捨ててあった手紙。破られていて内容までは分からない。",
        "被害者の死因は、後頭部を強打したことによる失血死だ。おそらく即死だっただろう。",
        "遺体の下あたりに落ちていたネックレス。チャームが付いている",
        "書斎にあったダイヤル金庫。中には何も入っていなかった。"
    };

    public static string Get_itemTexts(int index)
    {
        if (index >= 0 && index < itemTexts.Length)
        {
            return itemTexts[index];
        }
        else
        {
            return ""; // インデックスが範囲外の場合
        }
    }
}
