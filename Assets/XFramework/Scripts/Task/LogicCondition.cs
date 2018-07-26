using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XFramework.Task
{
    public class LogicCondition : ITaskCondition
    {
        protected bool m_IsFinish;
        public void Handle()
        {
        }

        public bool IsFinish()
        {
            return m_IsFinish;
        }

        public string Name()
        {
            return this.ToString();
        }

        public void Reset()
        {
            m_IsFinish = false;
        }

        public virtual void Start()
        {
        }
    }
}