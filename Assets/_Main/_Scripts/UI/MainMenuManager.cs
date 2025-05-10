using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels = new GameObject[5];
        //MainPanel 0
        //LevelSelectionPanel 1
        //ControlsPanel 2
        //SettingsPanel 3
        //CreditsPanel 4

        private bool _active = false;

        void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            //Reproducir Cinematica mientras carga la escena y la UI traducida
            UIMainMenu();

            int ID = PlayerPrefs.GetInt("LocaleKey",0);
            ChangeLocale(ID);
        }

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
            _active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            PlayerPrefs.SetInt("LocaleKey",localeID);
            _active = false;
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

    #region ShowUI

        public void LoadCinematic()
        {
            //Reproducir Cinematica
        }

        public void UIMainMenu()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[0].SetActive(true);
        }

        public void UILevelSelection()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[1].SetActive(true);
        }

        public void UIControls()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[2].SetActive(true);
        }

        public void UISettings()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[3].SetActive(true);
        }

        public void UICredits()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[4].SetActive(true);
        }

    #endregion

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

                default: Debug.LogWarning("This level do not exists: " + levelSelected);
                    break;
            }
        }

        public void SelectRandomLevel()
        {
            int randomLevelSelected = Random.Range(1, 5);
            SceneManager.LoadScene(randomLevelSelected);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}

