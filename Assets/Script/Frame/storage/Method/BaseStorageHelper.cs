
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public abstract class BaseStorageHelper
{
    /// <summary>
    /// 获取
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask<object> GetInfo()
    {
        await UniTask.Yield();
        return new object();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public virtual async UniTask<object> AddInfo(object inf)
    {
        await UniTask.Yield();
        return new object();
    }

    /// <summary>
    ///  修改
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public virtual async UniTask<object> ReviseInfo(object inf)
    {
        await UniTask.SwitchToMainThread();
        return new object();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public virtual object DeleteInfo(object inf)
    {
        return new object();
    }
}