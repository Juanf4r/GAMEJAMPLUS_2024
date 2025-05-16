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

namespace UI
{
    public class InGameUIManager : MonoBehaviour
    {
        public static InGameUIManager Instance { get; private set; }

        [SerializeField] private Animator gameplayCanvasAnimator;

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
        public GameObject extraRoundPanel;

        public GameObject finishGamePanel;

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
        [SerializeField] private Sprite goldenMeatVoid;
        [SerializeField] private Sprite goldenMeat;
        [SerializeField] private List<Sprite> meatsPlayers = new List<Sprite>();

        [Header("Buttons FinishGame")]
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button backLobbyButton;

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
            extraRoundPanel.SetActive(false);

            gameplayCanvasAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            finishGamePanel.SetActive(false);
        }

        private void Update()
        {   
            if (Input.GetKeyDown(KeyCode.F11))
            {
                ToogleScreenMode();
            }
        }

        public void AddListenerOnButtons()
        {
            //pauseButtons   
            pauseButton.onClick.AddListener(() => PauseGame(isPaused, true));
            controlsButton.onClick.AddListener(ShowControls);
            settingsButton.onClick.AddListener(ShowSettings);
            exitButton.onClick.AddListener(ExitGame);

            //controlsButtons
            backControlsButton.onClick.AddListener(() => ShowPause(true));

            //SettingsButtons
            backSettingsButton.onClick.AddListener(() => ShowPause(true));
            englishLanguageButton.onClick.AddListener(() => ChangeLocale(0));
            spanishLanguageButton.onClick.AddListener(() => ChangeLocale(1));
            portugueseLanguageButton.onClick.AddListener(() => ChangeLocale(2));
            sfxSlider.onValueChanged.AddListener(UpdateSFX);
            musicSlider.onValueChanged.AddListener(UpdateMusic);

            //Finish Game
            playAgainButton.onClick.AddListener(PlayAgainMap);
            backLobbyButton.onClick.AddListener(BackLobby);
        }

        public void RemoveListenerOnButtons()
        {
            //pauseButtons   
            pauseButton.onClick.RemoveListener(() => PauseGame(isPaused, true));
            controlsButton.onClick.RemoveListener(ShowControls);
            settingsButton.onClick.RemoveListener(ShowSettings);
            exitButton.onClick.RemoveListener(ExitGame);

            //controlsButtons
            backControlsButton.onClick.AddListener(() => ShowPause(true));

            //SettingsButton
            backSettingsButton.onClick.AddListener(() => ShowPause(true));
            englishLanguageButton.onClick.RemoveListener(() => ChangeLocale(0));
            spanishLanguageButton.onClick.RemoveListener(() => ChangeLocale(1));
            portugueseLanguageButton.onClick.RemoveListener(() => ChangeLocale(2));
            sfxSlider.onValueChanged.RemoveListener(UpdateSFX);
            musicSlider.onValueChanged.RemoveListener(UpdateMusic);

            //Finish Game
            playAgainButton.onClick.RemoveListener(PlayAgainMap);
            backLobbyButton.onClick.RemoveListener(BackLobby);
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

        public void StartGoldMeatUI()
        {
            CountImageMeattP1.sprite = goldenMeatVoid;
            CountImageMeattP2.sprite = goldenMeatVoid;
        }

        public void TakeGoldMeatUI(int player)
        {
            if (player == 1)
            {
                CountImageMeattP1.sprite = goldenMeat;
            }
            else if (player == 2)
            {
                CountImageMeattP2.sprite = goldenMeat;
            }
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

        private void PlayAgainMap()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        public void BackLobby()
        {
            SceneManager.LoadScene(0);
        }
        #region PauseLogic

        public void PauseGame(bool pauseState, bool UIPressed)
        {
            if (UIPressed)
            {
                pauseState = false;
                isPaused = pauseState;
                gameplayCanvasAnimator.SetTrigger("Gameplay");
            }

            if (pauseState)
            {
                Time.timeScale = 0;

                ShowPause(false);
            }
            else
            {
                Time.timeScale = 1;

                HidePause();
            }
        }

        private void HidePause()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            panels[0].SetActive(true);
        }

        private void ShowPause(bool isBackButton)
        {
            if (isBackButton)
            {
                gameplayCanvasAnimator.SetTrigger("Pause");
            }

            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(true);
            }
        }

        private void ShowControls()
        {
            gameplayCanvasAnimator.SetTrigger("Controls");
        }

        private void ShowSettings()
        {
            gameplayCanvasAnimator.SetTrigger("Settings");
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
            if (_active)
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
        
        public void ToogleScreenMode()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
