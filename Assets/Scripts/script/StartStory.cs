using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStory : MonoBehaviour
{
    // ==================================-
    // アイテム集め終わった後、キャンバス切り替え
    // =================================

    public GameObject storyCanvas;
    public GameObject ItemCollectCanvas;

    // ストーリーキャンバスの方のアイテム一覧管理スクリプトをONにする
    public GameObject itemListControllerDuringStory;

    // シーン切り替えスクリプト
    public GameObject sceneChangeController;

    void Start()
    {
        
    }

    void Update()
    {
        StoryCanvasActovate();
    }

    // キャンバス切り替え
    public void StoryCanvasActovate()
    {
        if (GetAllItem.StartStory && !storyCanvas.activeSelf)
        {
            storyCanvas.SetActive(true);
            ItemCollectCanvas.SetActive(false);
            itemListControllerDuringStory.SetActive(true);

            // シーン切り替え不可
            sceneChangeController.SetActive(false);
        }
    }
}
