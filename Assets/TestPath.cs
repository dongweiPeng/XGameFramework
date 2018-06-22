/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 6/21/2018 6:11:51 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestPath : MonoBehaviour {
    public Text debug_text;
    public Text debug2_text;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    void Start() {
        string filename = "/AssetBundle/ResourcesManifest.cfg";
        string androidStream = Application.streamingAssetsPath + filename;
        string androidPesist = Application.persistentDataPath + filename;
       
        string androidstream1 = "jar:file://" + Application.dataPath + "!/assets" + filename;
        string androidPesist1 = " /data/data/com.game.xframework/files" + filename;

        debug_text.text = androidStream + "\n" + androidPesist + " \n" + androidstream1 + "\n" + androidPesist1;


        btn1.onClick.AddListener(() => StartCoroutine(Copy(androidStream, androidPesist)));
        btn2.onClick.AddListener(() => StartCoroutine(Copy(androidStream, androidPesist1)));
        btn3.onClick.AddListener(() => StartCoroutine(Copy(androidstream1, androidPesist1)));
        btn4.onClick.AddListener(() => StartCoroutine(Copy(androidstream1, androidPesist)));
    }


    IEnumerator Copy(string src, string dest) {
        Debug.Log("==========>");
        using (WWW w = new WWW(src))
        {
            yield return w;
            debug2_text.text = string.Format("错误信息{0}", w.error);
            Debug.Log("拷贝中....错误信息>>" + w.error);
            if (string.IsNullOrEmpty(w.error))
            {
                while (w.isDone == false)
                    yield return null;
                zcode.FileHelper.CopyStreamingAssetsToFile(src, dest);
                Debug.Log("成功准备拷贝数据：" + w.text + " 到" + dest);
            }
            else
            {
                Debug.LogWarning(w.error);
            }
            StopAllCoroutines();
        }
    }
    

  
}
