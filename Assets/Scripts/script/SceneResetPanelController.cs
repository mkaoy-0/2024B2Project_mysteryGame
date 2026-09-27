using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneResetPanelController : MonoBehaviour
{
    // =================================-
    /// <summary>
    ///  場面転換と最後の画面暗転
    /// </summary>
    // =================================

    // 
    public sensorTrigger _sensorTrigger;
    public GameObject sceneChangeController;

    public Image resetPanel;
    private TextMeshProUGUI _text;

    private bool fadeIn = true;
    private float alpha = 0.01f;
    private float textAlpha = 1f;

    // =====================
    //
    private string[] endingText = new string[2];
    private int currentTextNum = 0;
    // 
    private bool goToNextText = true;
    private bool isPlaying = true;

    // 終了
    public static bool GameFinished = false;

    void Start()
    {
        _text = resetPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        endingText[0] = "―事件は解決した。資産家殺人事件、その真相は遺産を巡る悲劇だった。";
        endingText[1] = "なんだかんだあったけれど、これにて一件落着だ。さあ、帰ってゆっくりしよう。";
    }

    void Update()
    {
        if (!isPlaying)
        {
            //  FadeOutText();
            GameFinished = true;
        }

        // タイトルに戻る
        if (GameFinished)
        {
            if (_sensorTrigger.PressSubmit)
            {
                sensorTrigger.currentPage = 0;
                sceneChangeController.SetActive(true);
            }
            _sensorTrigger.PressSubmit = false;
        }
    }

    public IEnumerator FadePanelCoroutine()
    {
        yield return new WaitForSeconds(0.01f);

        resetPanel.enabled = true; // フェード開始時にパネルを有効化

        while (true)
        {
            if (fadeIn)
            {
                alpha += 0.1f;
                if (alpha >= 1f)
                {
                    alpha = 1f;
                    fadeIn = false;
                }
            }
            else
            {
                alpha -= 0.1f;
                if (alpha <= 0f)
                {
                    alpha = 0f;
                    resetPanel.enabled = false;
                    fadeIn = true;
                    break; // フェードが完了したらループを抜ける
                }
            }

            resetPanel.color = new Color(0, 0, 0, alpha);
            yield return null; // 1フレーム待機
        }
    }

    // 最後の演出
    public void Ending()
    {
        StartCoroutine(FadeEndingPanel());
        showText();
    }


    public IEnumerator FadeEndingPanel()
    {
        resetPanel.enabled = true; // フェード開始時にパネルを有効化

        while (true)
        {
            alpha += 0.08f;
            if (alpha >= 1f)
            {
                alpha = 1f;
                break; // フェードが完了したらループを抜ける
            }
            resetPanel.color = new Color(0, 0, 0, alpha);
            yield return null; // 1フレーム待機
        }
    }

    public void showText()
    {
        if (goToNextText)
        {
            if (isPlaying)
            {
                StartCoroutine(Dialogue());
            }       
        }
    }

    IEnumerator Dialogue()
    {
        goToNextText = false;
        _text.text = endingText[currentTextNum];
        int length = endingText[currentTextNum].Length;
        for (int i = 0; i < length; i++)
        {
            _text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.01f);
        }
        _text.maxVisibleCharacters = length;

        if (_text.maxVisibleCharacters == length)
        {
            goToNextText = true;
        }

        // 終ったらタイトルに戻る
        if (currentTextNum + 1 >= endingText.Length)
        {
            Debug.Log("GameEnd");
            isPlaying = false;
        }
        else
        {
            isPlaying = true;
        }

        // 次のセリフをセットする。
        currentTextNum = currentTextNum + 1;
    }

    // 最後にテキスト非表示にする
    public void FadeOutText()
    {
        textAlpha -= 0.01f;
        if (textAlpha < 0)
        {
            textAlpha = 0f;
            GameFinished = true;
        }
        _text.color = new Color(255, 255, 255, textAlpha);
    }
}
