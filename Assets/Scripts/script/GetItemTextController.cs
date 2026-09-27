using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class GetItemTextData
{
    public string itemName;
    public string img;
    public string text;
    public string flag;
}


public class GetItemTextController : MonoBehaviour
{
    // ================================-
    // テキスト送り
    // GetItemSelectで関数を呼び出して再生
    // ==============================


    // csvファイルの指定
    [Header("_の後ろの文字を入力")]
    public string storynum;

    // 流し込む配列
    public GetItemTextData[] getItemTextData;

    private GetItemTextData[] originalItemTextData; // オリジナルデータを保持

    // 証拠画像とテキストの親オブジェクト
    public GameObject DialogueSet;
    // テキストの親オブジェクト
    private GameObject textArea; 
    private TextMeshProUGUI itemText; // アイテムのテキスト
    private GameObject nextTextButton; // セリフ送りボタン
    // 画像の親オブジェクト
    private GameObject ImgSet;
    private Image itemImg; // アイテム画像
    // アイテム入手時のテキスト親オブジェクト
    private GameObject ItemGetTextParent;
    private TextMeshProUGUI itemGetText; // アイテム入手時のテキスト

    // アイテムの画像を格納
    public Sprite[] itemSprites;


    // 今何行目を再生しているか
    private int talkNum = 0;

    // 
    public bool goToNextText = true; //文を表示し終えたか

    // 再生中かどうか
    public bool isPlaying = false;
    public bool isPlaying_img = false;


    // 入手したアイテムを一覧に追加するスクリプト
    public ShowItemList showItemList;

    // =============
    // mainシーンで本棚を調べたかどうか
    public static bool checkedBookshelf = false;

    void Start()
    {
        showItemList = this.GetComponent<ShowItemList>();
    }
    void Update()
    {

    }

    public void ReadCSVFile()
    {
        //　テキストファイルの読み込みを行ってくれるクラス
        TextAsset textasset = new TextAsset();
        //　先ほど用意したcsvファイルを読み込ませる。
        //　ファイルは「Resources」フォルダを作り、そこに入れておくこと。また"CSVTestData"の部分はファイル名に合わせて変更する。
        textasset = Resources.Load("csvFiles/GetItem_" + storynum, typeof(TextAsset)) as TextAsset;
        //　CSVSerializerを用いてcsvファイルを配列に流し込む。
        getItemTextData = CSVSerializer.Deserialize<GetItemTextData>(textasset.text);
    }

    public void StartSettings()
    {
        // csvファイル読み込み
        ReadCSVFile();

        // 各オブジェクト取得
        if (DialogueSet != null)
        {
            textArea = DialogueSet.transform.GetChild(0).gameObject;
            itemText = textArea.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            nextTextButton = textArea.transform.GetChild(3).gameObject;

            ImgSet = DialogueSet.transform.GetChild(1).gameObject;
            itemImg = ImgSet.transform.GetChild(1).gameObject.GetComponent<Image>();

            ItemGetTextParent = DialogueSet.transform.GetChild(2).gameObject;
            itemGetText = ItemGetTextParent.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        // アイテム画像スプライト取得
        itemSprites = ItemSpriteAndText.itemSprites;

        // 該当データのリストを作るときにエラーが出ないようにするための処理
        // オリジナルのデータを保存しておく
        originalItemTextData = (GetItemTextData[])getItemTextData.Clone();
    }


    // テキスト表示開始
    public void ShowGetItemText(int itemNum)
    {
        // リスト作成
        MakeList(itemNum);


        // 開始時に変数リセット
        // 繰り返し再生可能にする
        if (!DialogueSet.activeSelf)
        {
            isPlaying = true;
            talkNum = 0;
        }

        if (goToNextText)
        {
            if (isPlaying)
            {
                if (!DialogueSet.activeSelf) DialogueSet.SetActive(true);


                // アイテム画像セット
                if (itemNum < itemSprites.Length)
                {
                    itemImg.sprite = itemSprites[itemNum];
                }

                // 表示開始
                StartCoroutine(Dialogue());

                ShowItemImg();
                // アイテム未入手のとき
                if (!GetItemTrigger.Get_getItems(itemNum)) GetItemFlag();

            }

            // 最後はオブジェクト非表示
            if (!isPlaying)
            {
                DialogueSet.SetActive(false);
                if (ItemGetTextParent.activeSelf) ItemGetTextParent.SetActive(false);
                talkNum = 0;
            }
        }

        // アイテムチェックした
        GetItemTrigger.checkedItem[itemNum] = true;

    }
    // コルーチンを使って、１文字ごと表示する。
    IEnumerator Dialogue()
    {
        goToNextText = false;
        itemText.text = getItemTextData[talkNum].text;
        int length =  getItemTextData[talkNum].text.Length;
        for (int i = 0; i < length; i++)
        {
            itemText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.01f);
        }
        itemText.maxVisibleCharacters = length;

        if (itemText.maxVisibleCharacters == length)
        {
            goToNextText = true;
        }

        // 次のセリフがない場合には、全部非表示。
        if (talkNum + 1 >= getItemTextData.Length)
        {
            Debug.Log("Fin");
            isPlaying = false;
        }
        else
        {
            isPlaying = true;
        }

        // 次のセリフをセットする。
        talkNum = talkNum + 1;
    }

