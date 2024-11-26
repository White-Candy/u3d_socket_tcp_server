

using System;
using UnityEngine;

public static class FPath
{
    public static string STORAGE_ROOT_PATH = Application.streamingAssetsPath; // Root Â·¾¶

    public static string STORAGE_USER = $"{STORAGE_ROOT_PATH}\\storage\\UsersInfo.json";
    public static string STORAGE_RESOURCE = $"{STORAGE_ROOT_PATH}\\storage\\rsCheck.json";
    public static string STORAGE_FACULTY = $"{STORAGE_ROOT_PATH}\\storage\\FacultyInfo.json";
    public static string STORAGE_MAJOR = $"{STORAGE_ROOT_PATH}\\storage\\MajorInfo.json";
    public static string STORAGE_CLASS = $"{STORAGE_ROOT_PATH}\\storage\\ClassInfo.json";
    public static string STORAGE_COLUMNS = $"{STORAGE_ROOT_PATH}\\storage\\ColumnsInfo.json";
    public static string STORAGE_COURSE = $"{STORAGE_ROOT_PATH}\\storage\\CourseInfo.json";
    public static string STORAGE_EXAMINE = $"{STORAGE_ROOT_PATH}\\storage\\ExamineInfo.json";
    public static string STORAGE_SCORE = $"{STORAGE_ROOT_PATH}\\storage\\ScoreInfo.json";
    public static string STORAGE_SOFTWARE = $"{STORAGE_ROOT_PATH}\\storage\\Software.json";
}