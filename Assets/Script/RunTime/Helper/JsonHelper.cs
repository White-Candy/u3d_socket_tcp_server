using Cysharp.Threading.Tasks;
using LitJson;

public class JsonHelper
{
    /// <summary>
    /// 异步去做ToJson
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static async UniTask<string> AsyncToJson(object obj)
    {
        string result = "";
        await UniTask.RunOnThreadPool(() => 
        {
            result = JsonMapper.ToJson(obj);
        });
        return result;
    }
}