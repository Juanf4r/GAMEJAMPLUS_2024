using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject musicMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controles;
    private void Start()
    {
        musicMenu.SetActive(false);
        mainMenu.SetActive(true);
        controles.SetActive(false);
    }
    public void iniciarjuego()
    {
        SceneManager.LoadScene(1);
    }

    public void salirDelJuego()
    {
        Application.Quit();
    }
    public void escenaCreditos()
    {
        SceneManager.LoadScene(2);
    }
    public void music()
    {
        musicMenu.SetActive(true);
        mainMenu.SetActive(false);
        controles.SetActive(false);
    }
    public void controller()
    {
        musicMenu.SetActive(false);
        mainMenu.SetActive(false);
        controles.SetActive(true);
    }
    public void back()
    {
        musicMenu.SetActive(false);
        mainMenu.SetActive(true);
        controles.SetActive(false);
    }
    
}
