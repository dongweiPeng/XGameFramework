using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Task;

public class TestTask : MonoBehaviour {
    Queue<XFramework.Task.Task> taskQueue = new Queue<XFramework.Task.Task>();
    public Button btn;
    XFramework.Task.Task curTask;
    XFramework.Task.Task task2;

    IEnumerator Start () {
        XFramework.Task.Task task1 = new XFramework.Task.Task("第一个任务 时间任务，间隔两秒", new TimeCondition(1.0f));
        TaskManager.Instance().AddTask(task1);

        //
        task2 = task2 ?? new XFramework.Task.Task("第二个任务 次数任务，3", new TimesCondition(3));
        task2 = new XFramework.Task.Task("第二个任务 次数任务，3", new TimesCondition(3));
        XFramework.Task.Task task3 = new XFramework.Task.Task("第三个任务 点击按钮",  new TirggerCondition(btn));
        TaskManager.Instance().AddTask(task2);
        TaskManager.Instance().AddTask(task3);

        curTask = TaskManager.Instance().Next();
        while (curTask!=null)
        {
            yield return curTask;
            Debug.Log(curTask.m_Name + " 已经做完了");
            curTask = TaskManager.Instance().Next();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "click")) {
            curTask.Condition.Handle();
        }
    }
}
