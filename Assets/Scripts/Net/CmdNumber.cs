using UnityEngine;
using System.Collections;
namespace XFramework.Net
{
    public enum CmdNumber
    {
        None,
        LoginTest_CS = 3328,
        AccountRegisterClientCmd_CS,
        RequestPingClientCmd_C,
        RespondPingClientCmd_S,
        RespondMsgClientCmd_S,
        ServerReturnLoginSuccessLoginClientCmd_S,
    }
}