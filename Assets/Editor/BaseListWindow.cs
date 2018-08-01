/// <summary>
/// 列表显示基类，主要控制单个执行的需求窗口 
/// 目前已具有的功能： 显示基本信息（id+name+执行选项） + 默认的初始化文件作为比较方式（可以自己扩展）+ 执行接口（需要自己实现）
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class BaseListWindow : EditorWindow {
	protected Vector2 scrollViewPos = Vector2.zero;

	//生成文件
	public string CreatedFilePath;
	public string CreatedFilePattern;

	//原始文件
	public string OriginFilePath;
	public string OriginFilePattern;

	protected List<BaseWindowListData> listShowingCreatedData = new List<BaseWindowListData>();
	protected List<BaseWindowListData> listShowingUnCreatedData = new List<BaseWindowListData>();
	//选中列表
	protected List<BaseWindowListData> listSelectedData = new List<BaseWindowListData>();
    //是否要同步到Game_Tools工程目录下
    protected bool isNeedSync2ToolProject = false;

	//显示的数据
	public class BaseWindowListData{
		public int id; 
		public string name;
		public bool IsExeced;
		public BaseWindowListData(int id, string name, bool isExeced){
			this.id = id;
			this.name = name;
			this.IsExeced = isExeced;
		}
	}
	
	public virtual void Sync2ToolsProjectDir()
    {

    }

    protected virtual void OnShowChildFilePath()
    {
    }

    /// <summary>
    /// 给子类去扩展的OnGUI方法
    /// </summary>
    public virtual void OnChildGUI()
    {
    }

	void OnGUI()
	{
		GUILayout.Space(5);
		GUI.color = Color.green;

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("一键执行 未创建选项")){
			ExecAllUnCreate();
		}
		if(GUILayout.Button("一键执行 所有选项")){
			ExecAll();
		}

		if(GUILayout.Button("一键执行 选中选项")){
			ExecAllSelected();
            if (isNeedSync2ToolProject)
            {
                Sync2ToolsProjectDir();
            }
		}
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        isNeedSync2ToolProject = EditorGUILayout.Toggle("是否需要同步到tools工程目录", isNeedSync2ToolProject);
		EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
	    OnShowChildFilePath();
        GUILayout.Space(5);

		GUILayout.Label("源文件路径: "+OriginFilePath);
		GUILayout.Label("生成文件路径: "+CreatedFilePath);

		GUILayout.Space(5);
		if(GUILayout.Button("刷新文件状态")){
			InitData ();
		}
		GUILayout.Space(5);
		GUILayout.Label("(未创建)", "GUIEditor.BreadcrumbLeft");
		GUILayout.Space(5);
		GUI.color = Color.gray;

		scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos, false, true);
		GUI.color = Color.cyan;
		ShowData(listShowingUnCreatedData);
		GUI.color = Color.white;
		GUILayout.Space(5);
		GUILayout.Label("(已创建)", "GUIEditor.BreadcrumbLeft");
		GUILayout.Space(5);
		ShowData(listShowingCreatedData);
		EditorGUILayout.EndScrollView();

	    OnChildGUI();
	}

	/// <summary>
	/// 显示数据
	/// </summary>
	void ShowData(List<BaseWindowListData>  dataList){

		foreach(BaseWindowListData data in dataList){
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();

            data.IsExeced = EditorGUILayout.Toggle("", data.IsExeced, GUILayout.Width(30));
			GUILayout.Label("Id:"+data.id+"", GUILayout.Width(50f));
            GUILayout.Label("名字:" + data.name + "", GUILayout.Width(250f));
			ShowExtData(data);
            //GUILayout.Label("Id:" + data.id + "", GUILayout.Width(50f));
            //data.IsExeced = EditorGUILayout.Toggle("是否执行", data.IsExeced);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
	}

	/// <summary>
	/// 默认的初始化数据。比较器是两个文件夹的文件是否一致 ,使用者可以自己扩展
	/// </summary>
	protected virtual void InitData(){

		listShowingCreatedData.Clear();
		listShowingUnCreatedData.Clear();
		listSelectedData.Clear();

		string[] originFiles = Directory.GetFiles(OriginFilePath, OriginFilePattern, SearchOption.AllDirectories);
		string[] createdFiles = Directory.GetFiles(CreatedFilePath, CreatedFilePattern, SearchOption.AllDirectories);

		BaseWindowListData data = null;
        string[] showFileArray = null;
		foreach(string ori in originFiles){
			string oriFile = Path.GetFileNameWithoutExtension(ori);
			bool bCreated = false;
			foreach(string cre in createdFiles){
				string creFile = Path.GetFileNameWithoutExtension(cre);
				if(creFile.Equals(oriFile)){
					data = new BaseWindowListData(listShowingCreatedData.Count, oriFile, false);
					listShowingCreatedData.Add(data);
					bCreated = true;
					break;
				}
			}
			if(!bCreated){
				data = new BaseWindowListData(listShowingUnCreatedData.Count, oriFile, false);
				listShowingUnCreatedData.Add(data);
			}
		}
	}

	/// <summary>
	/// 使用者显示的扩展数据, 如果有需要的
	/// </summary>
	protected virtual void ShowExtData(BaseWindowListData data){
		
	}

	/// <summary>
	/// 执行所有未创建的 
	/// </summary>
	protected virtual void ExecAllUnCreate(){
		
	}

	/// <summary>
	/// 全部执行 
	/// </summary>
	protected virtual void ExecAll(){

	}

	/// <summary>
	/// 执行所有选中选项
	/// </summary>
	protected virtual void ExecAllSelected(){
		if(listSelectedData.Count>0){
			listSelectedData.Clear();
		}
		if (listShowingCreatedData.Count>0){
			foreach(BaseWindowListData data in listShowingCreatedData){
				if(data.IsExeced){
					listSelectedData.Add(data);
				}
			}
		}
		if (listShowingUnCreatedData.Count>0){
			foreach(BaseWindowListData data in listShowingUnCreatedData){
				if(data.IsExeced){
					listSelectedData.Add(data);
				}
			}
		}
	}
}
