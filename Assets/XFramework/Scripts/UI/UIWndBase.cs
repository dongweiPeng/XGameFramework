/// <summary>
/// 抽象窗口 
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace XFramework.UI
{
    public class UIWndBase : MonoBehaviour
    {
        public WindowStatus status;

        public WindowID wndID;
        //前一窗口的id
        public WindowID preWndID;
        public float alpha;
        //注意CanvasRenderer 的alpha值 是与 image 配合的，必须调节image的alpha值为255才能有效
        private CanvasRenderer render;

        void Awake()
        {
            render = GetComponent<CanvasRenderer>();
        }

        //刷新
        public virtual void Refresh()
        {

        }

        public int Depth
        {
            get
            {
                return transform.GetSiblingIndex();
            }
            set
            {
                transform.SetSiblingIndex(value);
            }
        }

        public virtual void Show()
        {
            this.gameObject.SetActive(true);
            this.status = WindowStatus.Active;

        }

        public virtual void Close()
        {
            this.status = WindowStatus.Inactive;
            this.gameObject.SetActive(false);
        }

        public virtual void AdjustAlpha(float alpha)
        {
            if (alpha != 1)
            {
                this.status = WindowStatus.Gray;
            }
            this.alpha = alpha;
            if (render != null)
                render.SetAlpha(alpha);
        }

        public virtual void AdjustDepth(int depth)
        {

        }

        public void Back()
        {
            if (preWndID != WindowID.Invalid)
            {
                UIManager.Intance.ShowWindow(preWndID);
            }
            else
            {
                UIManager.Intance.ShowWindow(WindowID.MainCityPanel);
            }
        }
        public virtual void OnMsg(WindowMsgID msgId, object param)
        {
            Debug.Log("wndId = " + wndID + " , 收到消息 msgId =" + msgId);
        }

    }
}