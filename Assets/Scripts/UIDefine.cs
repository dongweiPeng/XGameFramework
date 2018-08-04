using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//客户端的预支体必须和这个相同
public enum WindowID : int{
	Invalid =0,
    LoginPanel,
	MainCityPanel,
    BagUI,
	DressPanel,
	Msgbox,
	BattlePanel,
    BattleStartPanel,
    BattleResultPanel,
    PhonePanel,
    PetPanel,
    HaoGanPanel,
    ShopPanel= 1004,
}

//消息
public enum WindowMsgID:int{
	None,
	ShowLoadingTips,
	ShowGuide,
	ShowMap,
	RefreshTroop,
	UpgradeStar, //升星
	ShowWpTip,
	CloseWpTip,
	ShowZhuanquan,
}


//窗口状态
public enum WindowStatus{
	Active,
	Inactive,
	Gray, //链状处于灰的状态
}

public enum WindowColliderType{
	Default, //默认窗口不带碰撞 
	Collider, //有碰撞的
}
public enum WindowOpenType{
	None,
	Left2Rgiht,
	Right2Left,
	Fade,
	Scale,
}

public enum WindowBarType{
	Default,
}

public enum MessageBoxType{
	OK,
	OK_Cancle,
	OK_Other_Cancle,
}

public enum MessageBoxEvent{
	Ok,
	Other,
	Cancle
}

