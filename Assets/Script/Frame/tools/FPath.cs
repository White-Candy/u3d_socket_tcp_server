

using System;
using UnityEngine;

public static class FPath
{
    public static string STORAGE_ROOT_PATH = Application.streamingAssetsPath; // Root Â·¾¶

    public static string STORAGE_USER = $"{STORAGE_ROOT_PATH}\\Storage\\UsersInfo.json";
    public static string STORAGE_RESOURCE = $"{STORAGE_ROOT_PATH}\\Storage\\rsCheck.json";
    public static string STORAGE_FACULTY = $"{STORAGE_ROOT_PATH}\\Storage\\FacultyInfo.json";
    public static string STORAGE_MAJOR = $"{STORAGE_ROOT_PATH}\\Storage\\MajorInfo.json";
    public static string STORAGE_CLASS = $"{STORAGE_ROOT_PATH}\\Storage\\ClassInfo.json";
    public static string STORAGE_COLUMNS = $"{STORAGE_ROOT_PATH}\\Storage\\ColumnsInfo.json";
    public static string STORAGE_COURSE = $"{STORAGE_ROOT_PATH}\\Storage\\CourseInfo.json";
    public static string STORAGE_EXAMINE = $"{STORAGE_ROOT_PATH}\\Storage\\ExamineInfo.json";
    public static string STORAGE_SCORE = $"{STORAGE_ROOT_PATH}\\Storage\\ScoreInfo.json";
    public static string STORAGE_SOFTWARE = $"{STORAGE_ROOT_PATH}\\Storage\\Software.json";
    public static string STORAGE_MCNT = $"{STORAGE_ROOT_PATH}\\Storage\\ModuleCount.json";
    public static string STORAGE_USTIME = $"{STORAGE_ROOT_PATH}\\Storage\\UseTime.json";
}