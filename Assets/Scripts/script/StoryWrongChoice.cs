using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MissedTextData
{
    public string img;
    public string face;
    public string text;
}

public class StoryWrongChoice : MonoBehaviour
{
    // ==================
    // ストーリーで間違えたときのテキスト再生
    // ======================

    // 
    public sensorTrigger _sensorTrigger;

    // 効果音
    public AudioSource audioSource;
    public AudioClip wrongSound;

    // 流し込む配列
    public MissedTextData[] missedTextData;

    private MissedTextData[] originalmissedTextData; // オリジナルデータを保持

    // ストーリーのテキスト管理スクリプト
    public StoryTextController storyTextController;


    // 今何行目を再生しているか
    public int talkNum = 0;

    // 
    public bool goToNextText = true; //文を表示し終えたか

    public bool finished = false;
    public bool missedTextIsPlaying = false; // 再生終了したか

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        storyTextController = this.GetComponent<StoryTextController>();
        ReadCSVFile();

    }



    // ===========================================
    // 初期設定
    public void ReadCSVFile()
    {
        //　テキストファイルの読み込みを行ってくれるクラス
        TextAsset textasset = new TextAsset();
        //　先ほど用意したcsvファイルを読み込ませる。
        //　ファイルは「Resources」フォルダを作り、そこに入れておくこと。また"CSVTestData"の部分はファイル名に合わせて変更する。
        textasset = Resources.Load("csvFiles/MissedText", typeof(TextAsset)) as TextAsset;
        //　CSVSerializerを用いてcsvファイルを配列に流し込む。
        missedTextData = CSVSerializer.Deserialize<MissedTextData>(textasset.text);
    }

    // ==========================================
    // テキスト表示開始
    public void ShowMissedText(string charaName)
    {
        int charaNum;
        switch (charaName)
        {
            case "ディラン": charaNum = 0; break;
            case "ルーシー": charaNum = 1; break;
            case "ジャック": charaNum = 2; break;
            default: charaNum = 0; break;
        }

        if (missedTextIsPlaying)
        {
            // 間違えた時用のテキスト表示
            if (goToNextText)
            {
                if (finished)
                {
                    storyTextController.wrongAnswer = true;
                    missedTextIsPlaying = false;
                    finished = false;
                    talkNum = 0; // リセット
                    Debug.Log("missedTextIsPlaying = " + missedTextIsPlaying);
                   // Debug.Log("Miss | talkNum = " + talkNum);
                    return;
                }

                StartCoroutine(Dialogue(charaNum));
                // 画像と名前設定
                ChangeCharaImg(charaNum);            
            }
        }
    }
    // コルーチンを使って、１文字ごと表示する。
    IEnumerator Dialogue(int talkCharaNum)
    {
        if (talkNum != 3) talkNum = talkCharaNum;
        else audioSource.PlayOneShot(wrongSound); // 効果音
      //  Debug.Log("ミステキスト talkNum = " + talkNum);
        goToNextText = false;
        storyTextController.Text.text = missedTextData[talkNum].text;
        int length = missedTextData[talkNum].text.Length;
        for (int i = 0; i < length; i++)
        {
            storyTextController.Text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.01f);
        }
        storyTextController.Text.maxVisibleCharacters = length;

        // 全文字表示し終わったら
        if (storyTextController.Text.maxVisibleCharacters == length)
        {
            goToNextText = true;
        }

        // 終了したら
        if (talkNum == 3)
        {
            finished = true;

        }
        else
        {
            finished = false;
            // 次のセリフをセットする。
            talkNum = 3;
        }
     //   _sensorTrigger.PressSubmit = false;
    }

    // =========================================---
    // 画像切り替え
    public void ChangeCharaImg(int talkCharaNum)
    {
        if (talkNum != 3) talkNum = talkCharaNum;
        // storyTextController.ChangeCharaImg(talkCharaNum);
        if (string.IsNullOrEmpty(missedTextData[talkNum].img))
        {
            storyTextController.charaImg.enabled = false;
            if (storyTextController.nameTag.activeSelf) storyTextController.nameTag.SetActive(false);
        }
        else
        {
            storyTextController.charaImg.enabled = true;
            if (!storyTextController.nameTag.activeSelf) storyTextController.nameTag.SetActive(true);
            switch (missedTextData[talkNum].img)
            {
                case "ディラン":
                    storyTextController.charaImg.sprite = storyTextController.charaSprites[int.Parse(missedTextData[talkNum].face)];
                    storyTextController.charaName.text = "ディラン";
                    break;
                case "ルーシー":
                    storyTextController.charaImg.sprite = storyTextController.charaSprites[int.Parse(missedTextData[talkNum].face) + 8];
                    storyTextController.charaName.text = "ルーシー";
                    break;
                case "ジャック":
                    storyTextController.charaImg.sprite = storyTextController.charaSprites[int.Parse(missedTextData[talkNum].face) + 16];
                    storyTextController.charaName.text = "ジャック";
                    break;
            }
        }
    }
}
