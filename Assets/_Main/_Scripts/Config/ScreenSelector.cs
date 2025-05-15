using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.Config
{
    public class ScreenSelector : MonoBehaviour
    {
        public Dropdown screenDropdown;
        public Dropdown fullscreenDropdown; 

        private List<DisplayInfo> displays = new List<DisplayInfo>();

        void Start()
        {
            Screen.GetDisplayLayout(displays);
            SetupMonitorDropdown();
            SetupFullscreenDropdown();
            ApplySavedSettings();
        }

        void SetupMonitorDropdown()
        {
            screenDropdown.ClearOptions();
            List<string> screenOptions = new List<string>();
        
            for (int i = 0; i < displays.Count; i++)
            {
                screenOptions.Add($"Monitor {i + 1} ({displays[i].width}x{displays[i].height})");
            }

            screenDropdown.AddOptions(screenOptions);
        }

        void SetupFullscreenDropdown()
        {
            fullscreenDropdown.ClearOptions();
            fullscreenDropdown.AddOptions(new List<string>
            {
                "Exclusive Fullscreen",
                "Borderless Window",
                "Windowed"
            });
        }

        void ApplySavedSettings()
        {
            Settings.Config config = ConfigManager.LoadConfig();
        
            int savedMonitor = Mathf.Clamp(config.settings.selectedMonitor, 0, displays.Count - 1);
            screenDropdown.value = savedMonitor;
            ChangeScreen(savedMonitor);

            int savedFullscreen = Mathf.Clamp(config.settings.fullscreenMode, 0, 2);
            fullscreenDropdown.value = savedFullscreen;
            ChangeFullscreenMode(savedFullscreen);
        }

        public void ChangeScreen(int screenIndex)
        {
            if (screenIndex >= 0 && screenIndex < displays.Count)
            {
                Screen.MoveMainWindowTo(displays[screenIndex], new Vector2Int(0, 0));
            
                Settings.Config config = ConfigManager.LoadConfig();
                config.settings.selectedMonitor = screenIndex;
                ConfigManager.SaveConfig(config);
            }
        }

        public void ChangeFullscreenMode(int modeIndex)
        {
            FullScreenMode mode = (FullScreenMode)modeIndex;
            Screen.fullScreenMode = mode;

            Settings.Config config = ConfigManager.LoadConfig();
            config.settings.fullscreenMode = modeIndex;
            ConfigManager.SaveConfig(config);
        }
    }
}