using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class StoryTextData
{
    public string loopNum;
    public string img;
    public string face;
    public string text;
    public string flag;
    public string selection0;
    public string selection1;
    public string selection2;
    public string selectionAnswer;
    public string miss;

}


public class StoryTextController : MonoBehaviour
{
    // ================================-
    // テキスト送り
    // 関数を呼び出して再生
    // ==============================

    // 
    public sensorTrigger _sensorTrigger;

    // 効果音
    public AudioSource audioSource;
    public AudioClip correctSound;

    // 流し込む配列
    public StoryTextData[] storyTextData;

    private StoryTextData[] originalstoryTextData; // オリジナルデータを保持

    // 証拠画像とテキストの親オブジェクト
    public Transform story;
    // テキストの親オブジェクト
    private GameObject textArea;
    public TextMeshProUGUI Text; // アイテムのテキスト
    public GameObject nextTextButton; // セリフ送りボタン
    public GameObject prevTextButton;
    public GameObject submitButton; // 証拠提出ボタン

    // キャラ名
    public GameObject nameTag;
    public TextMeshProUGUI charaName;

    public Image charaImg; // アイテム画像
    // アイテムの画像を格納
    public Sprite[] charaSprites;

    // 選択肢
    public GameObject selectSet;
    public GameObject selectionsParent;
    private GameObject[] selections = new GameObject[3]; // 各選択肢 



    // 今何行目を再生しているか
    public int talkNum = 0;

    // 
    public bool goToNextText = true; //文を表示し終えたか

    // 再生中かどうか
    public bool isPlaying = true;

    // イベント中
    public bool runningEvent = false;
    public bool runningEvent_select = false; // 選択肢イベント中
    public bool runningEvent_loop = false; // ループイベント中
    public bool runningEvent_presentItem = false; // アイテム選択イベント中

    // 回答が正しいか間違っているか
    public bool correctAnswer = false;
    public bool wrongAnswer = false;

    // =============================
    // どの選択肢をフォーカスしているか
    public int currentSelection = 0;

    // ループ開始
    public bool loopStart = false;

    // =====================================-
    // イベント管理スクリプト

    // アイテム一覧表示
    public ItemListControllerDuringStory itemListControllerDuring;

    // 選択肢ミスったときの処理を管理しているスクリプト
    public StoryWrongChoice storyWrongChoice;

    // ループイベント管理
    private LoopEventController loopEventController;

    // 画面暗転管理
    public SceneResetPanelController sceneResetPanelController;

    // =========================================

    // csvファイルを読み込んだか
    public bool fileReaded = false;
    public string csvFileName = "MainStory"; // 読み込むファイルの名前

    // ストーリー選択画面
    public GameObject selectStory;

    // ======================================

    void Start()
    {
        // スクリプト取得
        storyWrongChoice = this.GetComponent<StoryWrongChoice>();
        loopEventController = this.GetComponent<LoopEventController>();

        audioSource = this.GetComponent<AudioSource>();

        Settings();
      //  ReadCSVFile();
    }

    void Update()
    {
        if (GetAllItem.StartStory && fileReaded && !selectStory.activeSelf)
        {
            if (!itemListControllerDuring.showItemList.isActive)
            {
                if (_sensorTrigger.PressSubmit)
                {
                    ShowStoryText();
                }
            }
            _sensorTrigger.PressSubmit = false;

        }

    }


    // ===========================================
    // 初期設定
    public void LoadCSV(string filePath)
    {
        csvFileName = filePath;
        ReadCSVFile();
        selectStory.SetActive(false);
        if (fileReaded)
        {
            Debug.Log("読み込み完了");
            ShowStoryText();
        }
    }

    public void ReadCSVFile()
    {
        if (string.IsNullOrEmpty(csvFileName))
        {
            Debug.LogWarning("CSVファイルが選択されていません！");
            return;
        }

        //　テキストファイルの読み込みを行ってくれるクラス
        TextAsset textasset = new TextAsset();
        //　先ほど用意したcsvファイルを読み込ませる。
        //　ファイルは「Resources」フォルダを作り、そこに入れておくこと。また"CSVTestData"の部分はファイル名に合わせて変更する。
        textasset = Resources.Load("csvFiles/" + csvFileName, typeof(TextAsset)) as TextAsset;
        //　CSVSerializerを用いてcsvファイルを配列に流し込む。
        storyTextData = CSVSerializer.Deserialize<StoryTextData>(textasset.text);

        fileReaded = true;
    }
    public void Settings()
    {
        if (story != null)
        {
            textArea = story.GetChild(1).gameObject;
            Text = textArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            nextTextButton = textArea.transform.GetChild(3).gameObject;
            prevTextButton = textArea.transform.GetChild(4).gameObject;
            submitButton = textArea.transform.GetChild(6).gameObject;

            nameTag = textArea.transform.GetChild(5).gameObject;
            charaName = nameTag.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            charaImg = story.GetChild(0).GetComponent<Image>();

            selectSet = story.GetChild(2).gameObject;

        }
        if (selectionsParent != null)
        {
            selections[0] = selectionsParent.transform.GetChild(0).gameObject;
            selections[1] = selectionsParent.transform.GetChild(1).gameObject;
            selections[2] = selectionsParent.transform.GetChild(2).gameObject;
        }

        charaSprites = Resources.LoadAll<Sprite>("chara");
    }
    // ========================================================

