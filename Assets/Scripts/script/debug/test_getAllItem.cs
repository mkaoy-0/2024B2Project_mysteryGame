using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_getAllItem : MonoBehaviour
{
    /// <summary>
    /// 確認用
    /// 全アイテム取得、ストーリーに移行
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            for(int i = 0; i < GetItemTrigger.checkedItem.Length; i++)
            {
                GetItemTrigger.checkedItem[i] = true;
            }

            for(int i = 0; i < GetItemTrigger.getItems.Length; i++)
            {
                GetItemTrigger.AddItem(i);
            }
        }
    }
}