    // 再生するテキストのリストを作成
    public void MakeList(int itemNum)
    {
        // リストを作成し、該当するデータを手動で抽出
        List<GetItemTextData> tempList = new List<GetItemTextData>();

        for (int i = 0; i < originalItemTextData.Length; i++)
        {
            if (int.Parse(originalItemTextData[i].itemName) == itemNum)
            {
                // flag が null でない場合はスキップ（リストに追加しない）
                if (itemNum < GetItemTrigger.getItems.Length && GetItemTrigger.Get_getItems(itemNum))
                {
                    if (!string.IsNullOrEmpty(originalItemTextData[i].flag))
                    {
                        continue;
                    }
                }
                tempList.Add(originalItemTextData[i]);
            }
        }

        getItemTextData = tempList.ToArray();

        Debug.Log("再生するデータの行数：" + getItemTextData.Length);

        if (getItemTextData.Length == 0)
        {
            Debug.LogWarning("該当するアイテムテキストが見つかりません");
            return;
        }
    }
    // 画像表示
    public void ShowItemImg()
    {
        // nullじゃないときは画像表示
        if (getItemTextData[talkNum].img != null)
        {
            isPlaying_img = true;
            if (!ImgSet.activeSelf) ImgSet.SetActive(true);
        }
        else
        {
            isPlaying_img = false;
            if (ImgSet.activeSelf) ImgSet.SetActive(false);
        }
    }

    // アイテム入手
    public void GetItemFlag()
    {
        // アイテム入手フラグ
        if (getItemTextData[talkNum].flag != null)
        {
            if (int.Parse(getItemTextData[talkNum].flag) < GetItemTrigger.getItems.Length)
            {
                // アイテム入手フラグをtrueにする
                GetItemTrigger.Set_getItems(int.Parse(getItemTextData[talkNum].flag), true);
                // リストに入手したアイテムを追加
                GetItemTrigger.AddItem(int.Parse(getItemTextData[talkNum].flag));
                // 一覧にも追加
                showItemList.AddItemToList(int.Parse(getItemTextData[talkNum].flag));

                // テキスト表示場所とか変更
                if (!ItemGetTextParent.activeSelf) ItemGetTextParent.SetActive(true);
                itemGetText.text = getItemTextData[talkNum].text;
                itemText.text = "";
            }
            else if (int.Parse(getItemTextData[talkNum].flag) == 12) 
            {
                // mainシーンで本棚を調べたとき
                checkedBookshelf = true;
            }

        }

    }
}
