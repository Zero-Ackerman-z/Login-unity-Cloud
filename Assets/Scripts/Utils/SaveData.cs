using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveData
{
    public static void Save(string fileName, string json)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
    }

    public static string Load(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        return null;
    }

    public static void Delete(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
