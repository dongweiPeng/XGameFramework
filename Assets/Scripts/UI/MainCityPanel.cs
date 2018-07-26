/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 7/25/2018 7:22:37 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.UI;
public class MainCityPanel : UIWndBase {
    public Button btn;
	// Use this for initialization
	void Start () {
        btn.onClick.AddListener(()=>UIManager.Instance.ShowWindow(WindowID.BagUI));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
