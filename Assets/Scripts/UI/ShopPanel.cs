/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 7/26/2018 5:34:55 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XFramework.UI;

public class ShopPanel : UIWndBase
{
    public DynamicInfinityListRenderer m_LoopList;
    List<int> m_datas = new List<int>();
    int m_SelectedData;


    private void Awake()
    {
        for (int i = 0; i < 20; ++i)
        {
            m_datas.Add(i);
        }
        m_LoopList.InitRendererList(OnSelectHandler, OnUpdateHandler);
        m_LoopList.SetDataProvider(m_datas);
    }

    void OnSelectHandler(DynamicInfinityItem item)
    {
        print("on select " + item.ToString());
        m_SelectedData = (int)item.GetData();
        m_LoopList.GetDataProvider().Remove(m_SelectedData);
        m_LoopList.RefreshDataProvider();

    }

    void OnUpdateHandler(DynamicInfinityItem item)
    {
        print("on upadte " + item.ToString());
    }
}
