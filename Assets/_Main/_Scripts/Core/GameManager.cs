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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Action startGameAction;
    public Action gameOverAction;

    private InputPlayers _inputPlayers;

    [Header("Carnes")]
    [SerializeField] private List<Transform> spawnCarne;
    private List<Transform> usedSpawns = new List<Transform>();
    [SerializeField] private GameObject carne;
    [SerializeField] private Sprite meat;
    [SerializeField] private GameObject meatGold;

    [Header("Meats and Timer")]
    public int meatsOfPlayer1 = 0;
    public int meatsOfPlayer2 = 0;
    private float gameSeconds = 91f;
    public bool timeOver = false;

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

    [Header("Pause")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelMusica;

    private PlayerManager _player1;
    private PlayerManager _player2;

    private bool isPaused = false;

    [Header("Start Timer")]
    [SerializeField] private TextMeshProUGUI contadorInicio;

    [Header("Musica ganar")]
    [SerializeField] private AudioSource audioGanar;

    [Header("Sonidos")] 
    [SerializeField] private AudioClip derrota;

    private void Awake()
    {
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

        InGameUIManager.Instance.textJugador1.text = "0 / 3";
        InGameUIManager.Instance.textJugador2.text = "0 / 3"; 

        startGameAction?.Invoke();
        audioGanar.Stop();
    }

    private void FixedUpdate()
    {
        gameSeconds -= Time.deltaTime;
        InGameUIManager.Instance.textCronometro.text = gameSeconds.ToString("000"); 
        if (gameSeconds <= 0)
        {
            EndForTime();
        }
        else if (timeOver && gameSeconds <= 5)
        {
            StartCoroutine(AddGameSeconds());
        }
    }

    public void GanarRondaJugador1()
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

        InGameUIManager.Instance.textJugador1.text = meatsOfPlayer1.ToString() + " / 3";
        if (meatsOfPlayer1 >= 3)
        {
            GanarJuego();
            SoundFXChannel.PlaySoundFxClip(derrota, _player2.transform.position, .5f, true);

        }
    }

    public void GanarRondaJugador2()
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
    
        InGameUIManager.Instance.textJugador2.text = meatsOfPlayer2.ToString() + " / 3";

        if (meatsOfPlayer2 >= 3) 
        {
            GanarJuego();
            SoundFXChannel.PlaySoundFxClip(derrota, _player1.transform.position, .5f,true);
        }
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

            InGameUIManager.Instance.textCronometro.text = "";
            
            StartGame();
            gameSeconds += 100;
            InGameUIManager.Instance.timerCenter.SetActive(false);
            carne.transform.localPosition = meatGold.transform.position;
            MinimapController.instance.AddMinimapElement(meat, carne.transform);
            if (gameSeconds >= 5f)
            {
                StartCoroutine(AddGameSeconds());
            }
        }
        else
        {
            GanarJuego();
        }
    }

    private void GanarJuego()
    {

        CleanPowerUp();
        if (meatsOfPlayer1 >= 1)
        {
            InGameUIManager.Instance.panelGanador1.SetActive(true);
            
            _player1.OnWin();
            _player2.OnLose();
            
            audioGanar.Play();
        }
        else if (meatsOfPlayer2 >= 1)
        {
            InGameUIManager.Instance.panelGanador2.SetActive(true);
            
            _player2.OnWin();
            _player1.OnLose();
            
            audioGanar.Play();
        }
        else
        {
            LocateMeat();
            StartGame();
        }
        InGameUIManager.Instance.timerCenter.SetActive(false);
        StartCoroutine(BackToMenu(5f));
    }

    public void LocateMeat()
    {
        if (spawnCarne.Count > 0)
        {
            var randomIndex = UnityEngine.Random.Range(0, spawnCarne.Count);
            carne.transform.localPosition = spawnCarne[randomIndex].position;
            MinimapController.instance.AddMinimapElement(meat, carne.transform);
            usedSpawns.Add(spawnCarne[randomIndex]);
            spawnCarne.RemoveAt(randomIndex);

            if (usedSpawns.Count > 1)
            {
                spawnCarne.Add(usedSpawns[usedSpawns.Count - 2]);
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

    private void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            panelPausa.SetActive(true);
            panelGameplay.SetActive(false);
            panelMusica.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            panelPausa.SetActive(false);
            panelGameplay.SetActive(true);
            panelMusica.SetActive(false);
        }
    }

    private IEnumerator Countdown()
    {
        panelGameplay.SetActive(false);
        InGameUIManager.Instance.panelContador.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            contadorInicio.text = i.ToString(); 
            yield return new WaitForSeconds(1f);
        }

        contadorInicio.text = "GO!!";
        gameSeconds = 91f;
        
        yield return new WaitForSeconds(.5f);
        
        panelGameplay.SetActive(true);
        InGameUIManager.Instance.panelContador.gameObject.SetActive(false);
        _inputPlayers.Enable();
        _player1.canMove = true;
        _player2.canMove = true;
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
    private IEnumerator BackToMenu(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    private IEnumerator AddGameSeconds()
    {
        yield return new WaitForSeconds(4);
        gameSeconds += 10000f;
    }
}
