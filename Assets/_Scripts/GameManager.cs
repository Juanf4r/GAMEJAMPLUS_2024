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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private InputPlayers _inputPlayers;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textJugador1;
    [SerializeField] private TextMeshProUGUI textJugador2;
    [SerializeField] private TextMeshProUGUI textCronometro;
    [SerializeField] private GameObject panelGanador1;
    [SerializeField] private GameObject panelGanador2;
    [SerializeField] private GameObject timerCenter;

    [Header("Carnes")]
    [SerializeField] private List<Transform> spawnCarne;
    private List<Transform> usedSpawns = new List<Transform>();
    [SerializeField] private GameObject carne;
    [SerializeField] private GameObject meatGold;

    [Header("Contador")]
    public int contadorJugador1 = 0;
    public int contadorJugador2 = 0;
    private float cronometro = 90f;
    public bool timeOver = false;

    [Header("Referencias")]
    [SerializeField] private GameObject refPlayer1;
    [SerializeField] private GameObject refPlayer2;
    
    private PlayerManager _player1;
    private PlayerManager _player2;

    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;

    [FormerlySerializedAs("tpUP")]
    [Header("PowerUps")]
    [SerializeField] private GameObject powerUpPrefab;

    [SerializeField] private PowerUpSo teleportPu, speedPu, strengthPu;
    [SerializeField] private GameObject[] spawnPowerUP;
    private List<GameObject> powerUpInstances = new List<GameObject>();

    [Header("Pausa")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelMusica;
    private bool isPaused = false;

    [Header("Contador de inicio")]
    [SerializeField] private GameObject panelContador;
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
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        timeOver = false;
        textJugador1.text = "0 / 3";
        textJugador2.text = "0 / 3";   
        Iniciar();
        LocalizarCarne();
        panelGanador1.SetActive(false);
        panelGanador2.SetActive(false);
        audioGanar.Stop();
    }

    public void GanarRondaJugador1()
    {

        if (timeOver)
        {
            contadorJugador1 += 3;
        }
        else
        {
            contadorJugador1++;
        }

        textJugador1.text = contadorJugador1.ToString() + " / 3";
        if (contadorJugador1 >= 3)
        {
            GanarJuego();
            SoundFXChannel.PlaySoundFxClip(derrota, _player2.transform.position, .5f, true);

        }
    }

    public void GanarRondaJugador2()
    {

        if (timeOver)
        {
            contadorJugador2 += 3;
        }
        else
        {
            contadorJugador2++;
        }
        //Debug.Log("juagdor 2:" + contadorJugador2);
        textJugador2.text = contadorJugador2.ToString() + " / 3";
        if (contadorJugador2 >= 3) 
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
        LimpiarPowerUp();
        if (contadorJugador1 == contadorJugador2)
        {
            

            timeOver = true;
            textCronometro.text = "";
            _player1.canMove = false;
            _player2.canMove = false;
            _inputPlayers.Disable();
            Iniciar();
            cronometro += 100;
            timerCenter.SetActive(false);
            carne.transform.localPosition = meatGold.transform.position;
            if(cronometro >= 5f)
            {
                StartCoroutine(newCronometro());
            }
        }
        else
        {
            GanarJuego();
        }
    }

    private void GanarJuego()
    {

        LimpiarPowerUp();
        if (contadorJugador1 >= 1)
        {
            panelGanador1.SetActive(true);
            
            _player1.OnWin();
            _player2.OnLose();
            
            audioGanar.Play();
        }
        else if (contadorJugador2 >= 1)
        {
            panelGanador2.SetActive(true);
            
            _player2.OnWin();
            _player1.OnLose();
            
            audioGanar.Play();
        }
        else
        {
            LocalizarCarne();
            Iniciar();
        }
        timerCenter.SetActive(false);
        StartCoroutine(backMenu(5f));
    }

    public void LocalizarCarne()
    {
        if (spawnCarne.Count > 0)
        {
            var randomIndex = Random.Range(0, spawnCarne.Count);
            carne.transform.localPosition = spawnCarne[randomIndex].position;
            usedSpawns.Add(spawnCarne[randomIndex]);
            spawnCarne.RemoveAt(randomIndex);

            if (usedSpawns.Count > 1)
            {
                spawnCarne.Add(usedSpawns[usedSpawns.Count - 2]);
                usedSpawns.RemoveAt(usedSpawns.Count - 2);
            }
        }
    }

    private void Iniciar()
    {
        refPlayer1.transform.localPosition = spawn1.transform.localPosition;
        refPlayer2.transform.localPosition = spawn2.transform.localPosition;

        PowerUp();

        StartCoroutine(CuentaRegresiva());
    }

    private void FixedUpdate()
    {
        cronometro -= Time.deltaTime;
        textCronometro.text = cronometro.ToString("000"); 
        if (cronometro <= 0)
        {
            EndForTime();
        }
        else if (timeOver && cronometro <= 5)
        {
            StartCoroutine(newCronometro());
        }
    }

    private void PowerUp()
    {
        foreach (var spawnPoint in spawnPowerUP)
        {
            var random = Random.Range(0, 3);
            var powerUpInstance = Instantiate(powerUpPrefab, spawnPoint.transform.position, Quaternion.identity);
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

    private void LimpiarPowerUp()
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

    private IEnumerator CuentaRegresiva()
    {
        panelGameplay.SetActive(false);
        panelContador.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            contadorInicio.text = i.ToString(); 
            yield return new WaitForSeconds(1f);
        }

        contadorInicio.text = "GO!!";
        cronometro = 90f;
        
        yield return new WaitForSeconds(.5f);
        
        panelGameplay.SetActive(true);
        panelContador.gameObject.SetActive(false);
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

        var randomX = Random.Range(-1f, 2f); // Adjust range as needed
        var randomZ = Random.Range(-1f, 2f); // Adjust range as needed

        var modifyX = Random.value > 0.5f; // 50% chance to pick x or y
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
    private IEnumerator backMenu(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    private IEnumerator newCronometro()
    {
        yield return new WaitForSeconds(4);
        cronometro += 10000f;
    }
}
