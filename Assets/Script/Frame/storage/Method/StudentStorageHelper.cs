using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StudentStorageHelper : BaseStorageHelper
{
    /// <summary>
    /// 获取学生的信息
    /// </summary>
    /// <returns></returns>
    public override async UniTask<object> GetInfo()
    {
        List<UserInfo> students = new List<UserInfo>();

        await UniTask.SwitchToMainThread();
        foreach (UserInfo info in StorageHelper.Storage.userInfos)
        {
            if (info.level == 0)
            {
                students.Add(info);
            }
        }
        return students;
    }

    /// <summary>
    /// 添加学生信息
    /// </summary>
    /// <param name="l_inf"></param>
    /// <returns></returns>
    public override async UniTask<object> AddInfo(object obj)
    {
        List<UserInfo> l_inf = obj as List<UserInfo>;
        foreach (var inf in l_inf)
        {
            await StorageHelper.Register(inf);
        }
        return StorageHelper.Storage.userInfos;
    }

    /// <summary>
    /// 修改学生信息
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public override async UniTask<object> ReviseInfo(object obj)
    {
        await UniTask.SwitchToMainThread();

        UserInfo inf = obj as UserInfo;
        int index = StorageHelper.Storage.userInfos.FindIndex(x => x.userName == inf.userName);
        if (index != -1)
        {
            StorageHelper.Storage.userInfos[index] = inf;
            return StorageHelper.Storage.userInfos;
        }
        return new List<UserInfo>();
    }

    
    /// <summary>
    /// 删除学生信息
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public override object DeleteInfo(object obj)
    {
        UserInfo inf  = obj as UserInfo;
        int idx = StorageHelper.Storage.userInfos.FindIndex(x => x.userName == inf.userName);
        if (idx != -1)
        {
            StorageHelper.Storage.userInfos.RemoveAt(idx);
            return StorageHelper.Storage.userInfos;
        }
        return new List<UserInfo>();
    }
}