    // 
    // テキスト表示開始
    public void ShowStoryText()
    {
        if (goToNextText)
        {
            if (isPlaying)
            {
                // 表示開始
                StartCoroutine(Dialogue());

                // 画像と名前設定
                ChangeCharaImg(talkNum);

            }
            else // 全て終わったら
            {
                sceneResetPanelController.Ending();
            }
        }
    }
    // コルーチンを使って、１文字ごと表示する。
    IEnumerator Dialogue()
    {
        goToNextText = false;
        Text.text = storyTextData[talkNum].text;
        int length = storyTextData[talkNum].text.Length;
        for (int i = 0; i < length; i++)
        {
            Text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.01f);
        }
        Text.maxVisibleCharacters = length;

        //  Debug.Log("今再生している行 | talkNum = " + talkNum);

        // ============================
        // イベントがあるとき
        if (storyTextData[talkNum].flag != null && storyTextData[talkNum].flag != "action")
        {
            //  Debug.Log("イベント発生");
            yield return StartCoroutine(Event(storyTextData[talkNum].flag));
            yield break; // 現在のコルーチンを終了する
        }
        //=============================
        // イベントがない＋全文字表示し終わったら
        else if (Text.maxVisibleCharacters == length)
        {
            goToNextText = true;
        }

        // 次のセリフがない場合には、全部非表示。
        if (talkNum + 1 >= storyTextData.Length)
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

