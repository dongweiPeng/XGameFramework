using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static class ProtoBuildTool
{
	static ProtoSetting mProtoSetting;

    public static readonly string tempExcelDir = Application.dataPath + "/../csv2Data/tools/python_protoc/txt/";
    public static readonly string serverCsvDataDir = Application.dataPath + "/../../../common/data/";
	static string configPath = Application.dataPath + "/Editor/CsvToData/protoConfig";
	static SettingData mSettingData;

	/// <summary>
	/// 配置等级. 0:没有设置过配置 1:UseDefaultConfig 2:USeCustomConfig
	/// </summary>
	static int configLevel = 0;

	[MenuItem("游戏工具/游戏数据生成/生成数据")]
	public static void BuildData()
	{
		LoadPref ();
		LoadConfig ();
		if (configLevel == 0) {
			if (ShowDialog("你还没有进行过任何设置，请至少使用默认配置"))
			{
				OpenConfigWindow ();
			}
		} else {			
			OpenProtoListWindow ();
		}
	}

    [MenuItem("游戏工具/游戏数据生成/配置设置 %q")]
	public static void ConfigSetting()
	{
		LoadPref ();
		LoadConfig ();
		OpenConfigWindow ();
	}

    [MenuItem("游戏工具/游戏数据生成/一键拷贝到开发环境(程序编程环境)")]
	public static void CopyToDevEnv()
	{
		LoadPref ();
		if (configLevel == 0) {
			if (ShowDialog("你还没有进行过任何设置，请至少使用默认配置"))
			{
				OpenConfigWindow ();
			}
		}

		if (null == mSettingData) {
			LoadConfig ();
		}

		string bytesDataPath = EditorConfig.DevProjectDirRoot + "/Resources/ConfigData/";
		string genCodePath = EditorConfig.DevProjectDirRoot + "/Scripts/GenerateCodes/";
		EditorConfig.CopyDir (mSettingData.bytesPath, bytesDataPath);
		EditorConfig.CopyDir (mSettingData.csPath, genCodePath);
	}

    [MenuItem("游戏工具/游戏数据生成/一键导出csv+数据生成+拷贝到开发环境(程序编程环境)")]
	public static void OnKeyAllOperate()
	{
		LoadPref ();
		LoadConfig ();
		if (configLevel == 0) {
			if (ShowDialog("你还没有进行过任何设置，请至少使用默认配置"))
			{
				OpenConfigWindow ();
			}
		} else {			
			ExcuteModule ();
			CopyToDevEnv ();
		}
	}

    [MenuItem("游戏工具/游戏数据生成/SVN提交修改 %t")]
    public static void SvnCommit()
    {
        LoadPref();
        LoadConfig();
        if (configLevel == 0)
        {
            if (ShowDialog("你还没有进行过任何设置，请至少使用默认配置"))
            {
                OpenConfigWindow();
            }
        }
        else
        {
           Debug.Log("功能暂未实现..");
        }
    }

	static void LoadPref ()
	{
		configLevel = PlayerPrefs.GetInt ("Csv_ConfigLevel", 0);
	}

	public static bool ShowDialog(string hintStr)
	{
		return EditorUtility.DisplayDialog ("提示", hintStr, "OK");
	}

	static void OpenConfigWindow()
	{
		if (null == mProtoSetting)
			mProtoSetting = (ProtoSetting)EditorWindow.GetWindow <ProtoSetting>();		
		mProtoSetting.settingData = mSettingData;
		mProtoSetting.useDefaultConfig = (1 == configLevel);
		mProtoSetting.titleContent.text = "游戏数据生成";
		mProtoSetting.Show ();
	}

	static void OpenProtoListWindow()
	{
		BuildProtoListWindow bplw = (BuildProtoListWindow)EditorWindow.GetWindow<BuildProtoListWindow> ();
		bplw.titleContent.text = "生成列表窗口";
        bplw.InitListWindow(mSettingData.excelPath, mSettingData.bytesPath, mSettingData.csPath, mSettingData.protoToolsDir);
		bplw.Show ();
	}

	public static void LoadConfig()
	{
		string filename = "";
		bool useDefalutConfig = false;
		if (2 == configLevel) {
			filename = "/ProtoConfig.txt";
		} else {
			filename = "/ProtoConfigDefault.txt";
			useDefalutConfig = true;
		}
		LoadConfigData (filename, useDefalutConfig);
	}

	static void LoadConfigData(string filename, bool isDefaultConfig)///ProtoConfig.txt  
	{
		string filePath = configPath + filename;
		string assetPath = "Assets/Editor/CsvToData/protoConfig" + filename;
		if (File.Exists (filePath)) {
			TextAsset asset = (TextAsset)AssetDatabase.LoadAssetAtPath (assetPath, typeof(TextAsset));
			if (null == asset)
			{
				Debug.LogError("load " +filePath+" failed!");
			}
			string[] settingStr = asset.text.Split('\t');
			mSettingData = new SettingData();
            mSettingData.excelPath = ParseSettingStr(settingStr[0], isDefaultConfig);
			mSettingData.bytesPath = ParseSettingStr(settingStr [1], isDefaultConfig);
			mSettingData.csPath = ParseSettingStr(settingStr [2], isDefaultConfig);
			mSettingData.commdefPath = ParseSettingStr(settingStr [3], isDefaultConfig);
			mSettingData.protoToolsDir = ParseSettingStr(settingStr [4], isDefaultConfig);
		} else {
			EditorUtility.DisplayDialog ("提示", "配置文件读取失败,请检查配置文件是否存在!", "OK");
			mSettingData = new SettingData();
		}
	}

	public static void ReloadSettingData()
	{
		LoadPref ();
		LoadConfig ();
		if (null == mProtoSetting)
			mProtoSetting = (ProtoSetting)EditorWindow.GetWindow <ProtoSetting>();		
		mProtoSetting.settingData = mSettingData;
		mProtoSetting.Repaint ();
	}

	public static void SaveConfig(bool useDefaultConfig)
	{
		if (useDefaultConfig) {
			//设为默认配置
			configLevel = 1;
			PlayerPrefs.SetInt("Csv_ConfigLevel", configLevel);
			return;
		}

		if (mSettingData.isContentEmpty ()) {
			EditorUtility.DisplayDialog("提示", "请将内容选择完整！", "OK");
			return;
		}

		string filePath = configPath;
		if (File.Exists (filePath)) {
			File.Delete(filePath);
		} 
		Directory.CreateDirectory(filePath);
		filePath += "/ProtoConfig.txt";
        string data = mSettingData.excelPath + "\t" + mSettingData.bytesPath + "\t" + mSettingData.csPath
			+ "\t" + mSettingData.commdefPath + "\t" + mSettingData.protoToolsDir;
		File.WriteAllText (filePath, data);

		//设置为自定义配置
		configLevel = 2;
		PlayerPrefs.SetInt("Csv_ConfigLevel", configLevel);
	}

	public static void ExcuteModule(bool executAllFiles = true, bool useCsvExporter = false)
	{
		if (mSettingData.isContentEmpty ()) {
			EditorUtility.DisplayDialog("提示", "配置内容有误或不完整！", "OK");
			return;
		}
        bool executAllInConfigExcelDir = executAllFiles;
		//如果没有临时excel文件目录，有问题
        if (! Directory.Exists(tempExcelDir))
        {
            if (EditorUtility.DisplayDialog("提示", "请确认临时目录tempExcelDir生成成功，是否", "是", "否"))
            {
                executAllInConfigExcelDir = true;
            }
            else
            {
                return;
            }
		}
        int useOrignialDir = executAllInConfigExcelDir ? 1 : 0;
        int useCsvExport = useCsvExporter ? 1 : 0;
		string batDir = ParseStringForCmd(mSettingData.protoToolsDir);
		System.Diagnostics.Process process = null;
		try
		{
			process = new System.Diagnostics.Process();
			process.StartInfo.WorkingDirectory = batDir;
			process.StartInfo.FileName = "buildProto.bat";

            process.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6}", ParseStringForCmd(mSettingData.excelPath),
				ParseStringForCmd(mSettingData.bytesPath), ParseStringForCmd(mSettingData.csPath),
				ParseStringForCmd(mSettingData.commdefPath), ParseStringForCmd(mSettingData.protoToolsDir),
                useOrignialDir, useCsvExport);
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			process.WaitForExit();
		}
		catch(System.Exception e)
		{
			EditorUtility.DisplayDialog("执行失败:", e.Message + "\n" + e.StackTrace, "OK");
		}
	}

    private static void ExcuteCommit()
    {
        if (null == mSettingData || mSettingData.isContentEmpty())
        {
            EditorUtility.DisplayDialog("提示", "配置内容有误或不完整！", "OK");
            return;
        }
        string batDir = ParseStringForCmd(mSettingData.protoToolsDir);
        //合成commitPath
        string bytesPath = ParseStringForCmd(mSettingData.bytesPath);
        string genCodePath = ParseStringForCmd(mSettingData.csPath);
        string commitPath = bytesPath + "*" + genCodePath;
        System.Diagnostics.Process process = null;
        try
        {
            process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory = batDir;
            process.StartInfo.FileName = "svnUpdate.bat";
            process.StartInfo.Arguments = string.Format("{0}", commitPath);
            Debug.Log(commitPath);
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("执行失败:", e.Message + "\n" + e.StackTrace, "OK");
        }
    }

	static string ParseStringForCmd(string originStr)
	{
		return originStr.Replace("/", "\\");
	}

	static string ParseSettingStr(string configStr, bool useDefaultConfig)
	{
		if (useDefaultConfig) {
			// 加上头：E:/UnityDemos/UnityProtobufDemo/
			int index = Application.dataPath.LastIndexOf('/');
			string commonStr = Application.dataPath.Substring (0, index);
			return  commonStr + configStr;
		} else {
			return configStr;
		}
	}
}

