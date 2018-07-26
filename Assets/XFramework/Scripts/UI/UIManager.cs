/// <summary>
/// 窗口管理器
/// 支持功能： 动态改变缓存大小＋链状显示窗口＋动态更新alpha值 + 显示窗口，并自动关闭窗口 + 窗口msgbox + 动态配置bar + 预制窗口动画
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using XFramework.Utility;
using XFramework.AssetBundlePacker;
using XFramework;

namespace XFramework.UI {
	public class UIManager : MonoSingleton<UIManager> {
		 GameObject root;
		 GameObject forwardRoot;
		 GameObject uiRoot;

		private GameObject wndRoot;
		private GameObject msgBoxRoot;
		[HideInInspector]
		public  GameObject TipRoot;
        [HideInInspector]
        public GameObject DialogRoot;
        [HideInInspector]
		public  GameObject EffectRoot;
        [HideInInspector]
        public GameObject HubRoot;
        private static UIManager instance;

        private UIFrame FrameRoot;

		/// <summary>
		/// 缓存的窗口列表 
		/// </summary>
		private const int MAX_CACH_WND =6;
		//窗口 id  匹配 ui 预制体
		private Dictionary<WindowID, string> wndResDict = new Dictionary<WindowID, string>(); 

		private Dictionary<WindowID, UIWndBase> cacheWndDict = new Dictionary<WindowID, UIWndBase>(); 
		/// <summary>
		/// 窗口的返回队列 , 只有追加型的窗口会进入栈，一次只会存在一个栈
		/// </summary>
		private Stack<UIWndBase> backStack = new Stack<UIWndBase> ();
		/// <summary>
		/// 当前显示的窗口 
		/// </summary>
		private UIWndBase curShowWnd;
		public UIWndBase CurShowWnd{
			get{ 
				return curShowWnd;		
			}
		}
			
		public static UIManager Intance{
			get{ 
				if (instance == null) {
					GameObject obj = new GameObject ("UIRoot");

					instance = obj.AddComponent<UIManager> ();
				}
				return instance;
			}
		}

		public void Init(){
			instance = this;
			wndRoot = new GameObject ("WndRoot");
			msgBoxRoot = new GameObject("MsgBoxRoot");
            DialogRoot = new GameObject("DialogRoot");
            TipRoot = new GameObject("TipRoot");
			EffectRoot = new GameObject("EffectRoot");
			root = GameObject.FindWithTag ("UIRoot");
            HubRoot = new GameObject("HubRoot");
            Util.AddChildToTarget (root.transform, wndRoot.transform);
			Util.AddChildToTarget (root.transform, msgBoxRoot.transform);
            Util.AddChildToTarget(root.transform, DialogRoot.transform);
            Util.AddChildToTarget(root.transform, TipRoot.transform);
            Util.AddChildToTarget(root.transform, HubRoot.transform);
            //Util.AddChildToTarget (forwardRoot.transform, EffectRoot.transform);
            //初始化窗口预制体资源
            foreach (WindowID id in Enum.GetValues(typeof(WindowID))) {
				wndResDict.Add(id, "Panel/"+id.ToString());
			}
		}

			
		/// <summary>
		/// 向指定窗口发送消息
		/// </summary>
		public void SendMsg(WindowID wndId, WindowMsgID msgId, object param){
			if (cacheWndDict.ContainsKey (wndId)) {
				UIWndBase wnd = cacheWndDict [wndId];
				wnd.OnMsg (msgId, param);
			} else {
				Log ("窗口不在缓存中, 事件更新失败");
			}
		}


		/// <summary>
		/// 消息窗口的显示，目前支持3种类型
		/// </summary>
		public void ShowMessageBox(string msg, MessageBoxType type, MsgBoxCallBack callback=null){
			string wndPath = wndResDict [WindowID.Msgbox];
			GameObject prefab =	Resources.Load<GameObject> ("UIPrefab/Msgbox");
			if (prefab != null) {
				GameObject clone = (GameObject)GameObject.Instantiate (prefab);
				Util.AddChildToTarget (msgBoxRoot.transform, clone.transform, Vector3.zero);
				UIMessageBox msgBox = clone.GetComponent<UIMessageBox> ();
			}
		}

