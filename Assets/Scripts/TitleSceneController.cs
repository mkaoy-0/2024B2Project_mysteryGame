using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleSceneController : MonoBehaviour
{
    public sensorTrigger _sensorTrigger;

    public GameObject TitleCanvas;
    public GameObject IntroCanvas;
    public TextMeshProUGUI introText;
    public Image icon;
    public bool IntroFinished = false;

    public Image bg;
    public Sprite introImg;

    public int currentText = 0;
    private string[] text = new string[2]
    { "ある朝、資産家ローレンス氏の遺体が自宅の書斎で発見された。\n \n 本棚が倒れ、その下敷きになっていた。一見すると事故のようだが\nよく調べると不可解な点がいくつもある。\n\n昨夜、いったい何が起こったのか？\nこれは本当にただの不運な出来事なのか、それとも誰かが仕組んだ事件なのか？\n\n真相を突き止め、事件の全貌を明らかにせよ―。",
      "疑わしいのは、彼の三人の子供たち―\n長男のディラン、長女のルーシー、そして次男のジャック。\n\nそれぞれが何かを隠している。\n\nまずは、事件現場の書斎と三人の部屋を調べて、証拠を集めよう。"
    };

    public bool isProcessing = false;
    void Start()
    {
        
    }

    void Update()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (_sensorTrigger.PressSubmit)
        {
            if (!IntroFinished)
            {
                if (!isProcessing)
                // sensorTrigger.currentPage = 1;
                ShowIntroText();
            }
            else
            {
                sensorTrigger.currentPage = 1;
            }
        }
        _sensorTrigger.PressSubmit = false;
    }

    public void ShowIntroText()
    {
        if (TitleCanvas.activeSelf) TitleCanvas.SetActive(false);
        if (!IntroCanvas.activeSelf) IntroCanvas.SetActive(true);
        StartCoroutine(Dialogue());
    }

    IEnumerator Dialogue()
    { 
        if (currentText == 1)
        {
            bg.sprite = introImg;
        }
        icon.enabled = false;

        yield return new WaitForSeconds(0.1f);
        isProcessing = true;
        introText.text = text[currentText];
        introText.enabled = true;
        int length = introText.text.Length;
        for (int i = 0; i < length; i++)
        {
            introText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.05f);
        }
        introText.maxVisibleCharacters = length;

        if (introText.maxVisibleCharacters == length)
        {
            yield return new WaitForSeconds(0.3f);
            icon.enabled = true;
            isProcessing = false;
        }

        // 次のセリフがない場合には、全部非表示。
        if (currentText + 1 >= text.Length)
        {
            yield return new WaitForSeconds(0.3f);
            icon.enabled = true;
            isProcessing = false;
            IntroFinished = true;
        }

        // 次のセリフをセットする。
        currentText++;
    }

}
