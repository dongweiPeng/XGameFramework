using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

using UnityEditor;

public class BuildProtoListWindow : BaseListWindow
{

    string toolsPath;
    string CreatedCsFilePath;
    //是否要同步csv数据到server的data下
    protected bool isNeedSyncCsv2Server = false;

    public void InitListWindow(string excelPath, string bytesPath, string csPath, string csvToolPath)
    {
        OriginFilePath = excelPath;
        CreatedFilePath = bytesPath;
        CreatedCsFilePath = csPath;
        CreatedFilePattern = "*.bytes";
        //OriginFilePattern = "*.xlsx";
        toolsPath = csvToolPath;

        InitData();
    }

    public override void Sync2ToolsProjectDir()
    {
        string bytesDataPath = EditorConfig.ToolsProjectDirRoot + "/Resources/ConfigData/";
        string genCodePath = EditorConfig.ToolsProjectDirRoot + "/Scripts/GenerateCodes/";
        EditorConfig.CopyDir(CreatedFilePath, bytesDataPath);
        EditorConfig.CopyDir(CreatedCsFilePath, genCodePath);
    }

    public override void OnChildGUI()
    {
        GUILayout.Space(5);
        GUI.color = Color.green;

        EditorGUILayout.BeginHorizontal();
        isNeedSyncCsv2Server = EditorGUILayout.Toggle("是否需要同步Csv文件到服务器data目录", isNeedSyncCsv2Server);
        EditorGUILayout.EndHorizontal();
    }

    protected override void InitData()
    {
        listShowingCreatedData.Clear();
        listShowingUnCreatedData.Clear();
        listSelectedData.Clear();

        string[] originFiles = Directory.GetFiles(OriginFilePath);
        string[] createdFiles = Directory.GetFiles(CreatedFilePath, CreatedFilePattern, SearchOption.AllDirectories);

        BaseWindowListData data = null;
        foreach (string ori in originFiles)
        {
            string oriFile = Path.GetFileNameWithoutExtension(ori);
            bool bCreated = false;
            foreach (string cre in createdFiles)
            {
                string creFile = Path.GetFileNameWithoutExtension(cre);
                if (creFile.Equals(oriFile))
                {
                    data = new BaseWindowListData(listShowingCreatedData.Count, Path.GetFileName(ori), false);
                    listShowingCreatedData.Add(data);
                    bCreated = true;
                    break;
                }
            }
            if (!bCreated)
            {
                data = new BaseWindowListData(listShowingUnCreatedData.Count, Path.GetFileName(ori), false);
                listShowingUnCreatedData.Add(data);
            }
        }
    }

    void OnProjectChange()
    {
        //刷新文件状态列表
        InitData();
    }

    protected override void ExecAll()
    {
        ProtoBuildTool.ExcuteModule(true, isNeedSyncCsv2Server);
    }

    protected override void ExecAllSelected()
    {
        base.ExecAllSelected();
        ExecPartialFiles(listSelectedData);
    }

    protected override void ExecAllUnCreate()
    {
        ExecPartialFiles(listShowingUnCreatedData);
    }

    void ExecPartialFiles(List<BaseWindowListData> partialDataList)
    {
        string tempExcelDir = ProtoBuildTool.tempExcelDir;
        //拷贝文件到临时目录
        if (Directory.Exists(tempExcelDir))
        {
            //清空目录下的excel文件
            DirectoryInfo directoryInfo = new DirectoryInfo(tempExcelDir);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                string name = fileInfo.Name;
                if (name.EndsWith(".xls") || name.EndsWith(".xlsx"))
                {
                    File.Delete(fileInfo.FullName);
                }
            }
        }
        else
        {
            Directory.CreateDirectory(tempExcelDir);
        }
        int fileCount = 0;
        foreach (var selectFile in partialDataList)
        {
            string srcFilePath = OriginFilePath + "/" + selectFile.name;
            string destFilePath = tempExcelDir + "/" + selectFile.name;
            if (File.Exists(srcFilePath))
            {
                ++fileCount;
                File.Copy(srcFilePath, destFilePath, true);
            }
        }
        if (0 == fileCount)
        {
            ProtoBuildTool.ShowDialog("没有任何文件被选中！");
            return;
        }
        ProtoBuildTool.ExcuteModule(false, isNeedSyncCsv2Server);

        //同步
        if (isNeedSyncCsv2Server)
        {
            string exprotCsvPath = toolsPath + "/python_protoc/csv/";
            //DirectoryInfo info = new DirectoryInfo(ProtoBuildTool.serverCsvDataDir);
            EditorConfig.CopyDir(exprotCsvPath, ProtoBuildTool.serverCsvDataDir.Replace("/", "\\"));
        }
    }
}
