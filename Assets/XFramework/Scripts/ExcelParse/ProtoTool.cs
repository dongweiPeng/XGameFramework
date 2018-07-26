using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using ProtoBuf;
//using for BuildMap
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf.Meta;
using dbc;
using XFramework;
public class ProtoTool {
			#if USE_PROTODLL
	private static WdjDTOSerializer dbcSerializer = new WdjDTOSerializer();
			#endif
	public static byte[] Serialize<T>(T t)
	{
		#if USE_PROTODLL
		MemoryStream ms = new MemoryStream();
		dbcSerializer.Serialize(ms,t);
		return ms.ToArray();
		#else
		using (MemoryStream ms = new MemoryStream()) {
			Serializer.Serialize<T>(ms, t);
			return ms.ToArray();
		}
		#endif
	}

	public static T DeSerialize<T>(byte[] content)
	{
		#if USE_PROTODLL
		MemoryStream ms = new MemoryStream(content);
		T t = (T)dbcSerializer.Deserialize(ms,null,typeof(T));
		return t;
		#else
        using (MemoryStream ms = new MemoryStream(content))
        {
            T t = Serializer.Deserialize<T>(ms);
            return t;
        }
		#endif
	}
		
	public static T Load<T>(string fileName)where T:ProtoBuf.IExtensible
	{
		TextAsset textAsset = null;
		#if PC_RUN_ANDROID || PC_RUN_IOS
        textAsset = ResourceManager.GetInstance().LoadText("ConfigData/" + fileName, fileName);
			if (null == textAsset) {
			  CommonTools.MustLog("prototool bundle 加载失败, 没有对应的资源: " + fileName);
			}
		#else
		if(Application.isEditor || !Application.isMobilePlatform){
			string protoPath = "ConfigData/"+fileName;
			textAsset = XFramework.AssetBundlePacker.ResourcesManager.Load<TextAsset>(protoPath) as TextAsset;	
		}else{
            //textAsset = ResourceManager.LoadText("ConfigData/" + fileName, fileName);
            //if (null == textAsset)
            //{
            //    ///CommonTools.MustLog("prototool bundle 加载失败, 没有对应的资源: " + fileName);
            ////}
            string protoPath = "ConfigData/" + fileName;
            textAsset = XFramework.AssetBundlePacker.ResourcesManager.Load<TextAsset>(protoPath) as TextAsset;	
		}
		#endif
	    T t = default(T);
	    try
	    {
	        t = ProtoTool.DeSerialize<T>(textAsset.bytes);
	    }
	    catch (ProtoException proex)
	    {
			Debug.LogError(string.Format("解析配表：<color=red>{0}</color>出错,请相关人员检查配表! + ex:"+proex.Message, fileName));
	    }
	    catch (Exception ex)
	    {
			Debug.LogError(string.Format("解析配表：<color=red>{0}</color>出错,未知异常!exception:{1}", fileName,ex.Message));
	    }
		return t;
	}

	public static Dictionary<Tkey, T> BuildMap<Tkey,T>(string keyName, List<T> tlist)
	{
		System.Type protoType = typeof(T);
		PropertyInfo properInfo = protoType.GetProperty(keyName, typeof(Tkey));
		if (null == properInfo) {
			Debug.LogError ("不存在的Key名:" + keyName);
		}
		if (!(properInfo.PropertyType == typeof(Tkey))) {
			Debug.LogError ("KeyName类型不匹配!");
		}
		Dictionary<Tkey, T> buildmap = new Dictionary<Tkey, T> ();
		foreach (var entry in tlist) {
			Tkey mapkey = (Tkey)properInfo.GetValue (entry, null);
            if (buildmap.ContainsKey(mapkey))
                Debug.LogError(string.Format("字段名:{0},有重复值：{1}", properInfo.Name, mapkey));
			buildmap.Add (mapkey, entry);
		}

		return buildmap;
	}
}