       // _sensorTrigger.PressSubmit = false;
    }

    // ============================================================-
    // 
    // イベント発生
    // ストーリー中断
    public IEnumerator Event(string eventFlag)
    {
      //  _sensorTrigger.PressSubmit = false;
        runningEvent = true;
        goToNextText = false;   // テキスト送りを停止
        while (runningEvent)
        {
            switch (eventFlag)
            {
                case "select":
                    runningEvent_select = true;
                    Event_Selection();
                    break;

                case "showCommand":
                    runningEvent_loop = true;
                    if (_sensorTrigger.PressSubmit)
                    {
                        if (!loopStart)
                        {
                          //  StartCoroutine(sceneResetPanelController.FadePanelCoroutine());
                        }

                        loopStart = true;
                    }
                    if (loopStart)
                    {
                        loopEventController.Event_Loop();
                    }
                    break;

                case "action_auto":
                    runningEvent_presentItem = true;
                    Event_PresentItem();
                    break;

                default:
                    break;
            }

            if (correctAnswer)
            {
                audioSource.PlayOneShot(correctSound); // 効果音

                yield return new WaitForSeconds(0f);  // 必要なら待機時間を設定

                correctAnswer = false;
                runningEvent = false;  // イベントを終了
                talkNum++; // 次のセリフをセット
                goToNextText = true;     // 次のテキストへ進む

                // 各イベント中フラグをfalseにする
                runningEvent_select = false;
                runningEvent_loop = false;
                runningEvent_presentItem = false;

                /**/
                // イベント完了
                _sensorTrigger.PressSubmit = true;
                /**/
            }
            if (wrongAnswer)
            {
                yield return new WaitForSeconds(0f);  // 必要なら待機時間を設定
                wrongAnswer = false;
                runningEvent = false;  // イベントを終了
                goToNextText = true;     // 次のテキストへ進む

                /**/
                // イベント完了
                _sensorTrigger.PressSubmit = true;
                /**/
            }
            // 短い待機を入れて無限ループを防ぐ                       
            yield return null;
        }
    }

    // =============================================================
    // 画像切り替え
    public void ChangeCharaImg(int _talkNum)
    {
        if (string.IsNullOrEmpty(storyTextData[_talkNum].img))
        {
            charaImg.enabled = false;
            if (nameTag.activeSelf) nameTag.SetActive(false);
        }
        else
        {
            charaImg.enabled = true;
            if (!nameTag.activeSelf) nameTag.SetActive(true);
            switch (storyTextData[_talkNum].img)
            {
                case "ディラン":
                    charaImg.sprite = charaSprites[int.Parse(storyTextData[_talkNum].face)];
                    charaName.text = "ディラン";
                    break;
                case "ルーシー":
                    charaImg.sprite = charaSprites[int.Parse(storyTextData[_talkNum].face) + 8];
                    charaName.text = "ルーシー";
                    break;
                case "ジャック":
                    charaImg.sprite = charaSprites[int.Parse(storyTextData[_talkNum].face) + 16];
                    charaName.text = "ジャック";
                    break;
            }
        }
    }
    // ===================================================-
    //
    // 選択肢
    public void Event_Selection()
    {
        if (storyTextData[talkNum].flag == "select")
        {
            // ミス演出の最中じゃなかったら、選択肢表示
            if (!storyWrongChoice.missedTextIsPlaying)
            {
                if (!selectSet.activeSelf) selectSet.SetActive(true);
            }
            //  Debug.Log("イベント発生！" + storyWrongChoice.missedTextIsPlaying);
            SelectionSettings(); // テキスト設定
            SelectSelections(); // キーで選択
        }
    }

    // テキスト設定
    public void SelectionSettings()
    {
        selections[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = storyTextData[talkNum].selection0;
        selections[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = storyTextData[talkNum].selection1;
        selections[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = storyTextData[talkNum].selection2;

    }

    // 背景色変更
    public void ChangeFocusSelectionBG(int focusedNum)
    {
        for (int i = 0; i < 3; i++)
        {
            selections[i].GetComponent<Image>().color = new Color32(144, 82, 23, 255);
        }
        selections[focusedNum].GetComponent<Image>().color = new Color32(166, 119, 84, 255);

    }

    // 上下キーで選択肢変更
    public void SelectSelections()
    {

        // 上
        if (_sensorTrigger.PressW)
        {
            if (currentSelection > 0)
            {
                currentSelection--;
            }
            else currentSelection = 0;
        }
        _sensorTrigger.PressW = false;
        // 下
        if (_sensorTrigger.PressS)
        {
            if (currentSelection < 2)
            {
                currentSelection++;
            }
            else currentSelection = 2;
        }
        _sensorTrigger.PressS = false;
        // フォーカス中の選択肢の背景色変更
        ChangeFocusSelectionBG(currentSelection);

        // 決定
        if (_sensorTrigger.PressSubmit)
        {
            // 正解なら
            if (currentSelection == int.Parse(storyTextData[talkNum].selectionAnswer))
            {
                Correct_Selection();
            }
            else // 不正解なら
            {
                Miss_Selection();
            }
        }

    }

    // 提出した選択肢が正解のとき
    public void Correct_Selection()
    {
        // 選択肢非表示
        if (selectSet.activeSelf) selectSet.SetActive(false);
        correctAnswer = true;
    }
    // 提出した選択肢が不正解のとき
    public void Miss_Selection()
    {
        // 選択肢非表示
        if (selectSet.activeSelf) selectSet.SetActive(false);
        storyWrongChoice.missedTextIsPlaying = true;
        storyWrongChoice.ShowMissedText(storyTextData[talkNum].miss);
    }
    // ==========================================================

    // アイテム一覧のアイテムを選択するスクリプト
    public ItemListSelect itemListSelect;
    public void Event_PresentItem()
    {
        // ミス演出の最中じゃなかったら
        if (!storyWrongChoice.missedTextIsPlaying)
        {
            // アイテム一覧を表示
            itemListSelect.showItemList.ActivateItemList();
        }
        // ヘッダーテキスト変更
        itemListSelect.showItemList.header.text = storyTextData[talkNum].text;

        // 左右キーで選択
        itemListSelect.SelectItem();
        // その際に使うキーをfalseに戻しておく
        _sensorTrigger.PressA = false;
        _sensorTrigger.PressD = false;


        // 決定
        if (_sensorTrigger.PressSubmit)
        {
            // 正解なら
            if (GetItemTrigger.getItemList[itemListSelect.currentItem] == int.Parse(storyTextData[talkNum].selectionAnswer))
            {
                Correct_PresentItem();
            }
            else // 不正解なら
            {
                Miss_PresentItem();
            }
        }
    }


    // 提出した選択肢が正解のとき
    public void Correct_PresentItem()
    {
        // アイテム一覧を非表示
        itemListSelect.showItemList.InactivateItemList();
        correctAnswer = true;
    }
    // 提出した選択肢が不正解のとき
    public void Miss_PresentItem()
    {
        // アイテム一覧を非表示
        itemListSelect.showItemList.InactivateItemList();
        storyWrongChoice.missedTextIsPlaying = true;
        storyWrongChoice.ShowMissedText(storyTextData[talkNum].miss);
    }

}
