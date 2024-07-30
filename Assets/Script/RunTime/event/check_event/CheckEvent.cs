using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        Debug.Log("Check Event");
        AsyncExpandPkg expand_pkg = objs[0] as AsyncExpandPkg;
        Debug.Log(expand_pkg.messPkg.ret);
        ResourcesInfo cli_info = JsonMapper.ToObject<ResourcesInfo>(expand_pkg.messPkg.ret);
        ResourcesInfo info = StorageExpand.GetThisInfoPkg(cli_info); // 获取客户端请求的项目id和模块名字的文件版本号
        if (info == null)
        {
            Debug.Log("INFO NULL!");
            info = new ResourcesInfo(cli_info);
        }

        info.need_updata = cli_info.version_code == info.version_code ? false : true;
        Debug.Log($"{cli_info.version_code} || {info.version_code} || {info.need_updata}");
        string s_info = JsonMapper.ToJson(info);
        NetworkTCPServer.SendAsync(expand_pkg.socket, s_info, EventType.CheckEvent);
    }
}
