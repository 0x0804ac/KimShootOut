using UnityEngine;

//File I/O is intentionally confined to explicit Save and startup Load.
public sealed class PreferencesStorage
{
    private const string FILE_NAME = "game-preferences-v1.json";

    private readonly string path;

    public PreferencesStorage()
    {
        path = System.IO.Path.Combine(Application.persistentDataPath, FILE_NAME);
    }

    public PreferencesData Load()
    {
        if (!System.IO.File.Exists(path))
        {
            return new PreferencesData();
        }

        try
        {
            PreferencesData data = JsonUtility.FromJson<PreferencesData>(System.IO.File.ReadAllText(path));

            if (data != null && data.version == 1)
            {
                return data;
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning($"개인 설정을 불러오지 못했습니다: {exception.Message}");
        }

        return new PreferencesData();
    }

    public bool Save(PreferencesData data)
    {
        try
        {
            System.IO.File.WriteAllText(path, JsonUtility.ToJson(data, true));
            return true;
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning($"개인 설정을 저장하지 못했습니다: {exception.Message}");
            return false;
        }
    }
}
