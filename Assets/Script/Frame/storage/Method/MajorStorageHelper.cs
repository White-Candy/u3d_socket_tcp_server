using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MajorStorageHelper : BaseStorageHelper
{
    /// <summary>
    /// 获取专业信息
    /// </summary>
    /// <returns></returns>
    public override async UniTask<object> GetInfo()
    {
        List<MajorInfo> major = new List<MajorInfo>();

        await UniTask.SwitchToMainThread();
        foreach (MajorInfo info in StorageHelper.Storage.majorInfo)
        {
            major.Add(info);   
        }
        return major;
    }

    /// <summary>
    /// 添加专业信息
    /// </summary>
    /// <param name="l_inf"></param>
    /// <returns></returns>
    public override async UniTask<object> AddInfo(object obj)
    {
        await UniTask.SwitchToMainThread();
        MajorInfo inf = obj as MajorInfo;
        if (StorageHelper.Storage.majorInfo.Find(x => x.MajorName == inf.MajorName) == null)
        {
            StorageHelper.Storage.majorInfo.Add(inf);
        }
        return StorageHelper.Storage.majorInfo;
    }

    /// <summary>
    /// 修改专业信息
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public override async UniTask<object> ReviseInfo(object obj)
    {
        await UniTask.SwitchToMainThread();

        MajorInfo inf = obj as MajorInfo;
        int index = StorageHelper.Storage.majorInfo.FindIndex(x => x.id == inf.id);
        if (index != -1)
        {
            StorageHelper.Storage.majorInfo[index] = inf;
            return StorageHelper.Storage.majorInfo;
        }
        return new List<MajorInfo>();
    }

    
    /// <summary>
    /// 删除专业信息
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public override object DeleteInfo(object obj)
    {
        MajorInfo inf = obj as MajorInfo;
        int idx = StorageHelper.Storage.majorInfo.FindIndex(x => x.id == inf.id);
        if (idx != -1)
        {
            StorageHelper.Storage.majorInfo.RemoveAt(idx);
            return StorageHelper.Storage.majorInfo;
        }
        return new List<MajorInfo>();
    }
}