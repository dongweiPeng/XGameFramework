using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework { 
public enum EventsType  {
        Skill_Disperse, //技能驱散
        Skill_AddBuff, //添加Buff
        Skill_EndDmg, //伤害计算完毕
        Battle_End,

        #region UIFrame //窗口
        FrameItem_Refresh,
        #endregion
    }
}
