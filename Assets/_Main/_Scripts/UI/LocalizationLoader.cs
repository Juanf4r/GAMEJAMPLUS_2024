using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

namespace UI
{
    public class LocalizationLoader : MonoBehaviour
    {
        private bool _active = false;

        private void Awake() 
        {
            int ID = PlayerPrefs.GetInt("LocaleKey", 0);    
            ChangeLocale(ID);
        }

        public void ChangeLocale(int localeID){
            if(_active){
                return;
            }

            StartCoroutine(SetLocale(localeID));
        }

        private IEnumerator SetLocale(int localeID){
            _active = true;

            yield return LocalizationSettings.InitializationOperation;

            if(LocalizationSettings.AvailableLocales.Locales[localeID] == null)
            {
                Debug.Log("Locale not placed");
            }
            else
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
                PlayerPrefs.SetInt("LocaleKey",localeID);
                _active = false;

                switch(localeID){
                case 0: Debug.Log("Language Changed to English"); break;
                case 1: Debug.Log("Language Changed to Spanish"); break;
                case 2: Debug.Log("Language Changed to Portuguese"); break;
                }
            }
        }
    }
}

