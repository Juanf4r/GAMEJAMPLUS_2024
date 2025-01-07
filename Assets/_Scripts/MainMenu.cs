using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject musicMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controles;

    private bool _active = false;

    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey",0);
        ChangeLocale(ID);

        musicMenu.SetActive(false);
        mainMenu.SetActive(true);
        controles.SetActive(false);
    }

    public void ChangeLocale(int localeID)
    {
        if(_active)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }

    private IEnumerator SetLocale(int localeID)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocaleKey",localeID);
        _active = false;
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
