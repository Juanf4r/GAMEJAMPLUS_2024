using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textJugador1;
    [SerializeField] private TextMeshProUGUI textJugador2;
    [SerializeField] private TextMeshProUGUI textCronometro;
    [SerializeField] private TextMeshProUGUI textGanador;
    [SerializeField] private GameObject PanelGanador;

    [Header("Carnes")]
    [SerializeField] private GameObject[] spawnCarne;
    [SerializeField] private GameObject carne;

    [Header("Comtador")]
    private int contadorJugador1 = 0;
    private int contadorJugador2 = 0;
    private float cronometro;

    [Header("referencias")]
    [SerializeField] private GameObject refPlayer1;
    [SerializeField] private GameObject refPlayer2;
    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;

    [Header("PowerUps")]
    [SerializeField] private GameObject tpUP;   
    [SerializeField] private GameObject velUP;  
    [SerializeField] private GameObject attUP;
    [SerializeField] private GameObject[] spawnPowerUP;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject vacio;

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
    
    private void Start()
    {
        Iniciar();
        LocalizarCarne();
        textGanador.gameObject.SetActive(false);
        PanelGanador.SetActive(false);
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

        if (contadorJugador1 >= 3)
        {
            textJugador1.gameObject.SetActive(false);
            textJugador2.gameObject.SetActive(false);
            textGanador.gameObject.SetActive(true);
            PanelGanador.SetActive(true);
            textGanador.text = "Ganó el jugador 1";
            Time.timeScale = 0;
        }
        else if (contadorJugador2 >= 3)
        {
            textJugador1.gameObject.SetActive(false);
            textJugador2.gameObject.SetActive(false);
            textGanador.gameObject.SetActive(true);
            textGanador.text = "Ganó el jugador 2";
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
        cronometro = cronometro + Time.deltaTime;
        textCronometro.text = cronometro.ToString();
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

            switch (random)
            {
                case 0:
                    Instantiate(tpUP, spawnPoint.transform.position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(velUP, spawnPoint.transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(attUP, spawnPoint.transform.position, Quaternion.identity);
                    break;
            }
        }
    }

}