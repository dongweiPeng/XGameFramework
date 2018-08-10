/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 12:08:08 PM
Note : 玩家实体，数据由服务器或者本地发送
***************************************************/
using System;

public class Player{
    public int m_Id;
    public int m_Lv;
    public string m_Name;
    public int m_Hp = 48;
    public int m_Mp = 12;
    public int m_Fight = 12;
    public int m_Defence = 3;
    public int m_Dodge = 3;
    public int m_Hit = 3;

    public Spell m_Spell;
    public AttributePoint m_AttrPoint;
    /// <summary>
    /// 存储
    /// </summary>
    protected ISave m_Saver;


    public Player(bool listenLv = false) {
        m_Saver = m_Saver ?? new SavePlayer();
        Player database = m_Saver.GetData<Player>();

        m_AttrPoint = new AttributePoint();
        m_Lv = GameConfig.PlayerInitLv;
        m_Name = "小樱";

        DialogManager.Instance().Wirte("{0}准备起床......", m_Name);
        DialogManager.Instance().Wirte("{0}准备拉屎......", m_Name);
        DialogManager.Instance().Wirte("{0}准备出去活动......", m_Name);

        if (listenLv) {
            ExpManager.Instance().levelUp += (level) => {
                this.m_Lv = level;
                Save();
            };
        }
    }

    public void Save() {
        m_Saver.Save(this);
    }
}
