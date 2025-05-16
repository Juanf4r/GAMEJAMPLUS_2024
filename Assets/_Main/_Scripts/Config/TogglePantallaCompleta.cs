using UnityEngine;

public class TogglePantallaCompleta : MonoBehaviour
{
    private bool changeScreenMode;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            CambiarModoPantalla();
        }
    }

    public void CambiarModoPantalla()
    {
        changeScreenMode = !changeScreenMode;
        if (changeScreenMode)
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        else
        {
            Screen.fullScreen = Screen.fullScreen;
        }
        Debug.Log(changeScreenMode);
    }
}