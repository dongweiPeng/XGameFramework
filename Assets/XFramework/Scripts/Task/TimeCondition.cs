using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 时间作为任务条件
/// </summary>
namespace XFramework.Task {
    public class TimeCondition : ITaskCondition
    {
        private float m_duration;
        private float m_startTime;
        protected bool m_IsFinish;

        public TimeCondition(float duration) {
            this.m_duration = duration;
            Reset();
        }

        public void Handle()
        {
            throw new NotImplementedException();
        }

        public bool IsFinish()
        {
            if (this.m_IsFinish) return true;
            return (Time.time - this.m_startTime) >= this.m_duration;
        }

        public string Name()
        {
            throw new NotImplementedException();
        }

        public virtual void Start()
        {
            this.m_startTime = Time.time;
        }

        public void Reset()
        {
            this.m_IsFinish = false;
        }
    }
}
