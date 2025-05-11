using _Scripts.Players;
using _ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Settings;
using TMPro;
using UnityEngine.Serialization;

namespace UI
{
    public class InGameUIManager : MonoBehaviour
    {
        public static InGameUIManager Instance { get; private set;}

        [SerializeField] private List<Image> powerUpListImages = new();

        [Space]

        [Header("Texts")]
        public TextMeshProUGUI timeLeftText;
        public TextMeshProUGUI countdownText;

        [Space]

        [Header("Gameplay Panels")]
        public GameObject panelCountdown;
        public GameObject panelWinnerPlayer1;
        public GameObject panelWinnerPlayer2;
        public GameObject containerTimeLeft;

        [Space]

        [Header("Pause Panels")]
        public GameObject[] panels = new GameObject[4];
        //0 gameplayPanel
        //1 pausePanel
        //2 controlsPanel
        //3 settingsPanel
        [Header("PauseButtons")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [Space]

        [Header("ControlsButtons")]
        [SerializeField] private Button backControlsButton;

        [Space]

        [Header("SettingsButtons")]
        [SerializeField] private Button backSettingsButton;
        [SerializeField] private Button englishLanguageButton;
        [SerializeField] private Button spanishLanguageButton;
        [SerializeField] private Button portugueseLanguageButton;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;
        
        
        [Space]

        [Header("UI Meats")]
        [SerializeField] private Image CountImageMeattP1;
        [SerializeField] private Image CountImageMeattP2;
        [SerializeField] private Sprite goldenMeat;
        [SerializeField] private List<Sprite> meatsPlayers = new List<Sprite>();

        private bool _active = false;
        [HideInInspector] public bool isPaused = false;
        public Config config;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            config = ConfigManager.LoadConfig();

        }

        private void OnEnable()
        {
            AddListenerOnButtons();

            PlayerManager.OnPowerUpUpdated += UpdateImage;
            PlayerActions.OnPowerUpOut += DisablePowerUp;
        }

        private void OnDisable()
        {         
            RemoveListenerOnButtons();

            PlayerManager.OnPowerUpUpdated -= UpdateImage;
            PlayerActions.OnPowerUpOut -= DisablePowerUp;
        }

        private void Start()
        {
            foreach (var image in powerUpListImages)
            {
                image.gameObject.SetActive(false);
            }
            sfxSlider.value = config.settings.sfxvolume;
            musicSlider.value = config.settings.musicvolume;
        }

        public void AddListenerOnButtons()
        {
            //pauseButtons   
            pauseButton.onClick.AddListener(ContinueGame);
            controlsButton.onClick.AddListener(ShowControls);
            settingsButton.onClick.AddListener(ShowSettings);
            exitButton.onClick.AddListener(ExitGame);

            //controlsButtons
            backControlsButton.onClick.AddListener(ShowPause);

            //SettingsButtons
            backSettingsButton.onClick.AddListener(ShowPause);
            spanishLanguageButton.onClick.AddListener(() => ChangeLocale(0));
            englishLanguageButton.onClick.AddListener(() => ChangeLocale(1));
            portugueseLanguageButton.onClick.AddListener(() => ChangeLocale(2));
            sfxSlider.onValueChanged.AddListener(UpdateSFX);
            musicSlider.onValueChanged.AddListener( UpdateMusic);

        }

        public void RemoveListenerOnButtons()
        {
            //pauseButtons   
            pauseButton.onClick.RemoveListener(ContinueGame);
            controlsButton.onClick.RemoveListener(ShowControls);
            settingsButton.onClick.RemoveListener(ShowSettings);
            exitButton.onClick.RemoveListener(ExitGame);

            //controlsButtons
            backControlsButton.onClick.RemoveListener(ShowPause);

            //SettingsButton
            backSettingsButton.onClick.RemoveListener(ShowPause);
            spanishLanguageButton.onClick.RemoveListener(() => ChangeLocale(0));
            englishLanguageButton.onClick.RemoveListener(() => ChangeLocale(1));
            portugueseLanguageButton.onClick.RemoveListener(() => ChangeLocale(2));
            sfxSlider.onValueChanged.RemoveListener(UpdateSFX);
            musicSlider.onValueChanged.RemoveListener(UpdateMusic);
        }

        public void HidePlayersPanels()
        {
            if (panelWinnerPlayer1 != null && panelWinnerPlayer2 != null)
            {
                panelWinnerPlayer1.SetActive(false);
                panelWinnerPlayer2.SetActive(false);
            }
        }

        public void RestartMeatsTexts()
        {
            CountImageMeattP1.sprite = meatsPlayers[0];
            CountImageMeattP2.sprite = meatsPlayers[0];
        }

        public void PlayGoldMeatUI(){
            CountImageMeattP1.sprite = goldenMeat;
            CountImageMeattP2.sprite = goldenMeat;
        }

        public void ModUIMeats(int player, int count)
        {
            if (count < 0 || count >= meatsPlayers.Count) return;
            Image targetImage = player == 1 ? CountImageMeattP1 : CountImageMeattP2;
            targetImage.sprite = meatsPlayers[count];
        }

        private void UpdateSFX(float value)
        {
            config.settings.sfxvolume = value;
            ConfigManager.SaveConfig(config);
        }

        private void UpdateMusic(float value)
        {
            config.settings.musicvolume = value;
            MusicManager.UpdateMusicVolume();
            ConfigManager.SaveConfig(config);
        }
        
        #region PauseLogic

        public void PauseGame(bool pauseState)
        {
            pauseState = !pauseState;

            if (pauseState)
            {
                Time.timeScale = 0;

                ShowPause();
            }
            else
            {
                Time.timeScale = 1;

                HidePause();
            }
        }

        private void ContinueGame()
        {
            isPaused = !isPaused;

            Time.timeScale = 1;

            HidePause();
        }

        private void HidePause()
        {
            for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }

                panels[0].SetActive(true);
        }

        private void ShowPause()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            panels[1].SetActive(true);
        }

        private void ShowControls()
        {   
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            panels[2].SetActive(true);
        }

        private void ShowSettings()
        {   
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            panels[3].SetActive(true);
        }

        private void ExitGame()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region PowerUps

        private void UpdateImage(PowerUpSo powerUpData, int player)
        {
            var image = powerUpListImages[player - 1];
            image.sprite = powerUpData.buffSprite;
            image.gameObject.SetActive(true);
        }

        public void DisablePowerUp(int player)
        {
            powerUpListImages[player - 1].gameObject.SetActive(false);
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
