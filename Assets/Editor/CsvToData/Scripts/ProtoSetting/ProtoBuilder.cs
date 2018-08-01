using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ProtoBuilder {

	public List<ProtoFolderElement> folderElements = new List<ProtoFolderElement> ();
	public bool isUnFold = false;

	string bytesPath = "";
	string csvPath = "";

	public ProtoBuilder(string bytes_path, string csv_path)
	{
		bytesPath = bytes_path;
		csvPath = csv_path;

		initCsvDir ();
	}

	public void ShowEditorGUI()
	{
		EditorGUILayout.BeginHorizontal ();
		isUnFold = EditorGUILayout.Foldout (isUnFold, "csv文件列表");
		if (GUILayout.Button("刷新", GUILayout.Width(80)))
		{
			initCsvDir ();
		}
		EditorGUILayout.EndHorizontal ();

		ShowFolderElements ();

	}

	void ShowFolderElements()
	{
		if (isUnFold) {
			EditorGUILayout.BeginVertical ();
			foreach (var folderElement in folderElements) {
				EditorGUILayout.BeginHorizontal ();
				folderElement.isReadToBuild = GUILayout.Toggle (folderElement.isReadToBuild, folderElement.GetStateString () + folderElement.csvFileName, GUILayout.Width (200));
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndVertical ();

			GUILayout.Space (10);
			if (GUILayout.Button("选中所有", GUILayout.Width(80)))
			{
				SelectAll (true);
			}
			if (GUILayout.Button("清除所有", GUILayout.Width(80)))
			{
				SelectAll (false);
			}
			if (GUILayout.Button("选中所有新增文件", GUILayout.Width(100)))
			{
				SelectAllNew ();
			}
			if (GUILayout.Button ("批量生成选中文件", GUILayout.Width (150))) {
				ExcuteSelectFiles ();
			}
		}
		GUILayout.Space (20);
	}

	void SelectAll(bool setSelect)
	{
		foreach (var ele in folderElements) {
			ele.isReadToBuild = setSelect;
		}
	}

	void SelectAllNew()
	{
		foreach (var ele in folderElements) {
			if (ele.csvState == CsvState.State_New) {
				ele.isReadToBuild = true;
			}
		}
	}

	void ExcuteSelectFiles()
	{
        ////save file list
        //StringBuilder sb = new StringBuilder();
        //foreach (var element in folderElements) {
        //    if (element.isReadToBuild)
        //        sb.AppendLine (element.csvFileName);
        //}
        //string writeStr = sb.ToString ();
        //Debug.Log (writeStr);
        //using (FileStream fs = File.Open (ProtoBuildTool.selectFilePath, FileMode.Create)) {
        //    byte[] info = new System.Text.UTF8Encoding (true).GetBytes(writeStr);
        //    fs.Write (info, 0, info.Length);
        //}

        //ProtoBuildTool.ExcuteModuleSelectFiles ();
	}

	//读取csv目录，计算folderElement状态
	void initCsvDir()
	{
		folderElements.Clear ();

		Dictionary<string, FileInfo> bytesName2File = new Dictionary<string, FileInfo> ();
		DirectoryInfo bytesDirInfo = new DirectoryInfo (bytesPath);
		foreach (var file in bytesDirInfo.GetFiles("*.bytes")) {
			string fileName = ProtoFileUtils.GetFileNameWithNoExtension (file.Name);
			bytesName2File.Add (fileName, file);
		}

		DirectoryInfo dirInfo = new DirectoryInfo (csvPath);
		foreach (var file in dirInfo.GetFiles("*.csv")) {
			ProtoFolderElement element = new ProtoFolderElement ();
			string csvFileName = ProtoFileUtils.GetFileNameWithNoExtension (file.Name);
			element.csvFileName = csvFileName;
			//如果byteDict里面没有，说明是新增的csv文件
			if (!bytesName2File.ContainsKey (csvFileName)) {
				element.SetState (CsvState.State_New);
			} else {
				element.SetState (CsvState.State_NoChange);
			}
			folderElements.Add (element);
		}
		folderElements.Sort (new FolderElementComparator ());
	}
}

public enum CsvState
{
	State_New,
	State_Update,
	State_NoChange
}

public class ProtoFolderElement
{
	CsvState mState;
	public CsvState csvState{ get { return mState; } }

	public string csvFileName;
	public bool isReadToBuild = false;	//是否加入生成列表

	public void SetState(CsvState state)
	{
		mState = state;
	}

	public string GetStateString()
	{
		string stateStr = "";
		switch (mState) {
		case CsvState.State_New:
			stateStr = "(新增) ";
			break;
		default:
			break;
		}
		return stateStr;
	}
}

public class FolderElementComparator : IComparer<ProtoFolderElement>
{
	public int Compare(ProtoFolderElement element1, ProtoFolderElement element2)
	{
		if ((int)element1.csvState > (int)element2.csvState) {
			return 1;
		}
		else if ((int)element1.csvState < (int)element2.csvState) {
			return -1;
		}
		else {
			return 0;
		}

	}
}

public class ProtoFileUtils
{
	public static string GetFileNameWithNoExtension(string fileName)
	{
		int dotIndex = fileName.LastIndexOf('.');
		return fileName.Substring (0, dotIndex);
	}
}