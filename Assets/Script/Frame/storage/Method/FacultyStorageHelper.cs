using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FacultyStorageHelper : BaseStorageHelper
{
    /// <summary>
    /// 获取学院信息
    /// </summary>
    /// <returns></returns>
    public override async UniTask<object> GetInfo()
    {
        List<FacultyInfo> faculies = new List<FacultyInfo>();

        await UniTask.SwitchToMainThread();
        foreach (FacultyInfo info in StorageHelper.Storage.faculiesInfo)
        {
            faculies.Add(info);   
        }
        return faculies;
    }

    /// <summary>
    /// 添加学院信息
    /// </summary>
    /// <param name="l_inf"></param>
    /// <returns></returns>
    public override async UniTask<object> AddInfo(object obj)
    {
        await UniTask.SwitchToMainThread();
        FacultyInfo inf = obj as FacultyInfo;
        if (StorageHelper.Storage.faculiesInfo.Find(x => x.Name == inf.Name) == null)
        {
            StorageHelper.Storage.faculiesInfo.Add(inf);
        }
        return StorageHelper.Storage.faculiesInfo;
    }

    /// <summary>
    /// 修改学院信息
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public override async UniTask<object> ReviseInfo(object obj)
    {
        await UniTask.SwitchToMainThread();

        FacultyInfo inf = obj as FacultyInfo;
        Debug.Log($"{inf.id} | {inf.Name} | {inf.TeacherName}");
        int index = StorageHelper.Storage.faculiesInfo.FindIndex(x => x.id == inf.id);
        if (index != -1)
        {
            StorageHelper.Storage.faculiesInfo[index] = inf;
            return StorageHelper.Storage.faculiesInfo;
        }
        return new List<FacultyInfo>();
    }

    
    /// <summary>
    /// 删除学院信息
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public override object DeleteInfo(object obj)
    {
        FacultyInfo inf = obj as FacultyInfo;
        int idx = StorageHelper.Storage.faculiesInfo.FindIndex(x => x.id == inf.id);
        if (idx != -1)
        {
            StorageHelper.Storage.faculiesInfo.RemoveAt(idx);
            return StorageHelper.Storage.faculiesInfo;
        }
        return new List<FacultyInfo>();
    }
}