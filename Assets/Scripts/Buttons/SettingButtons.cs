using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButtons : MonoBehaviour
{
    // ゲーム終了
    public void EndGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

    // シーン切り替え ===================================
    public void ChangeRoom_main()
    {
        sensorTrigger.currentPage = 1;
    }

    public void ChangeRoom_d()
    {
        sensorTrigger.currentPage = 2;
    }

    public void ChangeRoom_l()
    {
        sensorTrigger.currentPage = 3;
    }
    public void ChangeRoom_j()
    {
        sensorTrigger.currentPage = 4;
    }
    // ================================================--

    // 全アイテム入手 ===================================
    public void AllItemGetting()
    {
        for (int i = 0; i < GetItemTrigger.checkedItem.Length; i++)
        {
            GetItemTrigger.checkedItem[i] = true;
        }

        for (int i = 0; i < GetItemTrigger.getItems.Length; i++)
        {
            GetItemTrigger.AddItem(i);
        }
    }
    // ================================================--


}
