using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelMusic;

    public void continuar()
    {
        Time.timeScale = 1;
        panelPausa.SetActive(false);
        panelGameplay.SetActive(true);
        panelMusic.SetActive(false);
    }

    public void salirGame()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Musica()
    {   
        panelPausa.SetActive(false );
        panelGameplay.SetActive(false);
        panelMusic.SetActive(true);
    }

    public void backM()
    {
        panelGameplay.SetActive(false);
        panelMusic.SetActive(false) ;
        panelPausa.SetActive(true) ;
    }
}
