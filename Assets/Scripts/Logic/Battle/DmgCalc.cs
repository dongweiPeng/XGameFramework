/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/6/2018 10:58:00 AM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public struct DmgResult
{
    public int value;
    public bool isDodge;
    public bool isDefence;
    public bool isBlock;
    public bool isDie;
    public bool isCrit;
}

public class DmgCalc : ISingleton<DmgCalc>
{
    /// <summary>
    /// 伤害计算
    /// </summary>
    /// <param name="caster">施法者</param>
    /// <param name="target">受击者</param>
    /// <param name="spell">释放技能</param>
    /// <returns></returns>
    public DmgResult Calc(BattleEntity caster, BattleEntity target)
    {
        DmgResult result = default(DmgResult);
        Spell spell = caster.m_Spell;
        float leftHpRate = (float)caster.m_Hp / (float)caster.m_MaxHp;
        int defenceRate = (int)((1f - leftHpRate) / 3f * 100f);
        int defenceRange = Random.Range(1,101);
        if (defenceRate <= defenceRange && defenceRate>0)
        {
            //下回合攻击方下降20%
            BuffInfo buffInfo = default(BuffInfo);
            buffInfo.type = BuffType.Hit;
            buffInfo.valueType = BuffValueType.rate;
            buffInfo.value = -20;
            BuffManager.Instance().AddBuff(buffInfo, target);

            //下回合攻击方受伤75%
            buffInfo = default(BuffInfo);
            buffInfo.type = BuffType.Damage;
            buffInfo.valueType = BuffValueType.rate;
            buffInfo.value = 75;

            result.isDefence = true;
            BuffManager.Instance().AddBuff(buffInfo, caster);
        }
        else
        {
            float dmgValue = (caster.m_Fight + spell.Fight()) * spell.m_Data.DmgRate * 0.01f - (1.25f * target.m_Defence * target.m_Lv) * Random.Range(9, 12) * 0.1f;
            if (dmgValue < 1) dmgValue = Random.Range(5, 26);
            result.value = (int)dmgValue;

            int hitRate = (int)((float)(caster.m_Hit + spell.Hit()) / (float)((caster.m_Hit + spell.Hit()) + target.m_Dodge)) * 100;
            int range = Random.Range(1, 101);
            //命中
            if (hitRate <= range) {
                result.isDodge = false;
                int critRate = (int)((float)(caster.m_Lv * 12.5f) / (float)(caster.m_Lv * 25 + target.m_Defence)) * 100;
                range = Random.Range(1, 101);
                //暴击
                if (critRate <= range)
                {
                    result.value = (int)(1.5f * result.value);
                    result.isCrit = true;
                }
            }
            //闪避
            else
            {
                result.isDodge = true;
            }
        }
        return result;
    }
}
