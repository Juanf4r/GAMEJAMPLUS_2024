using _Scripts.Players;
using _ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

namespace UI
{
    public class InGameUIManager : MonoBehaviour
    {
        public static InGameUIManager Instance { get; private set;}

        [SerializeField] private List<Image> powerUpListImages = new();

        [Header("Texts")]
        public TextMeshProUGUI player1MeatsText;
        public TextMeshProUGUI player2MeatsText;
        public TextMeshProUGUI timeLeftText;
        public TextMeshProUGUI countdownText;

        [Header("Gameplay Panels")]
        public GameObject panelCountdown;
        public GameObject panelWinnerPlayer1;
        public GameObject panelWinnerPlayer2;
        public GameObject containerTimeLeft;

        [Header("Pause Panels")]
        public GameObject[] panels = new GameObject[3];
        //0 gameplayPanel
        //1 pausePanel
        //2 settingsPanel

        private bool _active = false;

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
        }

        private void OnEnable()
        {
            PlayerManager.OnPowerUpUpdated += UpdateImage;
            PlayerActions.OnPowerUpOut += DisablePowerUp;
        }

        private void OnDisable()
        {
            PlayerManager.OnPowerUpUpdated -= UpdateImage;
            PlayerActions.OnPowerUpOut -= DisablePowerUp;
        }

        private void Start()
        {
            int ID = PlayerPrefs.GetInt("LocaleKey",0);

            foreach (var image in powerUpListImages)
            {
                image.gameObject.SetActive(false);
            }
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
            player1MeatsText.text = "0 / 3";
            player2MeatsText.text = "0 / 3";  
        }

        #region PauseLogic

        public void PauseGame(bool pauseState)
        {
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

        public void HidePause()
        {
            for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }

                panels[0].SetActive(true);
        }

        public void ShowPause()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            panels[1].SetActive(true);
        }

        public void ShowSettings()
        {   
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            panels[2].SetActive(true);
        }

        public void ExitGame()
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
            PlayerPrefs.SetInt("LocaleKey",localeID);
            _active = false;
        }

        #endregion
    }
}
