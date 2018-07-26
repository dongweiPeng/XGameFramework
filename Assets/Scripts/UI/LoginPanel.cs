/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 7/25/2018 6:59:51 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.UI;

public class LoginPanel : UIWndBase {
    public Button btn_Login;
	// Use this for initialization
	void Start () {
        btn_Login.onClick.AddListener(()=>UIManager.Instance.ShowWindow(WindowID.MainCityPanel));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