public class ProtoSetting : EditorWindow {
	
	public bool useDefaultConfig = false;
	SettingData mSettingData;
	public SettingData settingData{ set{ mSettingData = value;} }

	Vector2 scrollPos;
	ProtoBuilder mBuilder;

	void OnGUI()
	{		
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
		//@asvo，这里不明白为什么在ExcuteModule()后 mSettingData有时候会为null。暂时不清楚原因，先这样强行处理，mSettingData为null的时候重新载入.
		if (mSettingData != null) {
			ShowConntent ();
		} else {
			ProtoBuildTool.ReloadSettingData ();
		}
		EditorGUILayout.EndScrollView ();
	}

	void ShowConntent()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		useDefaultConfig = GUILayout.Toggle (useDefaultConfig, "", GUILayout.Width (30));
		GUILayout.Label ("是否使用默认配置", GUILayout.Width (100));
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (20);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Excel文件目录", GUILayout.Width (100));
		GUILayout.Label (mSettingData.excelPath, GUILayout.Width (500));
		if (GUILayout.Button("设置", GUILayout.Width(80)))
		{
			mSettingData.excelPath = EditorUtility.OpenFolderPanel("选择excel路径", "", "");
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("生成bytes路径", GUILayout.Width (100));
		GUILayout.Label (mSettingData.bytesPath, GUILayout.Width (500));
		if (GUILayout.Button("设置", GUILayout.Width(80)))
		{
			mSettingData.bytesPath = EditorUtility.OpenFolderPanel("选择bytes路径", "", "");
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("生成C#代码路径", GUILayout.Width (100));
		GUILayout.Label (mSettingData.csPath, GUILayout.Width (500));
		if (GUILayout.Button("设置", GUILayout.Width(80)))
		{
			mSettingData.csPath = EditorUtility.OpenFolderPanel("选择C#代码路径", "", "");
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("commdef.proto所在目录", GUILayout.Width (100));
		GUILayout.Label (mSettingData.commdefPath, GUILayout.Width (500));
		if (GUILayout.Button("设置", GUILayout.Width(80)))
		{
			mSettingData.commdefPath = EditorUtility.OpenFolderPanel("commdef.proto所在目录", "", "");
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("tools目录路径", GUILayout.Width (100));
		GUILayout.Label (mSettingData.protoToolsDir, GUILayout.Width (500));
		if (GUILayout.Button("设置", GUILayout.Width(80)))
		{
			mSettingData.protoToolsDir = EditorUtility.OpenFolderPanel("tools目录路径", "", "");
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button("保存设置", GUILayout.Width(150)))
		{
			ProtoBuildTool.SaveConfig(useDefaultConfig);
		}
		if (GUILayout.Button("一键导出excel+重新生成代码和数据", GUILayout.Width(250)))
		{
			ProtoBuildTool.ExcuteModule();
		}
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (30);
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("打开生成列表窗口", GUILayout.ExpandWidth (true))) {
			BuildProtoListWindow bplw = (BuildProtoListWindow)EditorWindow.GetWindow<BuildProtoListWindow> ();
			bplw.titleContent.text = "生成列表窗口";
		    bplw.InitListWindow(mSettingData.excelPath, mSettingData.bytesPath, mSettingData.csPath,
		        mSettingData.protoToolsDir);
			bplw.Show ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}
}

public class SettingData
{
	public string excelPath = ""; //.xlsx Excel文件目录
	public string csvPath = "";	//.csv文件目录
	public string bytesPath = "";	//生成的.bytes文件目录
	public string csPath = "";	//生成的.cs文件目录
	public string commdefPath = "";	//commdef.proto的文件所在目录
	public string protoToolsDir = "";	//tools所在目录,buildProto.bat的批处理文件也放在这个目录下

	public bool useExcelExporter = false;	//默认不重新导出excel表

	//只要有一个为空，就认为SettingData是空
	public bool isContentEmpty()
	{
		return string.IsNullOrEmpty (excelPath)
			|| string.IsNullOrEmpty (bytesPath)
            || string.IsNullOrEmpty(csPath)
			|| string.IsNullOrEmpty(commdefPath)
			|| string.IsNullOrEmpty(protoToolsDir);
	}
}
