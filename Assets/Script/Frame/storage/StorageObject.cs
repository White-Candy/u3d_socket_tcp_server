using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Storage/Storage_clump")]
public class StorageObject : ScriptableObject
{
    public List<ResourcesInfo> rsCheck = new List<ResourcesInfo>();
}

/// <summary>
/// �ͻ�����Դ����ǰ����м������
/// ����ͻ��˵İ汾��ͷ������Ĳ�һ������Ҫ����
/// </summary>
[Serializable]
public class ResourcesInfo
{
    public string id;
    public string moduleName;
    public string version_code;
}