		/// <summary>
		/// 窗口显示，队列型加载
		/// </summary>
		public void ShowWindow(WindowID windowId, bool bAppend = false){
            if (instance == null) Init();
            if (FrameRoot == null) {
                GameObject prefab = ResourcesManager.Load<GameObject>("BuildIn/Frame/UIFrame");
                FrameRoot = Util.NewGameObject(prefab, wndRoot).GetComponent<UIFrame>();
            } 
            FrameRoot.Init(windowId);

            if (wndResDict.ContainsKey (windowId)) {
				if (cacheWndDict.ContainsKey (windowId)) {
					UIWndBase wnd = cacheWndDict [windowId];
					Log ("缓存中, 状态是:" + wnd.status.ToString());
					if (wnd.status == WindowStatus.Inactive) {
						if (bAppend) {
							if (backStack.Count == 0) {
								backStack.Push (curShowWnd);
							}
							backStack.Push (wnd);
							AdjustAlpha ();
						} else {
							HideAllCach ();
							BreakBackStack ();
							curShowWnd.Close ();
						}
						wnd.Show ();
						curShowWnd = wnd;
					} else if (wnd.status == WindowStatus.Gray) {
						curShowWnd.Close ();
						curShowWnd = wnd;
						backStack.Pop ();
						AdjustAlpha ();
					} else if (wnd.status == WindowStatus.Active) {
						Log ("当前窗口已经是显示窗口");
					}
					wnd.Refresh ();
				} else {
					Log ("新加入窗口:" + windowId.ToString());
					RealShow(windowId, bAppend);
				}	

			} else {
				Log (windowId.ToString()+" 不存在资源");
			}
		}
			
		private void RealShow(WindowID windowId, bool bAppend){
			string wndPath = wndResDict [windowId];
            string panelPath = string.Format("{0}Panel/{1}",AssetDefine.ResRoot, windowId.ToString());
            GameObject prefab = ResourcesManager.Load<GameObject>(panelPath);
			if (prefab != null) {
				GameObject clone = (GameObject)GameObject.Instantiate (prefab);
				XFramework.Utility.Util.AddChildToTarget (wndRoot.transform, clone.transform);
				UIWndBase wnd = clone.GetComponent<UIWndBase> ();
				wnd.status=WindowStatus.Active;
				if (curShowWnd != null) {
					wnd.preWndID = curShowWnd.wndID;			
				}
				wnd.wndID = windowId;
				if (bAppend) {
					Log ("push into stack " +curShowWnd.wndID.ToString());
					if(backStack.Count==0){
						backStack.Push (curShowWnd);
					}
					curShowWnd = wnd;
					backStack.Push (curShowWnd);
					AdjustAlpha ();
				} else {
					curShowWnd = wnd;
					HideAllCach ();
					BreakBackStack ();
				}
				cacheWndDict.Add (windowId, curShowWnd);
				curShowWnd.AdjustAlpha (1.0f);
				CheckCach ();
				wnd.Refresh ();
			}
		} 

		/// <summary>
		/// 调整窗口alpha值 
		/// </summary>
		private void AdjustAlpha(){
			float tinyAlpha = (float)((float)1/ (float)backStack.Count);
			int index = 0;
			foreach(UIWndBase wnd in backStack){
				float alpha =(float) ((backStack.Count-index) *  (float)tinyAlpha);
				alpha = float.Parse(Mathf.Clamp (alpha, 0.2f, 1f).ToString ("0.0"));
				Log (" wnd = " + wnd.wndID.ToString() +", alpha = " + alpha);
				wnd.AdjustAlpha (alpha);

				index++;
			}
		}

		/// <summary>
		/// 检查缓存区~如果超过最大上限值，则进行清理，如果链存在不进行清理，一般情况下，链的长度不会超过缓存大小
		/// </summary>
		private void CheckCach(){
			if (cacheWndDict.Count < MAX_CACH_WND) {
				return;
			} else {
				int minDepth = MAX_CACH_WND;
				UIWndBase wnd = null;
				foreach(KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict){
					if (pair.Value.Depth < minDepth && !backStack.Contains(pair.Value)) {
						minDepth = pair.Value.Depth;
						wnd = pair.Value;
					}
				}
				if (wnd!=null) {
					Log ("当前窗口缓存过大 清理窗口 id = " + wnd.wndID);
					Destroy (wnd.gameObject);
					cacheWndDict.Remove (wnd.wndID);
				}
			}
		}

		public bool Exist(WindowID id){
			foreach(var v in cacheWndDict){
				if (id == v.Value.wndID) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 因此其他缓存窗口
		/// </summary>
		private void HideAllCach(){
			foreach(KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict){ 
				if (pair.Value == curShowWnd) {
					pair.Value.Show ();
				} else {
					pair.Value.Close ();
				}
			}
		}

		/// <summary>
		/// 处理断链 
		/// </summary>
		private void BreakBackStack(){
			backStack.Clear ();
		}

		/// <summary>
		/// 清除缓存窗口
		/// </summary>
		public void ClearCachExcp(WindowID windowID)
		{
			List<WindowID> deleteList = new List<WindowID>();
			foreach (KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict)
			{
				if (windowID != pair.Key)
				{
					Destroy(pair.Value.gameObject);
					deleteList.Add(pair.Key);
				}
			}

			foreach (WindowID wnd in deleteList)
			{
				cacheWndDict.Remove(wnd);
			}
			deleteList.Clear();
		}

		public void ClearMessageBoxs(){
		
		}
        public void CloseAllWindow()
        {
            foreach (KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict)
            {
      
                {
                    Destroy(pair.Value.gameObject);
                }
            }
            cacheWndDict.Clear();
        }

			
		public void Log(string msg){
			Debug.Log ("UIManager >>>>" + msg);
		}
	}
}
