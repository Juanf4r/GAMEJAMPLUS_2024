using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI textJugador1;
    [SerializeField] private TextMeshProUGUI textJugador2;
    [SerializeField] private TextMeshProUGUI textCronometro;
    [SerializeField] private TextMeshProUGUI textGanador;
    [SerializeField] private GameObject PanelGanador;

    [SerializeField] private GameObject[] spawnCarne;
    [SerializeField] private GameObject carne;

    private int contadorJugador1 = 0;
    private int contadorJugador2 = 0;
    private float cronometro;

    [SerializeField] private GameObject refPlayer1;
    [SerializeField] private GameObject refPlayer2;
    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;

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
    public void GanarJugador2()
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
            textGanador.text = "Gano el juagdor 1";
            Time.timeScale = 0;
        }
        else if (contadorJugador2 >= 3) 
        {
            textJugador1.gameObject.SetActive(false);
            textJugador2.gameObject.SetActive(false);
            textGanador.gameObject.SetActive(true);
            textGanador.text = "Gano el juagdor 2";
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
        int randomIndex = Random.Range(0,3);
        //Debug.Log(randomIndex);
        carne.transform.localPosition = spawnCarne[randomIndex].transform.localPosition;
    }

    private void Iniciar()
    {
        refPlayer1.transform.localPosition = spawn1.transform.localPosition;
        refPlayer2.transform.localPosition = spawn2.transform.localPosition;
        cronometro = 0f;
    }
    private void FixedUpdate()
    {
        cronometro = cronometro + Time.deltaTime;
        textCronometro.text = cronometro.ToString();
        //Debug.Log(cronometro);
        if (cronometro >= 30)
        {
            EndForTime();
        }
    }
    
}