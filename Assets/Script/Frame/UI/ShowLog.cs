

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShowLog : MonoBehaviour
{
    public static Text logText;

    public void Awake()
    {
        logText = GameObject.Find("Canvas/Image/LogText").GetComponent<Text>();
    }

    public async static void InputLog2Screen(string txt)
    {
        await UniTask.SwitchToMainThread();
        
        if (logText) 
            logText.text += "ServerLog># " + txt + "\n\n";
    }
}