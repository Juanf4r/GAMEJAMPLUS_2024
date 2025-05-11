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
        [SerializeField] private GameObject[] panels = new GameObject[7];
        //MainPanel 0
        //SelectCharacterPanel 1
        //LevelSelectionPanel 2
        //ControlsPanel 3
        //SettingsPanel 4
        //CreditsPanel 5
        //ConfirmExitPanel 6

        [Header("MainMenu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;

        [Space]

        [Header("SelectCharacter Buttons")]
        [SerializeField] private Button backButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button leftArrowPlayer1;
        [SerializeField] private Button rightArrowPlayer1;
        [SerializeField] private Button leftArrowPlayer2;
        [SerializeField] private Button rightArrowPlayer2;

        [Space]

        [Header("SelectLevel Buttons")]
        [SerializeField] private Button backLevelButton;
        [SerializeField] private Button map1Button;
        [SerializeField] private Button map2Button;
        [SerializeField] private Button map3Button;
        [SerializeField] private Button map4Button;
        [SerializeField] private Button randomMapButton;

        [Space]

        [Header("ControlsButtons")]
        [SerializeField] private Button backControlsButton;

        [Space]

        [Header("SettingsButtons")]
        [SerializeField] private Button backSettingsButton;
        [SerializeField] private Button englishLanguageButton;
        [SerializeField] private Button spanishLanguageButton;
        [SerializeField] private Button portugueseLanguageButton;

        [Space]

        [Header("CreditsButtons")]
        [SerializeField] private Button backCreditsButton;

        /*[SerializeField] private Button playButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;*/

        [Header("ExitBUttons")]
        [SerializeField] private Button backExitButton;
        [SerializeField] private Button exitGameButton;
        private bool _active = false;

        public Config config;

        void Awake()
        {
            Application.targetFrameRate = 60;
            config = ConfigManager.LoadConfig();
        }

        private void OnEnable() 
        {
            //MainMenuButtons
            playButton.onClick.AddListener(UICharacterSelection);
            controlsButton.onClick.AddListener(UIControls);
            settingsButton.onClick.AddListener(UISettings);
            creditsButton.onClick.AddListener(UICredits);
            exitButton.onClick.AddListener(UIExit);

            //SelectCharacterButtons
            backButton.onClick.AddListener(UIMainMenu);
            nextButton.onClick.AddListener(UILevelSelection);
            leftArrowPlayer1.onClick.AddListener(() => ChangeCharacterLeft(true));
            rightArrowPlayer1.onClick.AddListener(() => ChangeCharacterRight(true));
            leftArrowPlayer2.onClick.AddListener(() => ChangeCharacterLeft(false));
            rightArrowPlayer2.onClick.AddListener(() => ChangeCharacterRight(false));

            //SelectLevelButtons
            backLevelButton.onClick.AddListener(UICharacterSelection);
            map1Button.onClick.AddListener(() => SelectLevel(1));
            map2Button.onClick.AddListener(() => SelectLevel(2));
            map3Button.onClick.AddListener(() => SelectLevel(3));
            map4Button.onClick.AddListener(() => SelectLevel(4));
            randomMapButton.onClick.AddListener(SelectRandomLevel);

            //ControlsButton
            backControlsButton.onClick.AddListener(UIMainMenu);

            //SettingsButton
            backSettingsButton.onClick.AddListener(UIMainMenu);
            spanishLanguageButton.onClick.AddListener(() => ChangeLocale(0));
            englishLanguageButton.onClick.AddListener(() => ChangeLocale(1));
            portugueseLanguageButton.onClick.AddListener(() => ChangeLocale(2));

            //CreditsButtons
            backCreditsButton.onClick.AddListener(UIMainMenu);

            backExitButton.onClick.AddListener(UIMainMenu);
            exitButton.onClick.AddListener(UIExit);
        }

        private void OnDisable()
        {
            //MainMenuButtons
            playButton.onClick.AddListener(UICharacterSelection);
            controlsButton.onClick.AddListener(UIControls);
            settingsButton.onClick.AddListener(UISettings);
            creditsButton.onClick.AddListener(UICredits);
            exitButton.onClick.AddListener(UIExit);

            //SelectCharacterButtons
            backButton.onClick.RemoveListener(UIMainMenu);
            nextButton.onClick.RemoveListener(UILevelSelection);
            leftArrowPlayer1.onClick.RemoveListener(() => ChangeCharacterLeft(true));
            rightArrowPlayer1.onClick.RemoveListener(() => ChangeCharacterRight(true));
            leftArrowPlayer2.onClick.RemoveListener(() => ChangeCharacterLeft(false));
            rightArrowPlayer2.onClick.RemoveListener(() => ChangeCharacterRight(false));

            //SelectLevelButtons
            backLevelButton.onClick.RemoveListener(UICharacterSelection);
            map1Button.onClick.RemoveListener(() => SelectLevel(1));
            map2Button.onClick.RemoveListener(() => SelectLevel(2));
            map3Button.onClick.RemoveListener(() => SelectLevel(3));
            map4Button.onClick.RemoveListener(() => SelectLevel(4));
            randomMapButton.onClick.RemoveListener(SelectRandomLevel);

            //ControlsButton
            backControlsButton.onClick.RemoveListener(UIMainMenu);

            //SettingsButton
            backSettingsButton.onClick.RemoveListener(UIMainMenu);
            spanishLanguageButton.onClick.RemoveListener(() => ChangeLocale(0));
            englishLanguageButton.onClick.RemoveListener(() => ChangeLocale(1));
            portugueseLanguageButton.onClick.RemoveListener(() => ChangeLocale(2));

            //CreditsButtons
            backCreditsButton.onClick.RemoveListener(UIMainMenu);

            backExitButton.onClick.RemoveListener(UIMainMenu);
            exitButton.onClick.RemoveListener(UIExit);
        }

        private void Start()
        {
            UIMainMenu();
            ChangeLocale(config.settings.localeID);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        #region UILogic

        public void UIMainMenu()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[0].SetActive(true);
        }

        private void UICharacterSelection()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[1].SetActive(true);
        }

        public void ChangeCharacterLeft(bool isPlayerOne)
        {
            if(isPlayerOne)
            {
                //Logica de Cambiar Sprite y stats de Sliders para el Player 1
            }
            else
            {
                //Logica de Cambiar Sprite y stats de Sliders para el Player 2
            }
        }

        public void ChangeCharacterRight(bool isPlayerOne)
        {
            if(isPlayerOne)
            {
                //Logica de Cambiar Sprite y stats de Sliders para el Player 1
            }
            else
            {
                //Logica de Cambiar Sprite y stats de Sliders para el Player 2
            }
        }

        public void UpdateSliders()
        {
            //Logica de actualizar UI de Sliders con los config de los characters
        }

        private void UILevelSelection()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[2].SetActive(true);
        }

        private void UIControls()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[3].SetActive(true);
        }

        private void UISettings()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[4].SetActive(true);
        }

        private void UICredits()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[5].SetActive(true);
        }

        private void UIExit()
        {
            for (int i = 0; i < panels.Length;i++)
            {
                panels[i].SetActive(false);
            }

            panels[6].SetActive(true);
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

                default: Debug.LogWarning("This level do not exists: " + levelSelected);
                    break;
            }
        }

        public void SelectRandomLevel()
        {
            int randomLevelSelected = Random.Range(1, 5);
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

