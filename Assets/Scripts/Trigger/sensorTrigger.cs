using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorTrigger : MonoBehaviour
{
    // WSAD
    public bool _pressW, _pressS, _pressA, _pressD;

    // 決定キー
    public bool _pressSubmit, _pressSubmit2, _pressSubmit3, _pressSubmit4;

    // 開いているページ
    public static int _currentPage = 0;

    void Start()
    {
    }

    void Update()
    {
        KeyPressed();
        if (_currentPage > 0)
        {
            SceneChange();
        }
    }

    public void KeyPressed()
    {
        // 上下左右
        if (Input.GetKeyDown(KeyCode.W))
        {
            _pressW = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _pressS = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _pressA = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _pressD = true;
        }

        // 決定
        if (Input.GetKeyDown(KeyCode.L))
        {
            _pressSubmit = true;
        }
        // 予備ボタン
        if (Input.GetKeyDown(KeyCode.I))
        {
            _pressSubmit2 = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            _pressSubmit3 = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            _pressSubmit4 = true;
        }
    }

    public void SceneChange()
    {
        // ページ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentPage = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentPage = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentPage = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _currentPage = 4;
        }
    }


    // 変数受け渡し
    public bool PressW
    {
        get { return _pressW; }
        set { _pressW = value; }
    }
    public bool PressS
    {
        get { return _pressS; }
        set { _pressS = value; }
    }
    public bool PressA
    {
        get { return _pressA; }
        set { _pressA = value; }
    }
    public bool PressD
    {
        get { return _pressD; }
        set { _pressD = value; }
    }
    public bool PressSubmit
    {
        get { return _pressSubmit; }
        set { _pressSubmit = value; }
    }
    public bool PressSubmit2
    {
        get { return _pressSubmit2; }
        set { _pressSubmit2 = value; }
    }
    public bool PressSubmit3
    {
        get { return _pressSubmit3; }
        set { _pressSubmit3 = value; }
    }
    public bool PressSubmit4
    {
        get { return _pressSubmit4; }
        set { _pressSubmit4 = value; }
    }
    public static int currentPage
    {
        get { return _currentPage; }
        set { _currentPage = value; }
    }
}
