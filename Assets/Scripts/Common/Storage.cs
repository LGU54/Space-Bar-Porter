using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    private static Storage storage;
    public Storage()
    {
        storage = null ?? new Storage();
    }

    public static Storage GetStorage()
    {
        if (storage == null)
        {
            storage = new Storage();
        }
        return storage;
    }
}

public class UserPreference
{
    public static void Save(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static void Save(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static T Read<T>(string key) where T : struct
    {
        if (!Contains(key))
            return (T) Convert.ChangeType(null, typeof(T));
        
        switch(typeof(T).ToString())
        {
            case "System.Int32":
                return (T) Convert.ChangeType(PlayerPrefs.GetInt(key), typeof(T));
            case "System.Single":
                return (T) Convert.ChangeType(PlayerPrefs.GetFloat(key), typeof(T));
            case "System.String":
                return (T) Convert.ChangeType(PlayerPrefs.GetString(key), typeof(T));
            default: 
                throw new Exception("Type not supported");
        };
    }

    public static bool Contains(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void ClearAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
