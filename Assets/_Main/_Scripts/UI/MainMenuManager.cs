using UnityEngine;
using System.Collections;
using Settings;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {   
        [SerializeField] private Animator mainMenuAnimator;

        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;

        private bool _active = false;

        public Config config;

        void Awake()
        {
            Application.targetFrameRate = 60;
            config = ConfigManager.LoadConfig();
        }


        private void Start()
        {
            GetConfigValues();
            ChangeLocale(config.settings.localeID);
            MusicManager.Instance.PlayMainMenuMusic();
        }


        public void ExitGame()
        {
            Application.Quit();
        }

        public void GetConfigValues()
        {
            sfxSlider.value = config.settings.sfxvolume;
            musicSlider.value = config.settings.musicvolume;
        }

        public void UpdateSFX(float value)
        {
            config.settings.sfxvolume = value;
            ConfigManager.SaveConfig(config);
        }

        public void UpdateMusic(float value)
        {
            config.settings.musicvolume = value;
            MusicManager.UpdateMusicVolume();
            ConfigManager.SaveConfig(config);
        }
        
        #region UILogic

        public void UIMainMenu()
        {
            mainMenuAnimator.SetTrigger("MainMenu");
        }

        public void UICharacterSelection()
        {
            mainMenuAnimator.SetTrigger("SelectCharacter");
        }

        public void UILevelSelection()
        {
            mainMenuAnimator.SetTrigger("SelectMap");
        }

        public void UIControls()
        {
            mainMenuAnimator.SetTrigger("Controls");
        }

        public void UISettings()
        {   
            mainMenuAnimator.SetTrigger("Settings");
        }

        public void UICredits()
        {
            mainMenuAnimator.SetTrigger("Credits");
        }

        public void UIExit()
        {
            mainMenuAnimator.SetTrigger("Exit");
        }

        #endregion

        #region LevelSelection

        public void SelectLevel(int levelSelected)
        {
            switch (levelSelected)
            {
                case 1: SceneManager.LoadScene(levelSelected);
                    break;
                case 2: SceneManager.LoadScene(levelSelected);
                    break;
                case 3: SceneManager.LoadScene(levelSelected);
                    break;
                case 4: SceneManager.LoadScene(levelSelected);
                    break;
                case 5: SceneManager.LoadScene(levelSelected);
                    break;
                case 6: SceneManager.LoadScene(levelSelected);
                    break;
                case 7: SceneManager.LoadScene(levelSelected);
                    break;

                default: Debug.LogWarning("This level do not exists: " + levelSelected);
                    break;
            }
            MusicManager.Instance.StopMusic();
        }

        public void SelectRandomLevel()
        {
            int randomLevelSelected = Random.Range(1, 8);
            SceneManager.LoadScene(randomLevelSelected);
        }

        #endregion

        #region Localization

        public void ChangeLocale(int localeID)
        {
            if(_active)
            {
                return;
            }
            StartCoroutine(SetLocale(localeID));
        }

        private IEnumerator SetLocale(int localeID)
        {
            switch(localeID)
            {
                case 0: mainMenuAnimator.SetTrigger("English");
                    break;
                case 1: mainMenuAnimator.SetTrigger("Spanish");
                    break;
                case 2: mainMenuAnimator.SetTrigger("Portuguese");
                    break;
            }

            _active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            config.settings.localeID = localeID;
            ConfigManager.SaveConfig(config);
            _active = false;
        }

        #endregion
    }
}

