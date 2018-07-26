using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 任务
/// </summary>
namespace XFramework.Task
{
    public class Task : IEnumerator,IReset
    {
        /// <summary>
        /// 任务完成所需要的条件
        /// </summary>
        private ITaskCondition m_Condition;

        public string m_Name;

        public Task(string name, ITaskCondition condition)
        {
            this.m_Name = name;
            this.m_Condition = condition;
        }

        public Task(ITaskCondition condition)
        {
            this.m_Condition = condition;
        }

        public ITaskCondition Condition
        {
            get
            {
                return m_Condition;
            }
        }

        public object Current
        {
            get
            {
                return null;
            }
        }


        public bool MoveNext()
        {
            return !m_Condition.IsFinish();
        }

        public void Reset()
        {
            Debug.Log(string.Format("任务{0} 结束并重置",this.m_Name));
            this.m_Condition.Reset();
        }
    }
}