using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private InputPlayers _inputPlayers;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textJugador1;
    [SerializeField] private TextMeshProUGUI textJugador2;
    [SerializeField] private TextMeshProUGUI textCronometro;
    [SerializeField] private TextMeshProUGUI textGanador;
    [SerializeField] private GameObject panelGanador;

    [Header("Carnes")]
    [SerializeField] private GameObject[] spawnCarne;
    [SerializeField] private GameObject carne;

    [Header("Contador")]
    private int contadorJugador1 = 0;
    private int contadorJugador2 = 0;
    private float cronometro;

    [Header("Referencias")]
    [SerializeField] private GameObject refPlayer1;
    [SerializeField] private GameObject refPlayer2;
    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;

    [Header("PowerUps")]
    [SerializeField] private GameObject tpUP;
    [SerializeField] private GameObject velUP;
    [SerializeField] private GameObject attUP;
    [SerializeField] private GameObject[] spawnPowerUP;
    private List<GameObject> powerUpInstances = new List<GameObject>();

    [Header("Pausa")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelMusica;
    private bool isPaused = false;

    private void Awake()
    {
        _inputPlayers = new InputPlayers();
        _inputPlayers.Players.Pause.Enable();
        _inputPlayers.Players.Pause.performed += Pause;
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
        Iniciar();
        LocalizarCarne();
        textGanador.gameObject.SetActive(false);
        panelGanador.SetActive(false);
    }

    public void GanarRondaJugador1()
    {
        contadorJugador1 += 1;
        textJugador1.text = contadorJugador1.ToString();
        GanarJuego();
    }

    public void GanarRondaJugador2()
    {
        contadorJugador2 += 1;
        textJugador2.text = contadorJugador2.ToString();
        GanarJuego();
    }

    private void EndForTime()
    {
        GanarJuego();
    }

    private void GanarJuego()
    {

        LimpiarPowerUP();
        if (contadorJugador1 >= 3)
        {
            textJugador1.gameObject.SetActive(false);
            textJugador2.gameObject.SetActive(false);
            textGanador.gameObject.SetActive(true);
            panelGanador.SetActive(true);
            
            textGanador.text = "Gano el jugador 1";
            Time.timeScale = 0;
        }
        else if (contadorJugador2 >= 3)
        {
            textJugador1.gameObject.SetActive(false);
            textJugador2.gameObject.SetActive(false);
            textGanador.gameObject.SetActive(true);
            textGanador.text = "Gano el jugador 2";
            Time.timeScale = 0;
        }
        else
        {
            LocalizarCarne();
            Iniciar();

        }
    }

    private void LocalizarCarne()
    {
        int randomIndex = Random.Range(0, spawnCarne.Length);
        carne.transform.localPosition = spawnCarne[randomIndex].transform.localPosition;
    }

    private void Iniciar()
    {
        refPlayer1.transform.localPosition = spawn1.transform.localPosition;
        refPlayer2.transform.localPosition = spawn2.transform.localPosition;
        cronometro = 0f;
        PowerUP();
        Player1.Instance.restart();
        Player2.Instance.restart();
    }

    private void FixedUpdate()
    {
        cronometro += Time.deltaTime;
        textCronometro.text = cronometro.ToString("F2"); 
        if (cronometro >= 30)
        {
            EndForTime();
        }
    }

    private void PowerUP()
    {
        foreach (GameObject spawnPoint in spawnPowerUP)
        {
            int random = Random.Range(0, 3);

            GameObject powerUpToSpawn = null;

            switch (random)
            {
                case 0:
                    powerUpToSpawn = tpUP;
                    break;
                case 1:
                    powerUpToSpawn = velUP;
                    break;
                case 2:
                    powerUpToSpawn = attUP;
                    break;
            }

            if (powerUpToSpawn != null)
            {
                GameObject powerUpInstance = Instantiate(powerUpToSpawn, spawnPoint.transform.position, Quaternion.identity);
                powerUpInstances.Add(powerUpInstance); 
            }
        }
    }

    private void LimpiarPowerUP()
    {
        foreach (GameObject powerUp in powerUpInstances)
        {
            if (powerUp != null)
            {
                Destroy(powerUp);
            }
        }
        powerUpInstances.Clear();
    }

    private void Pause(InputAction.CallbackContext context)
    {
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
}
