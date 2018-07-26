using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务的抽象条件
/// </summary>

namespace XFramework.Task
{
    public interface ITaskCondition : IReset
    {
        void Start();
        bool IsFinish();
        string Name();
        void Handle();
    }
}
