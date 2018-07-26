/// <summary>
/// 消息窗口-模态化  参数返回说明: 0-左边按钮，1-中间按钮，2-右边按钮
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using XFramework.UI;

namespace XFramework.UI
{
    public delegate void MsgBoxCallBack(MessageBoxEvent evt);

    public class UIMessageBox : MonoBehaviour
    {
        public Text textMsg;
        public GameObject btnLeft;
        public Text textLeft;
        public GameObject btnCenter;
        public Text textCenter;
        public GameObject btnRight;
        public Text textRight;
        public MsgBoxCallBack callback;
        RectTransform rt;

        public void InitMsgBox(string msg, MessageBoxType type, MsgBoxCallBack callback)
        {
            if (rt == null)
            {
                rt = GetComponent<RectTransform>();
            }
            //	rt.DOScale(1, 0.5f);
            rt.gameObject.SetActive(true);
            btnLeft.SetActive(false);
            btnCenter.SetActive(false);
            btnRight.SetActive(false);

            textMsg.text = msg;
            this.callback = callback;
            if (type == MessageBoxType.OK)
            {
                SetCenterBtn("确定");
            }
            else if (type == MessageBoxType.OK_Cancle)
            {
                SetLeftBtn("确定");
                SetRightBtn("取消");
            }
            else if (type == MessageBoxType.OK_Other_Cancle)
            {
                SetLeftBtn("确定");
                SetCenterBtn("其他");
                SetRightBtn("取消");
            }
        }

        public void SetLeftBtn(string msg)
        {
            btnLeft.SetActive(true);
            textLeft.text = msg;
        }

        public void ClickLeft()
        {
            if (callback != null)
            {
                callback(MessageBoxEvent.Ok);
            }
            Close();
            //	Destroy(this.gameObject);
        }


        public void SetRightBtn(string msg)
        {
            btnRight.SetActive(true);
            textRight.text = msg;
        }

        public void ClickRight()
        {
            if (callback != null)
            {
                callback(MessageBoxEvent.Cancle);
            }
            Close();
            //Destroy(this.gameObject);
        }

        void SetCenterBtn(string msg)
        {
            btnCenter.SetActive(true);
            textCenter.text = msg;
        }

        public void ClickCenter()
        {
            if (callback != null)
            {
                callback(MessageBoxEvent.Ok);

            }
            Close();
            //Destroy(this.gameObject);
        }

        void Close()
        {
            //	rt.localScale = Vector3.zero;
            rt.gameObject.SetActive(false);
        }
    }
}