
using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EncryptionLuncher: MonoBehaviour
{
    private string m_BatFile = "C:/Program Files/Luncher.bat";
    public Text hintText;

    public void Start()
    {
        ExamineFile();
    }

    public async void ExamineFile()
    { 
        if (!File.Exists(m_BatFile)) 
        {
            ShowHint("警告，软件未激活！");
            await UniTask.Delay(2000);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif          
        }
        else
        {
            ShowHint("驱动已启动！");
        }
    }

    public void ShowHint(string message)
    {
        hintText.text = "";
        hintText.text = message;
    }
}