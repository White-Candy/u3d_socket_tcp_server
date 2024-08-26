using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg expand_pkg)
    {
        Debug.Log("Check Event: " + expand_pkg.messPkg.ret);
        ResourcesInfo cli_info = JsonMapper.ToObject<ResourcesInfo>(expand_pkg.messPkg.ret);
        ResourcesInfo info = StorageHelper.GetThisInfoPkg(cli_info); // ��ȡ�ͻ����������Ŀid��ģ�����ֵ��ļ��汾��
        if (info == null)
        {
            Debug.Log("INFO NULL!");
            info = new ResourcesInfo(cli_info);
        }

        info.need_updata = cli_info.version_code == info.version_code ? false : true;
        Debug.Log($"{cli_info.version_code} || {info.version_code} || {info.need_updata}");
        string s_info = JsonMapper.ToJson(info);
        NetworkTCPServer.SendAsync(expand_pkg.socket, s_info, EventType.CheckEvent, OperateType.NONE);

        await UniTask.Yield();
    }
}
