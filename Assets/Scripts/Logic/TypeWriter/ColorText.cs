/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/7/2018 6:42:09 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorText  {
    public string text;
    public List<ColorInfo> colorList = new List<ColorInfo>();

    public int colorHeadLength = 17;
    public int colorTailLength = 8;
    public ColorText(string txt) {
        string msg = string.Empty;
        if (txt.Contains("</color>")) {
            ColorInfo colorInfo = null;
            char[] array = txt.ToCharArray();
            bool colorHead = true;
            int len = 0;
            for (int i=0; i<array.Length; i++) {
                if (array[i] == '<')
                {
                    if (colorHead)
                    {
                        i += colorHeadLength;
                        colorHead = false;
                    }
                    else
                    {
                        i += colorTailLength;
                        colorHead = true;
                    }
                }else
                {
                    msg += array[i];
                }
            }
        }
    }
}

public class ColorInfo
{
    public int index;
    public int length;
    public string color;
}