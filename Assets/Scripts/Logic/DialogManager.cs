/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/7/2018 5:24:11 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;


public class DialogManager : ISingleton<DialogManager> {
    Dictionary<System.Int32, dbc.CommonDialog> dict;
    public void ShowCommonDialog(BattleEntity caster, BattleEntity target, DmgResult result)
    {
        dict = dict ?? LocalDataManager.Instance().m_ComDialog_Dict;
        
        string text = string.Empty;
        dbc.CommonDialog dialog = null;

        if (result.isDie)
        {
            dialog = dict[8];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
            dialog = dict[9];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
            return;
        }

        if (result.isDodge)
        {
            dialog = dict[1];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
        if (result.isBlock)
        {
            dialog = dict[2];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
        if (result.isDefence)
        {
            dialog = dict[3];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
       
        if (((float)target.m_Hp / (float)target.m_MaxHp) < 0.2f)
        {
            dialog = dict[7];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
        else if (((float)target.m_Hp / (float)target.m_MaxHp) < 0.3f)
        {
            dialog = dict[6];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
        else if (((float)target.m_Hp / (float)target.m_MaxHp) < 0.5f)
        {
            dialog = dict[5];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
        else
        {
            dialog = dict[4];
            Wirte(ParseText(dialog), caster.m_Name, target.m_Name, caster.m_Spell.m_Data.Name);
        }
    }

    public string ParseText(dbc.CommonDialog dialog) {
        int textIdx = UnityEngine.Random.Range(1, 6);
        string text = string.Empty;
        if (textIdx == 1)
        {
            text = dialog.RandomText1;
        } else if (textIdx == 2) {
            text = dialog.RandomText2;
        }
        else if (textIdx == 3)
        {
            text = dialog.RandomText3;
        }
        else if (textIdx == 4)
        {
            text = dialog.RandomText4;
        }
        else if (textIdx == 5)
        {
            text = dialog.RandomText5;
        }
        return text;
    }

    public void Wirte(string text, string casterName, string targetName=null, string skillName=null) {
        if (text.Contains("{2}"))
        {
            text = string.Format(text, "【" + casterName + "】:", "【" + skillName + "】", "【" + targetName + "】");
        }
        else if (text.Contains("{1}"))
        {
            text = string.Format(text, "【" + casterName + "】:", "【" + skillName + "】");
        }
        else
        {
            text = string.Format(text, "【" + casterName + "】:");
        }
        ShowManager.Instance().Add(text);
    }
}
