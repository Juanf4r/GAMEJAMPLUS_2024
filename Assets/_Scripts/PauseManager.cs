using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels = new GameObject[3];
    //0 panelGameplay
    //1 panelPause
    //2 panelSettings

    private bool _active = false;

    private void Start() 
    {
        int ID = PlayerPrefs.GetInt("LocaleKey",0);
    }

#region Settings

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

#endregion

    public void ContinueGame()
    {
        Time.timeScale = 1;

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[0].SetActive(true);
    }

    public void Settings()
    {   
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[2].SetActive(true);
    }

    public void GoBack()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[1].SetActive(true);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }


}
