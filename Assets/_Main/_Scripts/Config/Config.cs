using System;
using System.IO;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class Config
    {
        public GameSettings settings = new();
    }
    
    [Serializable]
    public class GameSettings
    {
        public float sfxvolume = .65f;
        public float musicvolume = .65f;
        public int localeID = 0;
    }


    public class ConfigManager
    {
        private static readonly string ConfigPath = Application.persistentDataPath + "/Config.json";
        private static Config _config;

        public static Config LoadConfig()
        {
            if (_config != null) return _config;
            if (!File.Exists(ConfigPath))
            {
                Debug.LogWarning("No config file found at " + ConfigPath + " - Creating new config with default ally levels");
                var defaultConfig = new Config();
                return defaultConfig;
            }
            
            var json = File.ReadAllText(ConfigPath);
            _config = JsonUtility.FromJson<Config>(json);
            return _config;
        }

        public static void SaveConfig(Config config)
        {
            var json = JsonUtility.ToJson(config, true);
            File.WriteAllText(ConfigPath, json);
            _config = config;
        }
    }
}
