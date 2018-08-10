/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 11:02:58 AM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public string[] buttonName = {"添加经验"};
    public int button_col;
    public int button_row;
    public long total_exp;
	void Start () {
        LocalDataManager.Instance().LoadData();
        ExpManager.Instance().Init(total_exp);

        SavePlayer sp = new SavePlayer();
        Player player = new Player();
        player.m_Name = "测试名字";
        sp.Save<Player>(player);
	}

    private void OnGUI()
    {
        for (int i=0; i< button_row; i++) {
            for (int j=0; j< button_col; j++) {
                GUI.Button(new Rect(j*110, i*60,100, 50), buttonName[i*j]);
            }
        }
    }
}
