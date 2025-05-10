using UnityEngine;
using TMPro;
using UI;
public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set;}

    [Header("Texts")]
    public TextMeshProUGUI textJugador1;
    public TextMeshProUGUI textJugador2;
    public TextMeshProUGUI textCronometro;

    [Header("Panels")]
    public GameObject panelContador;
    public GameObject panelGanador1;
    public GameObject panelGanador2;
    public GameObject timerCenter;

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
        GameManager.Instance.startGameAction += HidePlayersPanels;
    }

    void OnDisable()
    {
        GameManager.Instance.startGameAction -= HidePlayersPanels;
    }

    public void HidePlayersPanels()
    {
        panelGanador1.SetActive(false);
        panelGanador2.SetActive(false);
    }

    private void UpdateUI()
    {
        textJugador1.text = "0 / 3";
        textJugador2.text = "0 / 3";  
    }
}
