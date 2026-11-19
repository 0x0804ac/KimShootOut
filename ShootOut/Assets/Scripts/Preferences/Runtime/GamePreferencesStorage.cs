using System;
using System.IO;
using UnityEngine;

// File I/O is intentionally confined to explicit Save and startup Load.
public sealed class GamePreferencesStorage
{
    private const string fileName = "game-preferences-v1.json";

    private readonly string path;

    public GamePreferencesStorage()
    {
        path = Path.Combine(Application.persistentDataPath, fileName);
    }

    public GamePreferencesData Load()
    {
        if (!File.Exists(path))
        {
            return new GamePreferencesData();
        }

        try
        {
            GamePreferencesData data = JsonUtility.FromJson<GamePreferencesData>(File.ReadAllText(path));

            if (data != null && data.version == 1)
            {
                return data;
            }
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Failed to load preferences: {exception.Message}");
        }

        return new GamePreferencesData();
    }

    public bool Save(GamePreferencesData data)
    {
        try
        {
            File.WriteAllText(path, JsonUtility.ToJson(data, true));
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Failed to save preferences: {exception.Message}");
            return false;
        }
    }
}
