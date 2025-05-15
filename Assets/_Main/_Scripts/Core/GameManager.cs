using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using _ScriptableObjects.Scripts;
using _Scripts;
using _Scripts.Players;
using _Scripts.PowerUps;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Assets.Minimap;
using System;
using UI;
using UnityEngine.Localization.Components;

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
        [SerializeField] private GameObject meatGoldSpawn;
        [SerializeField] private Sprite meat;
        [SerializeField] private Sprite meatGold;
        [SerializeField] private GameObject meatGameObject;

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

        [HideInInspector] public PlayerManager _player1;
        [HideInInspector] public PlayerManager _player2;

        private bool isPaused = false;
        private bool canChangeVelocity = true;

        [Header("Sonidos")] 
        [SerializeField] private AudioClip cryingAudioClip;
        private bool gameFinished = false;


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
            if (InGameUIManager.Instance != null)
            {
                startGameAction += InGameUIManager.Instance.HidePlayersPanels;
            }
        }
        void OnDisable()
        {
            _inputPlayers.Players.Pause.performed -= Pause;
            startGameAction -= InGameUIManager.Instance.HidePlayersPanels;
        }

        private void Start()
        {
            Time.timeScale = 1;
            
            timeOver = false;
            meatsOfPlayer1 = 0;
            meatsOfPlayer2 = 0;
            StartGame();
            LocateMeat();
            canChangeVelocity = true;
            gameFinished = false;

            InGameUIManager.Instance.RestartMeatsTexts();

            startGameAction?.Invoke();
            MusicManager.Instance.PlayInGameMusic();
        }

        private void FixedUpdate()
        {
            _gameSeconds -= Time.deltaTime;
            InGameUIManager.Instance.timeLeftText.text = _gameSeconds.ToString("000"); 
            if (_gameSeconds <= 20f && canChangeVelocity)
            {
                MusicManager.Instance.SetVelocity(1.2f);
                canChangeVelocity = false;
            }
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

            Debug.Log("Entre");

            InGameUIManager.Instance.isPaused = !InGameUIManager.Instance.isPaused;
            InGameUIManager.Instance.PauseGame(InGameUIManager.Instance.isPaused, false);
        }

        #region GameLogic
        
        public void LocateMeat()
        {
            if (spawnMeats.Count > 0)
            {
                Transform farthestSpawn = null;
                float maxDistance = 0f;

                foreach (var spawn in spawnMeats)
                {
                    float distancePlayer1 = Vector3.Distance(spawn.position, _player1.transform.position);
                    float distancePlayer2 = Vector3.Distance(spawn.position, _player2.transform.position);
                    float minDistance = Mathf.Min(distancePlayer1, distancePlayer2);

                    if (minDistance > maxDistance)
                    {
                        maxDistance = minDistance;
                        farthestSpawn = spawn;
                    }
                }

                if (farthestSpawn != null)
                {
                    meatGameObject.transform.position = farthestSpawn.position;
                    MinimapElementData meatIconData = new MinimapElementData()
                    {
                        TargetTransform = meatGameObject.transform,
                        IconSprite = meat,
                        BaseSize = new Vector2(30, 30),
                        ScaleWithMap = false,
                        PreserveAspect = true
                    };
                    MinimapController.instance.AddMinimapElement(meatIconData);
                }

                meatGameObject.SetActive(true);
            }
        }

        private void StartGame()
        {
            refPlayer1.transform.localPosition = spawn1.transform.localPosition;
            refPlayer2.transform.localPosition = spawn2.transform.localPosition;

            PowerUp();
            StartCoroutine(Countdown());
        }

        public void CheckPlayer1Meat()
        {
            if (timeOver)
            {
                meatsOfPlayer1 += 3;
                InGameUIManager.Instance.TakeGoldMeatUI(1);
            }
            else
            {
                meatsOfPlayer1++;
                player1_Animator.SetBool("Eating", true);
                InGameUIManager.Instance.ModUIMeats(1, meatsOfPlayer1);
            }

            if (meatsOfPlayer1 >= 3)
            {   
                CheckPlayerWin();
            }
        }
        public void CheckPlayer2Meat()
        {
            if (timeOver)
            {
                meatsOfPlayer2 += 3;
                InGameUIManager.Instance.TakeGoldMeatUI(2);
            }
            else
            {
                meatsOfPlayer2++;
                player2_Animator.SetBool("Eating", true);
                InGameUIManager.Instance.ModUIMeats(2, meatsOfPlayer2);
            }
            if (meatsOfPlayer2 >= 3) 
            {
                CheckPlayerWin();
            }
        }

        private void CheckPlayerWin()
        {
            if (gameFinished) return;
            gameFinished = true;

            CleanPowerUp();

            if (meatsOfPlayer1 > meatsOfPlayer2)
            {
                InGameUIManager.Instance.panelWinnerPlayer1.SetActive(true);

                _player1.OnWin();
                _player2.OnLose();
                meatGameObject.SetActive(false);
                SoundFXChannel.PlaySoundFxClip(cryingAudioClip, _player2.transform.position, .5f, true);
            }
            else if (meatsOfPlayer2 > meatsOfPlayer1)
            {
                InGameUIManager.Instance.panelWinnerPlayer2.SetActive(true);

                _player2.OnWin();
                _player1.OnLose();
                meatGameObject.SetActive(false);
                SoundFXChannel.PlaySoundFxClip(cryingAudioClip, _player1.transform.position, .5f, true);
            }

            InGameUIManager.Instance.containerTimeLeft.SetActive(false);
            MusicManager.Instance.PlayInGameMusicGameOver();
            StartCoroutine(FinishGame(5f));
            _player1.canMove = false;
            _player2.canMove = false;
            _inputPlayers.Disable();
        }

        private void EndForTime()
        {
            //Reset PowerUps Players
            var player1Actions = _player1.GetComponent<PlayerActions>();
            var player2Actions = _player2.GetComponent<PlayerActions>();
            player1Actions?.ResetPowerUpsForBothPlayers();
            player2Actions?.ResetPowerUpsForBothPlayers();

            CleanPowerUp();

            //Extra round
            if (meatsOfPlayer1 == meatsOfPlayer2)
            {   
                MeatGoldRound();
            }
            else
            {
                CheckPlayerWin();
            }
        }

        public void MeatGoldRound()
        {
            //Disable mov players
            _player1.canMove = false;
            _player2.canMove = false;
            _inputPlayers.Disable();

            //Change music
            MusicManager.Instance.PlayInGameMusicExtraRound();
            MusicManager.Instance.SetVelocity(1f);  

            //Start ExtraRound
            timeOver = true;
            InGameUIManager.Instance.timeLeftText.text = "";
                
            refPlayer1.transform.localPosition = spawn1.transform.localPosition;
            refPlayer2.transform.localPosition = spawn2.transform.localPosition;
            PowerUp();
            StartCoroutine(CountdownExtraRound());
            _gameSeconds += 100;

            InGameUIManager.Instance.containerTimeLeft.SetActive(false);
                
            //Change Meat to Gold
            if (meatGameObject != null)
            {
                var spriteRenderer = meatGameObject.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = meatGold;
                    Debug.Log("Sprite del Meat cambiado a Gold.");
                }
                else
                {
                    Debug.LogError("SpriteRenderer no encontrado en los hijos de meatGameObject.");
                }
            }
            else
            {
                Debug.LogError("meatGameObject no está asignado.");
            }
            MeatController.Instance.SetAnimatorController(true);
            meatGameObject.transform.localPosition = meatGoldSpawn.transform.position;

            //Change UI to Gold meat
            InGameUIManager.Instance.StartGoldMeatUI();
            InGameUIManager.Instance.extraRoundPanel.SetActive(true);

            // Create proper minimap element data
            MinimapElementData goldMeatIcon = new MinimapElementData()
            {
                TargetTransform = meatGameObject.transform,
                IconSprite = meat, // Add this reference to your class
                BaseSize = new Vector2(40, 40), // Larger size for gold meat
                ScaleWithMap = false,
                PreserveAspect = true,
                // Add any special visual effects if needed
            };
            MinimapController.instance.AddMinimapElement(goldMeatIcon);

            if (_gameSeconds >= 5f)
            {
                StartCoroutine(AddGameSeconds());
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
        
                // Create minimap element data for the power-up

        
                // Add to minimap
        
                powerUpInstance.SetActive(false);
        
                powerUpInstance.GetComponent<PowerUp>().powerUpType = random switch
                {
                    0 => teleportPu,
                    1 => speedPu,
                    2 => strengthPu,
                    _ => powerUpInstance.GetComponent<PowerUp>().powerUpType
                };
                
                MinimapElementData powerUpIcon = new MinimapElementData()
                {
                    TargetTransform = powerUpInstance.transform,
                    IconSprite = powerUpInstance.GetComponent<PowerUp>().powerUpType.buffSprite,
                    BaseSize = new Vector2(15, 15), // Smaller size for power-ups
                    ScaleWithMap = false,
                    PreserveAspect = true
                };
                MinimapController.instance.AddMinimapElement(powerUpIcon);
                Debug.Log($"PowerUp {powerUpInstance.name}, icon: {powerUpIcon.IconSprite}");
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
            string iDText = "G035";
            LocalizeStringEvent changeLocalizationText = InGameUIManager.Instance.countdownText.GetComponent<LocalizeStringEvent>();
            if (changeLocalizationText != null)
            {
                changeLocalizationText.StringReference.TableEntryReference = iDText;
                InGameUIManager.Instance.countdownText.text = changeLocalizationText.StringReference.GetLocalizedString();
            }
            else
            {
                Debug.LogError("No se encontró el componente LocalizeStringEvent en countdownText.");
            }
            _gameSeconds = 91f;
            
            yield return new WaitForSeconds(.5f);
            
            InGameUIManager.Instance.panels[0].SetActive(true);
            InGameUIManager.Instance.panelCountdown.gameObject.SetActive(false);

            _inputPlayers.Enable();
            _player1.canMove = true;
            _player2.canMove = true;
        }
        private IEnumerator CountdownExtraRound()
        {
            InGameUIManager.Instance.panels[0].SetActive(false);
            InGameUIManager.Instance.panelCountdown.gameObject.SetActive(true);
            Color goldColor;
            if (ColorUtility.TryParseHtmlString("#FF9900", out goldColor))
            {
                InGameUIManager.Instance.countdownText.color = goldColor;
            }
            for (int i = 3; i > 0; i--)
            {
                InGameUIManager.Instance.countdownText.text = i.ToString(); 
                yield return new WaitForSeconds(1f);
            }
            string iDText = "G035";
            LocalizeStringEvent changeLocalizationText = InGameUIManager.Instance.countdownText.GetComponent<LocalizeStringEvent>();
            if (changeLocalizationText != null)
            {
                changeLocalizationText.StringReference.TableEntryReference = iDText;
                InGameUIManager.Instance.countdownText.text = changeLocalizationText.StringReference.GetLocalizedString();
            }
            else
            {
                Debug.LogError("No se encontró el componente LocalizeStringEvent en countdownText.");
            }
            //InGameUIManager.Instance.countdownText.text = "GO!! \n Meat Gold timeee!!";
            _gameSeconds = 91f;
            
            yield return new WaitForSeconds(1f);
            
            InGameUIManager.Instance.panels[0].SetActive(true);
            InGameUIManager.Instance.panelCountdown.gameObject.SetActive(false);
            InGameUIManager.Instance.extraRoundPanel.SetActive(false);

            _inputPlayers.Enable();
            _player1.canMove = true;
            _player2.canMove = true;
        }

        private IEnumerator FinishGame(float time)
        {
            yield return new WaitForSeconds(time);
            InGameUIManager.Instance.finishGamePanel.SetActive(true);
        }

        private IEnumerator AddGameSeconds()
        {
            yield return new WaitForSeconds(4);
            _gameSeconds += 10000f;
        }
        
        #endregion
    }
}