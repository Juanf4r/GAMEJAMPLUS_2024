using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using _ScriptableObjects.Scripts;
using _Scripts;
using _Scripts.Players;
using _Scripts.PowerUps;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;
using Assets.Minimap;
using System;
using UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}

        public Action startGameAction;
        public Action gameOverAction;

        private InputPlayers _inputPlayers;

        [Header("Meats")]
        [SerializeField] private List<Transform> spawnMeats;
        private List<Transform> usedSpawns = new List<Transform>();
        [SerializeField] private GameObject meatGameObject;
        [SerializeField] private Sprite meat;
        [SerializeField] private GameObject meatGold;

        [Header("Meats and Timer")]
        [HideInInspector] public int meatsOfPlayer1 = 0;
        [HideInInspector] public int meatsOfPlayer2 = 0;
        [HideInInspector] public bool timeOver = false;
        private float _gameSeconds = 91f;

        [Header("References")]
        [SerializeField] private GameObject refPlayer1;
        [SerializeField] private GameObject refPlayer2;

        [SerializeField] private Animator player1_Animator;
        [SerializeField] private Animator player2_Animator;
        
        [SerializeField] private GameObject spawn1;
        [SerializeField] private GameObject spawn2;

        [FormerlySerializedAs("tpUP")]
        [Header("PowerUps")]
        [SerializeField] private GameObject powerUpPrefab;
        [SerializeField] private PowerUpSo teleportPu, speedPu, strengthPu;
        [SerializeField] private GameObject[] spawnPowerUP;

        private List<GameObject> powerUpInstances = new List<GameObject>();

        private PlayerManager _player1;
        private PlayerManager _player2;

        private bool isPaused = false;

        [Header("Musica ganar")]
        [SerializeField] private AudioSource winAudioSource;

        [Header("Sonidos")] 
        [SerializeField] private AudioClip cryingAudioClip;

        private void Awake()
        {
            //Enable Inputs for Both Players
            _inputPlayers = new InputPlayers();
            _inputPlayers.Players.Pause.Enable();
            _inputPlayers.Players.Pause.performed += Pause;

            var players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                if (player.isPlayerOne)
                {
                    _player1 = player;
                }
                else
                {
                    _player2 = player;
                }
            }        
            
            //Singleton
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
            startGameAction += InGameUIManager.Instance.HidePlayersPanels;
        }

        void OnDisable()
        {
            startGameAction -= InGameUIManager.Instance.HidePlayersPanels;
        }

        private void Start()
        {
            Time.timeScale = 1;
            
            timeOver = false;

            StartGame();
            LocateMeat();

            InGameUIManager.Instance.RestartMeatsTexts();

            startGameAction?.Invoke();
            winAudioSource.Stop();
        }

        private void FixedUpdate()
        {
            _gameSeconds -= Time.deltaTime;
            InGameUIManager.Instance.timeLeftText.text = _gameSeconds.ToString("000"); 
            if (_gameSeconds <= 0)
            {
                EndForTime();
            }
            else if (timeOver && _gameSeconds <= 5)
            {
                StartCoroutine(AddGameSeconds());
            }
        }

        private void Pause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            isPaused = !isPaused;

            InGameUIManager.Instance.PauseGame(isPaused);
        }

        #region GameLogic
        
        public void LocateMeat()
        {
            if (spawnMeats.Count > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, spawnMeats.Count);
                meatGameObject.transform.localPosition = spawnMeats[randomIndex].position;
                MinimapController.instance.AddMinimapElement(meat, meatGameObject.transform);
                usedSpawns.Add(spawnMeats[randomIndex]);
                spawnMeats.RemoveAt(randomIndex);

                if (usedSpawns.Count > 1)
                {
                    spawnMeats.Add(usedSpawns[usedSpawns.Count - 2]);
                    usedSpawns.RemoveAt(usedSpawns.Count - 2);
                }
            }
        }

        private void StartGame()
        {
            refPlayer1.transform.localPosition = spawn1.transform.localPosition;
            refPlayer2.transform.localPosition = spawn2.transform.localPosition;

            PowerUp();
            StartCoroutine(Countdown());
        }

        private void WinGame()
        {
            CleanPowerUp();

            if (meatsOfPlayer1 >= 1)
            {
                InGameUIManager.Instance.panelWinnerPlayer1.SetActive(true);
                
                _player1.OnWin();
                _player2.OnLose();
                
                winAudioSource.Play();
            }
            else if (meatsOfPlayer2 >= 1)
            {
                InGameUIManager.Instance.panelWinnerPlayer2.SetActive(true);
                
                _player2.OnWin();
                _player1.OnLose();
                
                winAudioSource.Play();
            }
            else
            {
                LocateMeat();
                StartGame();
            }

            InGameUIManager.Instance.containerTimeLeft.SetActive(false);
            StartCoroutine(BackToMenu(5f));
        }

        private void EndForTime()
        {
            var player1Actions = _player1.GetComponent<PlayerActions>();
            var player2Actions = _player2.GetComponent<PlayerActions>();
            player1Actions?.ResetPowerUpsForBothPlayers();
            player2Actions?.ResetPowerUpsForBothPlayers();

            CleanPowerUp();

            if (meatsOfPlayer1 == meatsOfPlayer2)
            {
                _player1.canMove = false;
                _player2.canMove = false;
                _inputPlayers.Disable();

                timeOver = true;

                InGameUIManager.Instance.timeLeftText.text = "";
                
                StartGame();
                _gameSeconds += 100;

                InGameUIManager.Instance.containerTimeLeft.SetActive(false);

                meatGameObject.transform.localPosition = meatGold.transform.position;
                MinimapController.instance.AddMinimapElement(meat, meatGameObject.transform);

                if (_gameSeconds >= 5f)
                {
                    StartCoroutine(AddGameSeconds());
                }
            }
            else
            {
                WinGame();
            }
        }

        public void Player1Win()
        {
            if (timeOver)
            {
                meatsOfPlayer1 += 3;
            }
            else
            {
                meatsOfPlayer1++;
                player1_Animator.SetBool("Eating", true);
            }

            InGameUIManager.Instance.player1MeatsText.text = meatsOfPlayer1.ToString() + " / 3";

            if (meatsOfPlayer1 >= 3)
            {
                WinGame();
                SoundFXChannel.PlaySoundFxClip(cryingAudioClip, _player2.transform.position, .5f, true);
            }
        }

        public void Player2Win()
        {
            if (timeOver)
            {
                meatsOfPlayer2 += 3;
            }
            else
            {
                meatsOfPlayer2++;
                player2_Animator.SetBool("Eating", true);
            }
        
            InGameUIManager.Instance.player2MeatsText.text = meatsOfPlayer2.ToString() + " / 3";

            if (meatsOfPlayer2 >= 3) 
            {
                WinGame();
                SoundFXChannel.PlaySoundFxClip(cryingAudioClip, _player1.transform.position, .5f,true);
            }
        }

        #endregion

        #region PowerUps

        private void PowerUp()
        {
            foreach (var spawnPoint in spawnPowerUP)
            {
                var random = UnityEngine.Random.Range(0, 3);
                var powerUpInstance = Instantiate(powerUpPrefab, spawnPoint.transform.position, Quaternion.identity);
                MinimapController.instance.AddMinimapElement(powerUpInstance.GetComponent<SpriteRenderer>().sprite, powerUpInstance.transform);
                powerUpInstance.SetActive(false);
                
                powerUpInstance.GetComponent<PowerUp>().powerUpType = random switch
                {
                    0 => teleportPu,
                    1 => speedPu,
                    2 => strengthPu,
                    _ => powerUpInstance.GetComponent<PowerUp>().powerUpType
                };
                powerUpInstance.SetActive(true);
                powerUpInstances.Add(powerUpInstance);
                
            }
        }

        private void CleanPowerUp()
        {
            foreach (var powerUp in powerUpInstances)
            {
                if (powerUp)
                {
                    Destroy(powerUp);
                }
            }
            powerUpInstances.Clear();
        }

        public Vector3 GetTeleportLocation(int playerIndex)
        {
            var position = playerIndex switch
            {
                1 => _player1.transform.position,
                2 => _player2.transform.position,
                _ => Vector3.zero
            };

            var randomX = UnityEngine.Random.Range(-1f, 2f); // Adjust range as needed
            var randomZ = UnityEngine.Random.Range(-1f, 2f); // Adjust range as needed

            var modifyX = UnityEngine.Random.value > 0.5f; // 50% chance to pick x or y
            if (modifyX)
            {
                position.x += randomX;
            }
            else
            {
                position.z += randomZ;
            }

            return position;
        }

        #endregion

        #region IEnumerators

        private IEnumerator Countdown()
        {
            InGameUIManager.Instance.panels[0].SetActive(false);
            InGameUIManager.Instance.panelCountdown.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                
                InGameUIManager.Instance.countdownText.text = i.ToString(); 
                yield return new WaitForSeconds(1f);
            }

            InGameUIManager.Instance.countdownText.text = "GO!!";
            _gameSeconds = 91f;
            
            yield return new WaitForSeconds(.5f);
            
            InGameUIManager.Instance.panels[0].SetActive(true);
            InGameUIManager.Instance.panelCountdown.gameObject.SetActive(false);

            _inputPlayers.Enable();
            _player1.canMove = true;
            _player2.canMove = true;
        }

        private IEnumerator BackToMenu(float time)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene(0);
        }

        private IEnumerator AddGameSeconds()
        {
            yield return new WaitForSeconds(4);
            _gameSeconds += 10000f;
        }
        
        #endregion
    }
}
