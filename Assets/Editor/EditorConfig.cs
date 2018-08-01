/// <summary>
/// 编辑器配置文件
/// 1: 如何加入新的assetbundle打包文件夹
///   （Arts下）
///    第一步：加入 SourcePaths 源数组
///    第二步：加入 FolderNames 拷贝数组
///   （非Arts下，比如Resources下的文件；StateMachine）
///    第一步：需要将目录加入到assetbundle auto rename 中，参考statemachine的操作
///    第二步：加入 FolderNames 拷贝数组
/// 
/// 2: 其他人如何进行打包操作
///    第一步：修改 ToolsProjectDirRoot ＋　DevProjectDirRoot
/// 　　第二步：Ctrl + e 即可打包并拷贝到你指定的开发目录中
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class EditorConfig  {
	
	//源文件路径
	public static string ABSourceRoot = Application.dataPath + "/Arts/";
	//导出的路径
	public static string ABExportPath = Application.dataPath + "/StreamingAssets/";
    //模型对应的位置
	public static string ModelSourcePath = ABSourceRoot  +"AssetBundle/Model/";
    //dll对应位置
    public static string ABDllPath = Application.dataPath + "/Plugins/DLL/";
    public static string ABDllNewPath = Application.dataPath + "/Resources/Dll/";
	//源文件存放路径，如果需要新的文件进行打包必须放到这里面来
	public static string[] SourcePaths = new string[]{
		"Scene", "AssetBundle/Model", "AssetBundle/Effect", "AssetBundle/Effect/Scene_effect", "AssetBundle/Anim", "AssetBundle/CG", "UI"
	};

	/// <summary>
	/// 文件夹拷贝的路径
	/// </summary>
	public static string[] FolderNames = new string[]{
		"scene",  "assetBundle/model", "assetBundle/effect", "assetBundle/effect/scene_effect",  "assetBundle/anim", "assetBundle/cg" , "assetBundle/UI", "assetBundle/statemachine", 
	};


	//工具工程的目录
	public readonly static string ToolsProjectDirRoot = Application.dataPath.Substring(0, Application.dataPath.Length-7); //"GameControl\";
	//程序员的开发环境根目录
	public readonly static string DevProjectDirRoot = Application.dataPath;// "D \\Assets";

	//技能数据的输出路径
	public readonly static string SkillDataOutPutPath ="Assets/Resources/SkillMonitor/SkillData/";
	public readonly static string EffRootPath ="Assets/Arts/AssetBundle/Effect/";
	public readonly static string ModelRootPath ="Assets/Arts/AssetBundle/Model/";
	public readonly static string UIRootPath = "Assets/Arts/UI/";
    public readonly static string UIScriptsRootPath = "Assets/Scripts/";

	public static string GetoutputPath(bool bFolderOnly = false){
		BuildTarget target =  EditorUserBuildSettings.activeBuildTarget;
		string floderName = "";
		if(target == BuildTarget.StandaloneWindows){
			floderName = "Windows";
		}else if(target == BuildTarget.Android){
			floderName = "Android";		
		}else if(target == BuildTarget.iOS){
			floderName = "IOS";		
		}else{
			Debug.LogError("不存在的打包平台, 请程序员解决");
		}
		if(bFolderOnly){
			return floderName;
		}else{
			string outputPath = EditorConfig.ABExportPath + floderName+"/";
			return outputPath;	
		}
	}

	private static int copyCount=0;
	//拷贝到指定的文件夹中, 含有相同的则以覆盖的方式
	public static void CopyDir(string srcPath, string destPath){
		Debug.Log("[源文件夹] " + srcPath + " [目标文件夹]" + destPath);
		if(!Directory.Exists(srcPath)) return;

		if(!Directory.Exists(destPath)){
			Directory.CreateDirectory(destPath);
		}

		string[] fileList = Directory.GetFileSystemEntries(srcPath);
		foreach(string file in fileList){
			if(file.EndsWith(".meta"))continue;

			if(Directory.Exists(file)){
				CopyDir(file, destPath+Path.GetFileName(file));
			}else{
				//覆盖式拷贝
				copyCount++;
				Debug.Log("[拷贝文件] " + file);
				File.Copy(file, destPath+Path.GetFileName(file), true);
			}
		}
		Debug.Log("==========> 一共拷贝了 " + copyCount + ", 个文件");
		copyCount = 0;
	}


	public static void OpenScriptableObject<T>(string path, ref T outObject) where T : UnityEngine.ScriptableObject
	{
		T openObject = AssetDatabase.LoadAllAssetsAtPath(path) as T;
		outObject = openObject;
	}

	public static void SaveScriptableObject<T>(string path,  T saveObject) where T : UnityEngine.ScriptableObject
	{
		T saveT = AssetDatabase.LoadAllAssetsAtPath(path) as T;
		if (saveT == null)
		{
			saveT = ScriptableObject.CreateInstance<T>();
			EditorUtility.CopySerialized(saveObject, saveT);
			AssetDatabase.CreateAsset(saveT, path);
		}
		else
		{
			EditorUtility.CopySerialized(saveObject, saveT);

		}
	}

	public static void CreateTempScriptableObject<T>(string path, ref T createT) where T : UnityEngine.ScriptableObject
	{
		T tempT = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
		if (tempT == null)
		{
			createT = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(createT, path);
		}
		else
		{
			createT = tempT;
		}
	}
}
