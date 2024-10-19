using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TextMeshPro panelJugador1;
    [SerializeField] private TextMeshPro panelJugador2;
    [SerializeField] private TextMeshPro panelGanador;
    [SerializeField] private TextMeshPro panelCronometro;
    [SerializeField] private GameObject[] spawnCarne;
    [SerializeField] private GameObject carne;
    public int contadorJugador1 = 0;
    public int contadorJugador2 = 0;
    private float cronometro;

    [SerializeField] private GameManager refPlayer1;
    [SerializeField] private GameManager refPlayer2;
    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;

    public void GanarRondaJugador1()
    {
        contadorJugador1 += 1;
        panelJugador1.text = contadorJugador1.ToString();
        GanarJuego();
    }
    public void GanarJugador2()
    {
        contadorJugador2 += 1;
        panelJugador2.text = contadorJugador2.ToString();
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
            panelJugador1.gameObject.SetActive(false);
            panelJugador2.gameObject.SetActive(false);
            panelGanador.text = "Gano el juagdor 1";
            Time.timeScale = 0;
        }
        else if (contadorJugador2 >= 3) 
        {
            panelJugador1.gameObject.SetActive(false);
            panelJugador2.gameObject.SetActive(false);
            panelGanador.text = "Gano el juagdor 2";
            Time.timeScale = 0;
        }
        else
        {
            LocalizarCarne();
            iniciar();
        }

    }
    private void LocalizarCarne()
    {
        int randomIndex = Random.Range(1,10);
        carne.transform.localPosition = spawnCarne[randomIndex].transform.localPosition;
    }

    private void iniciar()
    {
        refPlayer1.transform.localPosition = spawn1.transform.localPosition;
        refPlayer2.transform.localPosition = spawn2.transform.localPosition;
        cronometro = 0f;
    }
    private void FixedUpdate()
    {
        cronometro = cronometro + Time.deltaTime;
        panelCronometro.text = cronometro.ToString();
        //Debug.Log(cronometro);
        if (cronometro >= 30)
        {
            EndForTime();
        }
    }

}